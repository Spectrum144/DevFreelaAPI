using DevFreela.Application.Models;
using DevFreela.Application.Notification.ProjectCreated;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Persistence.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.InsertProject {
    public class InsertProjectHandler : IRequestHandler<InsertProjectCommand, ResultViewModel<int>>{

        //private readonly DevFreelaDbContext _context;
        //private readonly IMediator _mediator;
        private readonly IProjectRepository _repository;
        public InsertProjectHandler(IProjectRepository repository) {            
            _repository = repository;            
        }

        public async Task<ResultViewModel<int>> Handle(InsertProjectCommand request, CancellationToken cancellationToken) {
            var project = request.ToEntity();

            var id = await _repository.Add(project);

            return ResultViewModel<int>.Success(id);
        }
    }
}
