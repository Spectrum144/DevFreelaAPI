using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.InsertComment {
    public class InsertCommentHandler : IRequestHandler<InsertCommentCommand, ResultViewModel> {

        private readonly DevFreelaDbContext _context;
        public InsertCommentHandler(DevFreelaDbContext context) {
            _context = context;
        }

        public async Task<ResultViewModel> Handle(InsertCommentCommand request, CancellationToken cancellationToken) {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == request.IdProject);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.IdUser);

            if (project == null) {
                return ResultViewModel<ProjectViewModel>.Error("Projeto não existe.");
            }

            if (user == null) {
                return ResultViewModel<ProjectViewModel>.Error("Usuario não existe.");
            }

            var comment = new ProjectComment(request.Content, request.IdProject, request.IdUser, project, user);

            await _context.ProjectComments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return ResultViewModel.Success();
        }
    }
}
