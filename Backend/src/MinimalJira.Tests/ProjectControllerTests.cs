using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalJira.Application.DTOs;
using MinimalJira.Application.UseCases.Project.AddProject;
using MinimalJira.Application.UseCases.Project.DeleteProject;
using MinimalJira.Application.UseCases.Project.GetProjectById;
using MinimalJira.Application.UseCases.Project.GetProjects;
using MinimalJira.Application.UseCases.Project.UpdateProject;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Host.Controllers;
using MinimalJira.Host.DTOs.Request;
using MinimalJira.Host.Mappers;
using Moq;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MinimalJira.Tests;

public class ProjectControllerTests
{
    private readonly ProjectController _controller;

    public ProjectControllerTests()
    {
        _controller = new ProjectController();
    }

    [Fact]
    public async Task AddProject_ValidDataTest()
    {
        var projectId = Guid.Parse("C42D18EB-FDE1-4DB4-96B4-62D4A04B28D0");

        var useCaseMock = new Mock<IAddProjectUseCase>();
        var command = new AddProjectCommand("Проект", "Описание");

        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectId);

        var request = new AddProjectRequest("Проект", "Описание");

        var validationResult = new ValidationResult();
        var mockValidator = new Mock<IValidator<AddProjectRequest>>();
        mockValidator.Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var controllerResult =
            await _controller.AddProject(request, useCaseMock.Object, mockValidator.Object, CancellationToken.None);

        var result = Assert.IsType<CreatedAtActionResult>(controllerResult);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        useCaseMock.Verify(x => x.ExecuteAsync(It.Is<AddProjectCommand>(cmd =>
                cmd.Name == command.Name && cmd.Description == command.Description),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddProject_NotValidDataTest()
    {
        var useCaseMock = new Mock<IAddProjectUseCase>();

        var request = new AddProjectRequest("", "Описание");

        var validationResult = new ValidationResult(new List<ValidationFailure>()
        {
            new ValidationFailure("Name", "Название проекта не должно быть пустым!")
        });
        var validatorMock = new Mock<IValidator<AddProjectRequest>>();
        validatorMock.Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var controllerResult =
            await _controller.AddProject(request, useCaseMock.Object, validatorMock.Object, CancellationToken.None);

        var result = Assert.IsType<BadRequestObjectResult>(controllerResult);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task GetProjectsTest()
    {
        var query = new GetProjectsQuery(1, 10);
        var useCaseMock = new Mock<IGetProjectsUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProjectDataResponse>());

        var pagination = new Pagination(1, 10);

        var controllerResult = await _controller.GetProjects(pagination, useCaseMock.Object, CancellationToken.None);

        var result = Assert.IsType<OkObjectResult>(controllerResult);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        useCaseMock.Verify(
            useCase => useCase.ExecuteAsync(
                It.Is<GetProjectsQuery>(q => q.Page == query.Page && q.PageSize == query.PageSize),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProject_FoundProjectTest()
    {
        var projectId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var query = new GetProjectByIdQuery(projectId);
        var useCaseMock = new Mock<IGetProjectByIdUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProjectDataResponse()
            {
                Id = projectId,
                Name = "ProjectName",
                Description = "ProjectDescription",
                CreatedAt = DateTime.UtcNow,
                Tasks = new List<TaskDataResponse>()
            });

        var controllerResult = await _controller.GetProject(projectId, useCaseMock.Object, CancellationToken.None);

        var result = Assert.IsType<OkObjectResult>(controllerResult);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        useCaseMock.Verify(useCase =>
            useCase.ExecuteAsync(It.Is<GetProjectByIdQuery>(q => q.Id == projectId),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProject_NotFoundProjectTest()
    {
        var projectId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var query = new GetProjectByIdQuery(projectId);
        var useCaseMock = new Mock<IGetProjectByIdUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Проект c Id: {query.Id} не найден!"));

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _controller.GetProject(projectId, useCaseMock.Object, CancellationToken.None));

        Assert.Equal($"Проект c Id: {query.Id} не найден!", exception.Message);
    }

    [Fact]
    public async Task UpdateProject_ValidDataTest()
    {
        var projectId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var request = new UpdateProjectRequest("проект1", "проект1");

        var validationResult = new ValidationResult();
        var validatorMock = new Mock<IValidator<UpdateProjectRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var useCaseMock = new Mock<IUpdateProjectUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(request.ToCommand(projectId), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var controllerResult = await _controller.UpdateProject(projectId, request, useCaseMock.Object,
            validatorMock.Object,
            CancellationToken.None);

        var result = Assert.IsType<NoContentResult>(controllerResult);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }

    [Fact]
    public async Task UpdateProject_NotValidDataTest()
    {
        var projectId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var request = new UpdateProjectRequest("", "проект1");

        var validationResult = new ValidationResult(new List<ValidationFailure>()
        {
            new ValidationFailure("Name", "Название проекта не должно быть пустым!")
        });
        var validatorMock = new Mock<IValidator<UpdateProjectRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var useCaseMock = new Mock<IUpdateProjectUseCase>();

        var controllerResult = await _controller.UpdateProject(projectId, request, useCaseMock.Object,
            validatorMock.Object,
            CancellationToken.None);

        var result = Assert.IsType<BadRequestObjectResult>(controllerResult);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task UpdateProject_NotFoundProjectTest()
    {
        var projectId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var request = new UpdateProjectRequest("Проект1", "Проект1");

        var command = request.ToCommand(projectId);
        var useCaseMock = new Mock<IUpdateProjectUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Проект c Id: {command.Id} не найден!"));

        var validationResult = new ValidationResult();
        var validatorMock = new Mock<IValidator<UpdateProjectRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _controller.UpdateProject(projectId, request, useCaseMock.Object, validatorMock.Object,
                CancellationToken.None));

        Assert.Equal($"Проект c Id: {command.Id} не найден!", exception.Message);
    }

    [Fact]
    public async Task DeleteProject_Test()
    {
        var projectId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var command = ProjectMappers.ToCommand(projectId);
        
        var useCaseMock = new Mock<IDeleteProjectUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var controllerResult = await _controller.DeleteProject(projectId, useCaseMock.Object, CancellationToken.None);
        
        var result = Assert.IsType<NoContentResult>(controllerResult);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteProject_ProjectNotFoundTest()
    {
        var projectId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var command = ProjectMappers.ToCommand(projectId);
        
        var useCaseMock = new Mock<IDeleteProjectUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Проект c Id: {command.Id} не найден!"));

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _controller.DeleteProject(projectId, useCaseMock.Object, CancellationToken.None));
        
        Assert.Equal($"Проект c Id: {command.Id} не найден!", exception.Message);
    }
}