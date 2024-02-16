using Postgrest.Models;

namespace tokenAuth.Model
{
    public class users : BaseModel
    {
        public int id { get; set; }
        public string first_name { get; set; }

        public string middle_name { get; set; }
        public string last_name { get; set;}
        public string email { get; set; }   
        public string password { get; set; }
        public int phone_code { get; set; }
        public string phone { get; set; }
        public  DateTime inserted_at { get; set; }
        public DateTime updated_at { get; set;}
    }

}
