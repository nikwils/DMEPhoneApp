using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DMEPhoneApp.Data;
using DMEPhoneApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace DMEPhoneApp.Controllers
{
    public class ResultsController : Controller
    {
        private readonly DMEPhoneAppContext _context;

        public ResultsController(DMEPhoneAppContext context)
        {
            _context = context;
        }

        // GET: Results
        public async Task<IActionResult> Index(string LastName, DateTime DateOfBirthFrom, DateTime DateOfBirthTo, SortState sortOrder = SortState.LastNameAsc, int page = 1)
        {
            int pageSize = 20;

            IQueryable<result> results = _context.Result
                    .Include(r => r.name)
                    .Include(r => r.dob)
                    .Include(r => r.picture)
                    .OrderBy(r => r.name.last);

            if (!string.IsNullOrEmpty(LastName))
            {
                results = results.Where(p => p.name.last.Contains(LastName));
            }
            if (DateOfBirthFrom > DateTime.MinValue)
            {
                results = results.Where(m => m.dob.date <= DateOfBirthTo && m.dob.date >= DateOfBirthFrom);
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.LastNameDesc:
                    results = results.OrderByDescending(s => s.name.last);
                    break;
                case SortState.DateOfBirthAsc:
                    results = results.OrderBy(s => s.dob.date);
                    break;
                case SortState.DateOfBirthDesc:
                    results = results.OrderByDescending(s => s.dob.date);
                    break;
                default:
                    results = results.OrderBy(s => s.name.last);
                    break;
            }

            // пагинация
            var count = await results.CountAsync();
            var items = await results.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel viewModel = new IndexViewModel(
                items,
                new SortViewModel(sortOrder),
                new PageViewModel(count, page, pageSize),
                new FilterViewModel(LastName, DateOfBirthFrom, DateOfBirthTo)
            );
            

            return _context.Result != null ?
                        View(viewModel) :
                        Problem("Entity set 'DMEUserContext.User'  is null.");
            
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginData loginData)
        {
            // находим пользователя 
            result results = _context.Result.FirstOrDefault(p => p.email == loginData.email && p.login.password == loginData.password);
            // если пользователь не найден, отправляем статусный код 401
            if (results is null) return (IActionResult)Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, results.email) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // формируем ответ
            var response = new
            {
                access_token = encodedJwt,
                username = results.email
            };

            return (IActionResult)Results.Json(response);
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Result == null)
            {
                return NotFound();
            }

            var result = await _context.Result
                .FirstOrDefaultAsync(m => m.id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Results/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Phone")] result result)
        {
            if (ModelState.IsValid)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        // GET: Results/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Result == null)
            {
                return NotFound();
            }

            var result = await _context.Result.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Phone")] result result)
        {
            if (id != result.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultExists(result.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        // GET: Results/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Result == null)
            {
                return NotFound();
            }

            var result = await _context.Result
                .FirstOrDefaultAsync(m => m.id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Result == null)
            {
                return Problem("Entity set 'DMEPhoneAppContext.Result'  is null.");
            }
            var result = await _context.Result.FindAsync(id);
            if (result != null)
            {
                _context.Result.Remove(result);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultExists(int id)
        {
          return (_context.Result?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
