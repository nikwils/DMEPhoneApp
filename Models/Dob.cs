using System.ComponentModel.DataAnnotations;

namespace DMEPhoneApp.Models
{
    public class dob
    {
        public int id { get; set; }

        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        public int age { get; set; }
    }

}

