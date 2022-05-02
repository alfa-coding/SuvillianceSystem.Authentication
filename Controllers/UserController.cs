using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuvillianceSystem.Connections.Infrastructure;
using SuvillianceSystem.Models;

namespace SuvillianceSystem.Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        public ICRUD<User> UsersRepo { get; }
        
        public UserController(ILogger<UserController> logger,ICRUD<User> _usersRepo)
        {
            _logger = logger;
            UsersRepo = _usersRepo;
        }


        [HttpGet]
        public IEnumerable<User> Get()
        {
            throw new NotImplementedException();
        }
    }
}
