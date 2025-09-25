using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.UnitTests.Core {
    public class ProjectTests {
        [Fact]
        public void ProjectIsCreated_Start_Sucess() {
            // Arrange
            var project = new Project("Projeto A", "Descrição do Projeto", 1, 2, 10000);

            // Act
            project.Start();

            // Assert
            Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
            Assert.NotNull(project.StartedAt);

            Assert.True(project.Status == ProjectStatusEnum.InProgress);
            Assert.False(project.StartedAt is null);

        }

        [Fact]
        public void ProjectIsInInvalidState_Start_ThrowsException() {
            // Arrange
            var project = new Project("Projeto A", "Descrição do Projeto", 1, 2, 10000);
            project.Start();

            // Act
            Action? start = project.Start;

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(start);
            Assert.Equal(Project.INVALID_STATE_MESSAGE, exception.Message);

        }

        [Fact]
        public void ProjectIsCompleted_Start_ThrowsException() {
            // Arrange
            var project = new Project("Projeto A", "Descrição do Projeto", 1, 2, 10000);
            project.Start();

            // Act
            //project.Complete();
            Action? complete = project.Complete;

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(complete);
            Assert.Equal(Project.INVALID_STATE_MESSAGE, exception.Message);
            //Assert.Equal(project.Status, ProjectStatusEnum.Completed);


        }

    }
}
