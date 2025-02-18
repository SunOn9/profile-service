namespace ProfileService.Model.Validator;
using FluentResults;

public abstract class BaseValidator
{
    public class BaseRequest<T>
    {
        public required string AppInfo { get; init; }
        public string? UserId { get; init; }
        public required T Data { get; set; }
    }
    
    public static async Task<Result<bool>> ValidateRequest<T>(BaseRequest<T> request)
    {
        var validateAppInfoResp = await ValidateAppInfo(request.AppInfo);
        if (validateAppInfoResp.IsFailed)
        {
            return Result.Fail(validateAppInfoResp.Errors);
        }

        var validateUserIdResp = await ValidateUserId(request.UserId);
        if (validateUserIdResp.IsFailed)
        {
            return Result.Fail(validateUserIdResp.Errors);
        }
        
        // base on T and get that request

        return Result.Ok(true);
    }

    private static Task<Result<bool>> ValidateAppInfo(string appInfo)
    {
        if (string.IsNullOrWhiteSpace(appInfo))
        {
            return Task.FromResult<Result<bool>>(Result.Fail("AppInfo is required."));
        }
        return Task.FromResult(Result.Ok(true));
    }

    private static Task<Result<bool>> ValidateUserId(string? userId)
    {
        if (userId is { Length: < 5 })
        {
            return Task.FromResult<Result<bool>>(Result.Fail("UserId must be at least 5 characters long."));
        }
        return Task.FromResult(Result.Ok(true));
    }
}