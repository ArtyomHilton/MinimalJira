using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalJira.Application.DTOs;
using MinimalJira.Application.UseCases.Task.AddTask;
using MinimalJira.Application.UseCases.Task.DeleteTask;
using MinimalJira.Application.UseCases.Task.GetTaskById;
using MinimalJira.Application.UseCases.Task.GetTasks;
using MinimalJira.Application.UseCases.Task.UpdateTask;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Host.Controllers;
using MinimalJira.Host.DTOs.Request;
using MinimalJira.Host.Mappers;
using Moq;
using Xunit;

namespace MinimalJira.Tests;

public class TaskControllerTests
{
    private readonly TaskController _controller;

    public TaskControllerTests()
    {
        _controller = new TaskController();
    }

    [Fact]
    public async Task AddTask_ValidDataTest()
    {
        var taskId = Guid.Parse("C42D18EB-FDE1-4DB4-96B4-62D4A04B28D0");
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");

        var useCaseMock = new Mock<IAddTaskUseCase>();
        var request = new AddTaskRequest("Задача1", "Описание", false, projectId);

        useCaseMock.Setup(useCase => useCase.ExecuteAsync(request.ToCommand(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskId);

        var validationResult = new ValidationResult();
        var mockValidator = new Mock<IValidator<AddTaskRequest>>();
        mockValidator.Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var controllerResult =
            await _controller.AddTask(request, useCaseMock.Object, mockValidator.Object, CancellationToken.None);

        var result = Assert.IsType<CreatedAtActionResult>(controllerResult);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task AddTask_NotValidDataTest()
    {
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");

        var useCaseMock = new Mock<IAddTaskUseCase>();
        var request = new AddTaskRequest("", "Описание", false, projectId);

        var validationResult = new ValidationResult(new List<ValidationFailure>()
        {
            new ValidationFailure("Title", "Название задачи не должно быть пустым!")
        });

        var validatorMock = new Mock<IValidator<AddTaskRequest>>();
        validatorMock.Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var controllerResult =
            await _controller.AddTask(request, useCaseMock.Object, validatorMock.Object, CancellationToken.None);

        var result = Assert.IsType<BadRequestObjectResult>(controllerResult);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task AddTask_ProjectNotFoundDataTest()
    {
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");

        var request = new AddTaskRequest("", "Описание", false, projectId);
        var command = request.ToCommand();
        var useCaseMock = new Mock<IAddTaskUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Проект c Id: {command.ProjectId} не найден!"));

        var validationResult = new ValidationResult();

        var validatorMock = new Mock<IValidator<AddTaskRequest>>();
        validatorMock.Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _controller.AddTask(request, useCaseMock.Object, validatorMock.Object, CancellationToken.None));

        Assert.Equal($"Проект c Id: {command.ProjectId} не найден!", exception.Message);
    }

    [Fact]
    public async Task GetTasks_Test()
    {
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");

        var request = new GetTasksRequest(true, projectId);

        var useCaseMock = new Mock<IGetTasksUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(request.ToQuery(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TaskDataResponse>());

        var controllerResult = await _controller.GetTasks(request, useCaseMock.Object, CancellationToken.None);

        var result = Assert.IsType<OkObjectResult>(controllerResult);
        Assert.Equal(new List<TaskDataResponse>(), result.Value);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task GetTask_FoundTaskTest()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var taskData = new TaskDataResponse()
        {
            Id = taskId,
            Title = "Title",
            Description = "ProjectDescription",
            CreatedAt = DateTime.UtcNow,
            IsComplete = true
        };

        var query = TaskMappers.ToQuery(taskId);
        var useCaseMock = new Mock<IGetTaskByIdUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskData);

        var controllerResult = await _controller.GetTask(taskId, useCaseMock.Object, CancellationToken.None);
        var exceptedData = taskData.ToResponse();

        var result = Assert.IsType<OkObjectResult>(controllerResult);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(exceptedData, result.Value);
    }

    [Fact]
    public async Task GetTask_NotFoundTaskTest()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");

        var query = TaskMappers.ToQuery(taskId);
        var useCaseMock = new Mock<IGetTaskByIdUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Задача c Id: {query.TaskId} не найдена!"));

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _controller.GetTask(taskId, useCaseMock.Object, CancellationToken.None));

        Assert.Equal($"Задача c Id: {query.TaskId} не найдена!", exception.Message);
    }

    [Fact]
    public async Task UpdateProject_ValidDataTest()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");

        var request = new UpdateTaskRequest("Задача1", "Описание", true, projectId);

        var validationResult = new ValidationResult();
        var validatorMock = new Mock<IValidator<UpdateTaskRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var useCaseMock = new Mock<IUpdateTaskUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(request.ToCommand(projectId), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var controllerResult = await _controller.UpdateTask(taskId, request, useCaseMock.Object,
            validatorMock.Object,
            CancellationToken.None);

        var result = Assert.IsType<NoContentResult>(controllerResult);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }

    [Fact]
    public async Task UpdateProject_NotValidDataTest()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");

        var request = new UpdateTaskRequest("", "Описание", true, projectId);

        var validationResult = new ValidationResult(new List<ValidationFailure>()
        {
            new ValidationFailure("Title", "Название задачи не должно быть пустым!")
        });
        
        var validatorMock = new Mock<IValidator<UpdateTaskRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var useCaseMock = new Mock<IUpdateTaskUseCase>();

        var controllerResult = await _controller.UpdateTask(taskId, request, useCaseMock.Object,
            validatorMock.Object,
            CancellationToken.None);

        var result = Assert.IsType<BadRequestObjectResult>(controllerResult);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    
    [Fact]
    public async Task UpdateTask_NotFoundTaskTest()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");
    
        var request = new UpdateTaskRequest("Задача", "Описание", true, projectId);

        var command = request.ToCommand(taskId);

        var useCaseMock = new Mock<IUpdateTaskUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"{nameof(Domain.Entities.Task)} с Id: {command.Id} не найдена!"));

        var validateData = new ValidationResult();
        var validatorMock = new Mock<IValidator<UpdateTaskRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validateData);

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _controller.UpdateTask(taskId, request, useCaseMock.Object, validatorMock.Object,
                CancellationToken.None));
        Assert.Equal($"{nameof(Domain.Entities.Task)} с Id: {command.Id} не найдена!", exception.Message);
    }
    
    [Fact]
    public async Task UpdateTask_NotFoundProjectTest()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");
        var projectId = Guid.Parse("7B521A0D-0C1B-4BC3-9D18-286FD2CE6BD5");
    
        var request = new UpdateTaskRequest("Задача", "Описание", true, projectId);

        var command = request.ToCommand(taskId);

        var useCaseMock = new Mock<IUpdateTaskUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"{nameof(Domain.Entities.Project)} с Id: {command.Id} не найдена!"));

        var validateData = new ValidationResult();
        var validatorMock = new Mock<IValidator<UpdateTaskRequest>>();
        validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validateData);

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _controller.UpdateTask(taskId, request, useCaseMock.Object, validatorMock.Object,
                CancellationToken.None));
        Assert.Equal($"{nameof(Domain.Entities.Project)} с Id: {command.Id} не найдена!", exception.Message);
    }
    
    [Fact]
    public async Task DeleteTask_Test()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");
    
        var command = TaskMappers.ToCommand(taskId);
    
        var useCaseMock = new Mock<IDeleteTaskUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    
        var controllerResult = await _controller.DeleteTask(taskId, useCaseMock.Object, CancellationToken.None);
    
        var result = Assert.IsType<NoContentResult>(controllerResult);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }
    
    [Fact]
    public async Task DeleteTask_TaskNotFoundTest()
    {
        var taskId = Guid.Parse("D87A5554-C541-4681-B28F-2E3B71954B92");
    
        var command = TaskMappers.ToCommand(taskId);
    
        var useCaseMock = new Mock<IDeleteTaskUseCase>();
        useCaseMock.Setup(useCase => useCase.ExecuteAsync(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"{nameof(Domain.Entities.Task)} c Id: {command.Id} не найден!"));
    
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _controller.DeleteTask(taskId, useCaseMock.Object, CancellationToken.None));
    
        Assert.Equal($"{nameof(Domain.Entities.Task)} c Id: {command.Id} не найден!", exception.Message);
    }
}