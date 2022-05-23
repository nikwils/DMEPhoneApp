using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DMEPhoneApp.Models;

namespace DMEPhoneApp.Data
{
    public class DMEPhoneAppContext : DbContext
    {
        public DMEPhoneAppContext (DbContextOptions<DMEPhoneAppContext> options)
            : base(options)
        {
        }

        public DbSet<DMEPhoneApp.Models.result>? Result { get; set; }
    }
}
