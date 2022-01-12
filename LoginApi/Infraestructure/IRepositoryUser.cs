using LoginApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginApi.Infraestructure
{
    public interface IRepositoryUser
    {
        public Task<Response> CreateUser(LoginUserEntiy userRequest);
        public Task<Response> LoginValidation(LoginUserEntiy userRequest);
        public Task<ListUsers> GetUsers();
        public Task<LoginUserEntiy> GetUser(int Id);
        public Task<Response> Updateuser(LoginUserEntiy userRequest);

        public Task<Response> DeleteUser(int Id);
    }
}
