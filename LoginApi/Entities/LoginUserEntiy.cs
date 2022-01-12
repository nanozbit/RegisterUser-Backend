using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginApi.Entities
{
    public class LoginUserEntiy : Response
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public DateTime BirdDate { get; set; }

    }

    public class ListUsers : Response
    {
        public List<LoginUserEntiy> ListUser { get; set; }
    }
}
