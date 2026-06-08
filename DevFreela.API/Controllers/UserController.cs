using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.Notifications;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DevFreela.API.Controllers {
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase {

        private readonly DevFreelaDbContext _context;
        private readonly IAuthService _authService;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        public UserController(
            DevFreelaDbContext context, 
            IAuthService authService,
            IMemoryCache cache,
            IEmailService emailService) {
                _context = context;
                _authService = authService;
                _cache = cache;
                _emailService = emailService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var user = _context.Users
                .Include(u => u.Skills)
                    .ThenInclude(u => u.Skill)
                .SingleOrDefault(u => u.Id == id);
            
            if(user == null) {
                return NotFound();
            }

            var model = UserViewModel.FromEntity(user);

            return Ok(model);
        }

        // POST api/users
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(CreateUserInputModel model) {
            var hash = _authService.ComputeHash(model.Password);
            var user = new User(model.FullName, model.Email, model.BirthDate, hash, model.Role);

            _context.Users.Add(user);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("{id}/skills")]
        public IActionResult PostSkills(int id, UserSkillsInputModel model) {
            var userSkills = model.SkillIds.Select(s => new UserSkill(id, s)).ToList();

            _context.UserSkills.AddRange(userSkills);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/profile-picture")]
        public IActionResult PostProfilePicture(int id, IFormFile file) {
            var description = $"File: {file.FileName}, Size: {file.Length}";
            
            //Processar a Imagem
            
            return Ok(description);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginInputModel model) {
            var hash = _authService.ComputeHash(model.Password);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == hash);

            if (user == null) {
                var error = ResultViewModel<LoginViewModel?>.Error("Erro de Login.");

                return BadRequest(error);
            }

            // Gerando Token
            var token = _authService.GenerateToken(user.Email, user.Role);

            var viewModel = new LoginViewModel(token);

            var result = ResultViewModel<LoginViewModel>.Success(viewModel);

            return Ok(result);
        }

        // Endpoints para recuperação de Senha
        [HttpPost("password-recovery/request")]
        public async Task<IActionResult> RequestPasswordRecovery(PasswordRecoveryRequestInputModel model) {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);

            if (user == null) return BadRequest();

            var code = new Random().Next(100000, 999999).ToString();

            var cacheKey = $"RecoveryCode:{model.Email}";

            _cache.Set(cacheKey, model, TimeSpan.FromMinutes(10));

            await _emailService.SendAsync(user.Email, "Código de Recuperação de Senha", $"Seu código de recuperação é: {code}");

            return NoContent();
        }

        [HttpPost("password-recovery/validate")]
        public IActionResult ValidateRecoveryCode(ValidateRecoveryCodeInputModel model) {
            var cacheKey = $"RecoveryCode:{model.Email}";

            if (!_cache.TryGetValue(cacheKey, out string? code) ||  code == model.Code) {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("password-recovery/change")]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model) {
            var cacheKey = $"RecoveryCode:{model.Email}";
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);

            if (!_cache.TryGetValue(cacheKey, out string? code) || code == model.Code) {
                return BadRequest();
            }

            // Remover o RecoveryCode para não haver uma nova tentativa com essa chave
            _cache.Remove(cacheKey);
            
            if(user == null) return BadRequest();

            var hash = _authService.ComputeHash(model.NewPassword);
            user.UpdatePassword(hash);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
