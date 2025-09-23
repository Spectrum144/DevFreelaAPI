using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.UnitTests.Application {
    public class InsertProjectHandlerTests {
        [Fact]
        public async Task InsertDataAreOk_Insert_Success_NSubstitute() {
            // Arrange
            const int id = 1;
            var repository = Substitute.For<IProjectRepository>();
            repository.Add(Arg.Any<Project>()).Returns(Task.FromResult(id));

            var command = new InsertProjectCommand {
                Title = "Project A",
                Description = "Descrição do Projeto",
                TotalCost = 20000,
                IdClient = 1,
                IdFreelancer = 2
            };

            var handler = new InsertProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(id, result.Data);
            await repository.Received(id).Add(Arg.Any<Project>());
        }
    }
}
