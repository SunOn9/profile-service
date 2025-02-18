using FluentResults;
using ProfileService.Model.Entity;
using ProfileService.Repository;

namespace ProfileService.Business;

public class SomethingService(SomethingRepository repository)
{
    private readonly SomethingRepository _repository = repository;

    public Task<Result<Something>> Create(CreateSomething data)
    {
        var uuid = Guid.NewGuid();
        var createData = new SomethingEntity(){
            Id = uuid.ToString(),
            Name = data.Name,
            Age = data.Age
        };
        
        var response = _repository.Add(createData);
        return response.Result.IsFailed ? 
            Task.FromResult<Result<Something>>(Result.Fail(response.Result.Errors)) :
            Task.FromResult<Result<Something>>(response.Result.Value);
    }
    
    public Task<Result<Something>> GetById(string id)
    {
        var response = _repository.GetById(id);
        return response.Result.IsFailed ? 
            Task.FromResult<Result<Something>>(Result.Fail(response.Result.Errors)) :
            Task.FromResult<Result<Something>>(response.Result.Value);
    }
}