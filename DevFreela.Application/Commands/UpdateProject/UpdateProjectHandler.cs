using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.UpdateProject {
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ResultViewModel> {

        private readonly IProjectRepository _repository;
        public UpdateProjectHandler(IProjectRepository repository) {
            _repository = repository;
        }


        public async Task<ResultViewModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken) {
            var project = await _repository.GetById(request.IdProject);

            if (project == null) {
                return ResultViewModel.Error("Projeto não existe.");
            }

            project.Update(request.Title, request.Description, request.TotalCost);

            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
