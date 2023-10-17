using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Requests;
using ProductsManagement.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest registerRequest)
        {
            try
            {
                var userDTO = await _service.Register(registerRequest);
                return CreatedAtAction(nameof(GetUser), new { id = userDTO.Id }, userDTO);
            }
            catch (DbUpdateException ex)
            {
                return Conflict("User with given e-mail already exists!");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequest loginRequest)
        {
            var token = await _service.Login(loginRequest);
            if (token == "User with given e-mail does not exist!")
            {
                return NotFound(token);
            }

            if (token == "Wrong password!")
            {
                return BadRequest(token);
            }
            return Ok(token);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            return (await _service.GetUser(id)) is var user ? Ok(user) : NotFound();
        }

    }
}
