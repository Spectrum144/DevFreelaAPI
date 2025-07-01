using DevFreela.Application.Models;
using DevFreela.Application.Services;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private readonly DevFreelaDbContext _context;
        private readonly IProjectService _service;
        public ProjectsController(DevFreelaDbContext context, IProjectService service) {
            _context = context;
            _service = service; 
        }

        //Paginação da busca de projetos
        //[HttpGet]
        //public IActionResult Get(string search = "", int page = 0, int size = 3) {
        //    var projects = _context.Projects
        //        .Include(p => p.Client)
        //        .Include(p => p.Freelancer)
        //        .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search) || p.Description.Contains(search)))
        //        .Skip(page * size)
        //        .Take(size)
        //        .ToList();


        //    var model = projects.Select(ProjectItemViewModel.FromEntity).ToList();

        //    return Ok(model); // _configService.GetValue()
        //}
        //

        // GET api/projects?search=crm - implementando a camada Application/Services
        [HttpGet]
        public IActionResult Get(string search = "") {
            var result = _service.GetAll();            

            return Ok(result); // _configService.GetValue()
        }

        ////GET api/projects/123
        //[HttpGet("{id}")]
        //public IActionResult GetById(int id) {
        //    var project = _context.Projects
        //        .Include(p => p.Client)
        //        .Include(p => p.Freelancer)
        //        .Include(p => p.Comments)
        //        .SingleOrDefault(p => p.Id == id);

        //    var model = ProjectViewModel.FromEntity(project);


        //    //throw new Exception();
        //    return Ok(model);
        //}

        //GET api/projects/123
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var result = _service.GetById(id);

            if (!result.IsSuccess) {
                return BadRequest(result);
            }

            //throw new Exception();
            return Ok(result);
        }

        //[HttpPost]
        //public IActionResult Post(CreateProjectInputModel model) {
        //    //if(model.TotalCost < _config.Minimum || model.TotalCost > _config.Maximum) {
        //    //    return BadRequest("Numero fora dos limites.");
        //    //}

        //    var project = model.ToEntity();

        //    _context.Projects.Add(project);
        //    _context.SaveChanges();

        //    return CreatedAtAction(nameof(GetById), new { id = 1 }, model);
        //}

        [HttpPost]
        public IActionResult Post(CreateProjectInputModel model) {

            var result = _service.Insert(model);

            return CreatedAtAction(nameof(GetById), new {id = result.Data}, model);
        }

        //PUT api/projects/123
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, UpdateProjectInputModel model) {

        //    var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        //    if(project == null) {
        //        return NotFound();
        //    }

        //    project.Update(model.Title, model.Description, model.TotalCost);

        //    _context.Projects.Update(project);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        //PUT api/projects/123
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectInputModel model) {

            var result = _service.Update(model);

            if (!result.IsSuccess) {
                return BadRequest(result.Message);
            }
            
            return NoContent();
        }

        // DELETE api/projects/123
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id) {
        //    var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        //    if (project == null) {
        //        return NotFound();
        //    }

        //    project.SetAsDeleted();
        //    _context.Projects.Update(project);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            var result = _service.Delete(id);

            if (!result.IsSuccess) {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        //PUT api/projects/123/start
        //[HttpPut("{id}/start")]
        //public IActionResult Start(int id) {
        //    var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        //    if (project == null) {
        //        return NotFound();
        //    }

        //    project.Start();
        //    _context.Projects.Update(project);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        [HttpPut("{id}/start")]
        public IActionResult Start(int id) {
            var result = _service.Start(id);

            if (!result.IsSuccess) {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        //PUT api/projects/123/complete
        //[HttpPut("{id}/complete")]
        //public IActionResult Complete(int id) {
        //    var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        //    if (project == null) {
        //        return NotFound();
        //    }

        //    project.Complete();
        //    _context.Projects.Update(project);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        [HttpPut("{id}/complete")]
        public IActionResult Complete(int id) {
            var result = _service.Complete(id);

            if (!result.IsSuccess) {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        // POST api/projects/123/comments

        //[HttpPost("{id}/comments")]
        //public IActionResult PostComent(int id, CreateProjectCommentInputModel model) {
        //    var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        //    if (project == null) {
        //        return NotFound();
        //    }

        //    var comment = new ProjectComment(model.Content, model.IdProject, model.IdUser);

        //    _context.ProjectComments.Add(comment);
        //    _context.SaveChanges();

        //    return Ok();
        //}

        [HttpPost("{id}/comments")]
        public IActionResult PostComent(int id, CreateProjectCommentInputModel model) {
            var result = _service.InsertComment(id, model);

            if (!result.IsSuccess) {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

    }
}
