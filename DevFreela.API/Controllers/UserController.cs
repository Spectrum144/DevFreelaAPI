using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers {
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase {
        
        // POST api/users
        [HttpPost]
        public IActionResult Post() {
            return Ok();
        }

        [HttpPut("{id}/profile-picture")]
        public IActionResult PostProfilePicture(IFormFile file) {
            var description = $"File: {file.FileName}, Size: {file.Length}";
            
            //Processar a Imagem
            
            return Ok(description);
        }
    }
}
