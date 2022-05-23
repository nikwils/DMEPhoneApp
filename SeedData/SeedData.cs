using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DMEPhoneApp.Data;
using DMEPhoneApp.Models;
using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;


namespace DMEPhoneApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DMEPhoneAppContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<DMEPhoneAppContext>>()))
            {
                // Look for any movies.
                if (context.Result.Any())
                {
                    return;   // DB has been seeded
                }
                //static async Task<string> LoadJsonString()
                //{
                //    using (var client = new HttpClient())
                //    {
                //        await client.GetStringAsync(Uri);
                //        return await response.Content.ReadAsStringAsync();
                //    }
                //}


                const string Uri = "https://randomuser.me/api/?inc=name,dob,phone,email,picture,login&noinfo&results=200";
                static string LoadJsonString()
                {
                    using (var wc = new WebClient())
                        return wc.DownloadString(Uri);
                }
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                };

                string LoadJsonString1 = LoadJsonString();


                Rootobject restoredPerson = JsonSerializer.Deserialize<Rootobject>(LoadJsonString1);
                Console.WriteLine(restoredPerson.GetType());


                //newtonsoft
                //Result? restoredPerson2 = JsonConvert.DeserializeObject<Result>(LoadJsonString1);

                
                foreach (result r in restoredPerson.results)
                {
                    context.Result.AddRange(
                    new result
                    {

                        name = new name
                        {
                            title = r.name.title,
                            first = r.name.first,
                            last = r.name.last

                        },
                        email = r.email,
                        login = new login
                        {
                            uuid = r.login.uuid,
                            username = r.login.username,
                            password = r.login.password,
                            salt = r.login.salt,
                            md5 = r.login.md5,
                            sha1 = r.login.sha1,
                            sha256 = r.login.sha256
                        },
                        dob = new dob
                        {
                            date = r.dob.date,
                            age = r.dob.age
                        },
                        phone = r.phone,
                        picture = new picture
                        {
                            large = r.picture.large,
                            medium = r.picture.medium,
                            thumbnail = r.picture.thumbnail
                        }
                    }
                );
                }


                context.SaveChanges();
            }
        }
    }


    public class Rootobject
    {
        public result[] results { get; set; }
    }

}