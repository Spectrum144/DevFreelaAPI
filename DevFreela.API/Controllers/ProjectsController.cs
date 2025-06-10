using DevFreela.API.Models;
using DevFreela.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers {
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase {
        //private readonly FreelanceTotalCostConfig _config;
        //private readonly IConfigService _configService;

        //public ProjectsController(
        //    IOptions<FreelanceTotalCostConfig> options,
        //    IConfigService configService
        //    ) {
        //    _config = options.Value;
        //    _configService = configService;
        //}

        public ProjectsController() {

        }
        
        [HttpGet]
        public IActionResult Get(string search = "") {
            return Ok(); // _configService.GetValue()
        }

        //GET api/projects/123
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            //throw new Exception();
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(CreateProjectInputModel model) {
            //if(model.TotalCost < _config.Minimum || model.TotalCost > _config.Maximum) {
            //    return BadRequest("Numero fora dos limites.");
            //}

            return CreatedAtAction(nameof(GetById), new { id = 1 }, model);
        }

        //PUT api/projects/123
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectInputModel model) {
            return NoContent();
        }

        // DELETE api/projects/123
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            return NoContent();
        }

        //PUT api/projects/123/start
        [HttpPut("{id}/start")]
        public IActionResult Start(int id) {
            return NoContent();
        }

        //PUT api/projects/123/complete
        [HttpPut("{id}/complete")]
        public IActionResult Complete(int id) {
            return NoContent();
        }

        // POST api/projects/123/comments
        [HttpPost("{id}/comments")]
        public IActionResult PostComent(int id, CreateProjectCommentInputModel model) {
            return Ok();
        }

    }
}
