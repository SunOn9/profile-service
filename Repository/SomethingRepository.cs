using FluentResults;
using Microsoft.EntityFrameworkCore;
using ProfileService.Model.Entity;

namespace ProfileService.Repository;

public class SomethingRepository(DatabaseContext context) 
{
    private readonly DatabaseContext _context = context;

    public async Task<Result<IEnumerable<ProfileService.Something>>> GetAll()
    {
        try
        {
            var items = await _context.Somethings.ToListAsync();
            return Result.Ok(items.Select(item => item.Into()));
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to retrieve data: {ex.Message}");
        }
    }

    public async Task<Result<ProfileService.Something>> GetById(string id)
    {
        try
        {
            Console.WriteLine(id);
            var items = await _context.Somethings.FindAsync(id);
            
            Console.WriteLine(items);

            return items != null ? Result.Ok(items.Into()) : Result.Fail($"Cannot get with condition: {id}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to retrieve data: {ex.Message}");
        }
    }

    public async Task<Result<ProfileService.Something>> Add(SomethingEntity entity)
    {
        try
        {
            await _context.Somethings.AddAsync(entity);
            await _context.SaveChangesAsync();
            var items = await _context.Somethings.FindAsync(entity.Id);
            return items != null ? Result.Ok(items.Into()) : Result.Fail($"Cannot get with condition: {entity.Id}");
        }
        catch (Exception ex)
        {
            // Log the exception (you can use Serilog, NLog, etc.)
            Console.WriteLine($"Exception: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Fail($"Failed to create data: {ex.Message}");
        }
        
    }

    public async Task<Result<ProfileService.Something>> Update(string id, SomethingEntity entity)
    {
        try
        {
            var entityOld = await _context.Somethings.FindAsync(id);
            if (entityOld == null) return Result.Fail($"Cannot get with condition: {id}");
            _context.Somethings.Update(entity);
            await _context.SaveChangesAsync();
            var items = await _context.Somethings.FindAsync(id);
            return items != null ? Result.Ok(items.Into()) : Result.Fail($"Cannot get with condition: {id}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Failed to update data: {ex.Message}");
        }
    }

    public async Task<Result<bool>> Delete(string id)
    {
        var entity = await _context.Somethings.FindAsync(id);
        if (entity == null)
        {
            Result.Fail($"Cannot get with condition: {id}");
        }
        else
        {
            _context.Somethings.Remove(entity);
            await _context.SaveChangesAsync();
        }

        return Result.Ok (true);
    }

    // private SetupQueryCondition(SearchSomething request)
    // {
    //
    // }
}
