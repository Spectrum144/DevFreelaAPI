using Azure.Core;
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

namespace DevFreela.Application.Commands.CompleteProject {
    public class CompleteProjectHandler : IRequestHandler<CompleteProjectCommand, ResultViewModel> {

        private readonly IProjectRepository _repository;
        public CompleteProjectHandler(IProjectRepository repository) {
            _repository = repository;
        }
        public async Task<ResultViewModel> Handle(CompleteProjectCommand request, CancellationToken cancellationToken) {
            var project = await _repository.GetById(request.Id);//_context.Projects.SingleOrDefaultAsync(p => p.Id == request.Id);

            if (project == null) {
                return ResultViewModel<ProjectViewModel>.Error("Projeto não existe.");
            }

            project.Complete();

            await _repository.Update(project);

            return ResultViewModel.Success();
        }
    }
}
