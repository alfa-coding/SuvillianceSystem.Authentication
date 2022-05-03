using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuvillianceSystem.Connections.Infrastructure;
using SuvillianceSystem.Models;
using SuvillianceSystem.Authentication.Infrastructure;

namespace SuvillianceSystem.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPasswordHasher _passwordHasherManager;
        public ICRUD<User> UsersRepo { get; }
        
        public UserController(ILogger<UserController> logger,ICRUD<User> _usersRepo,IPasswordHasher passwordHasherManager)
        {
            _logger = logger;
            _passwordHasherManager = passwordHasherManager;
            UsersRepo = _usersRepo;
        }


        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await this.UsersRepo.GetAll();
        }

        [HttpPost]
        public async Task Post([FromBody]User user)
        {   

            _passwordHasherManager.HashPassword(user);   
            await this.UsersRepo.Insert(user);
        }
    }
}
