using LoginApi.Entities;
using LoginApi.Infraestructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryUser _repositoryUser;
        public UserController(IRepositoryUser repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            Response res = await _repositoryUser.GetUsers();
            if (res.Status == "Ok")
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("{Id:int}")]
        public async Task<ActionResult> GetUsers(int Id)
        {
            Response res = await _repositoryUser.GetUser(Id);
            if (res.Status == "Ok")
                return Ok(res);
            return BadRequest(res);
        }

        // POST api/<UserController>
        [HttpPost("login")]
        public async Task<ActionResult> LoginUserValidation([FromBody] LoginUserEntiy rq)
        {
            Response res = await _repositoryUser.LoginValidation(rq);
            if (res.Status == "Ok")
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] LoginUserEntiy rq)
        {
            Response res = await _repositoryUser.CreateUser(rq);
            if(res.Status == "Ok")
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] LoginUserEntiy rq)
        {
            Response res = await _repositoryUser.Updateuser(rq);
            if (res.Status == "Ok")
                return Ok(res);

            return BadRequest(res);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            Response res = await _repositoryUser.DeleteUser(id);
            if (res.Status == "Ok")
                return Ok(res);

            return BadRequest(res);
        }

    }
}
