using Grpc.Core;
using ProfileService.Business;
using ProfileService.Model.Validator;

namespace ProfileService.Services;

public class GreeterService(ILogger<GreeterService> logger, SomethingService somethingService) : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger = logger;
    private readonly SomethingService _somethingService = somethingService;
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
    
    public override async Task<SomethingReply> CreateSomething(CreateSomethingRequest request, ServerCallContext context)
    {
        var validateRequestResp = await BaseValidator.ValidateRequest(new BaseValidator.BaseRequest<CreateSomethingRequest>
        {
            AppInfo = request.AppInfo,
            UserId = request.UserId,
            Data = request
        });

        if (validateRequestResp.IsFailed)
        {
            return new SomethingReply
            {
                Code = 10000,
                Message = validateRequestResp.Errors[0].Message,
                Payload = null
            };
        }
        
        var resp = _somethingService.Create(request.Data);
        if (resp.Result.IsFailed)
        {
            return await Task.FromResult(new SomethingReply
            {
                Code = 10000,
                Message = resp.Result.Errors[0].Message,
                Payload = null
            });
        }
        
        return await Task.FromResult(new SomethingReply
        {
            Code = 200,
            Message = "Success",
            Payload = resp.Result.Value
        });
    }
    
    public override async Task<SomethingReply> GetSomething(GetSomethingRequest request, ServerCallContext context)
    {
        var validateRequestResp = await BaseValidator.ValidateRequest(new BaseValidator.BaseRequest<GetSomething>
        {
            AppInfo = request.AppInfo,
            UserId = request.UserId,
            Data = request.Data
        });

        if (validateRequestResp.IsFailed)
        {
            return new SomethingReply
            {
                Code = 10000,
                Message = validateRequestResp.Errors[0].Message,
                Payload = null
            };
        }        
        var resp = _somethingService.GetById(request.Data.Id);
        if (resp.Result.IsFailed)
        {
            return await Task.FromResult(new SomethingReply
            {
                Code = 10000,
                Message = resp.Result.Errors[0].Message,
                Payload = null
            });
        }
        
        return await Task.FromResult(new SomethingReply
        {
            Code = 200,
            Message = "Success",
            Payload = resp.Result.Value
        });
    }
}