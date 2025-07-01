using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Services {
    public class ProjectService : IProjectService {
        private readonly DevFreelaDbContext _context;
        public ProjectService(DevFreelaDbContext context) {
            _context = context;
        }

        //Passando os métodos que estavam no controller, para a camada de serviços - Arquitetura Limpa

        // COMPLETE PROJECT METHOD
        public ResultViewModel Complete(int id) {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project == null) {
                return ResultViewModel<ProjectViewModel>.Error("Projeto não existe.");
            }

            project.Complete();
            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }

        // DELETE PROJECT METHOD
        public ResultViewModel Delete(int id) {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project == null) {
                return ResultViewModel<ProjectViewModel>.Error("Projeto não existe.");
            }

            project.SetAsDeleted();
            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }

        //GET ALL PROJECTS
        public ResultViewModel<List<ProjectItemViewModel>> GetAll(string search = "") {
            var projects = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Where(p => !p.IsDeleted)
                .ToList();


            var model = projects.Select(ProjectItemViewModel.FromEntity).ToList();

            return ResultViewModel<List<ProjectItemViewModel>>.Success(model);
        }

        //GET BY ID
        public ResultViewModel<ProjectViewModel> GetById(int id) {
            var project = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.Id == id);

            if(project == null) {
                return ResultViewModel<ProjectViewModel>.Error("Projeto não existe");
            }

            var model = ProjectViewModel.FromEntity(project);

            return ResultViewModel<ProjectViewModel>.Success(model);
        }

        // POST METHOD
        public ResultViewModel<int> Insert(CreateProjectInputModel model) {
            var project = model.ToEntity();

            _context.Projects.Add(project);
            _context.SaveChanges();

            return ResultViewModel<int>.Success(project.Id);
        }

        // POST COMMENT ON PROJECT
        public ResultViewModel InsertComment(int id, CreateProjectCommentInputModel model) {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            var user = _context.Users.SingleOrDefault(u => u.Id == model.IdUser);

            if (project == null) {
                return ResultViewModel<ProjectViewModel>.Error("Projeto não existe.");
            }

            if(user == null) {
                return ResultViewModel<ProjectViewModel>.Error("Usuario não existe.");
            }

            var comment = new ProjectComment(model.Content, model.IdProject, model.IdUser, project, user);

            _context.ProjectComments.Add(comment);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }

        // START PROJECT
        public ResultViewModel Start(int id) {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project == null) {
                return ResultViewModel<ProjectViewModel>.Error("Projeto não existe.");
            }

            project.Start();
            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }

        // PUT METHOD
        public ResultViewModel Update(UpdateProjectInputModel model) {
            var project = _context.Projects.SingleOrDefault(p => p.Id == model.IdProject);

            if (project == null) {
                return ResultViewModel.Error("Projeto não existe.");
            }

            project.Update(model.Title, model.Description, model.TotalCost);

            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }
    }

}
