using Microsoft.EntityFrameworkCore;

namespace PersistentGlobalConfig;

internal class GlobalConfigDal<T> : IGlobalConfigDal<T>
    where T: class, IGlobalConfig, new()
{
    private readonly DbContext _dbContext;
    private readonly VersionByEnum _versionBy;
    private readonly DbSet<T> _repository;

    public GlobalConfigDal(DbContext dbContext, VersionByEnum versionBy)
    {
        _dbContext = dbContext;
        _versionBy = versionBy;
        _repository = dbContext.Set<T>();
    }

    public async Task<T> Get()
    {
        return _versionBy switch
        {
            VersionByEnum.NoVersion => await _repository.FirstOrDefaultAsync() ?? new T(),
            VersionByEnum.TimeStamp => await _repository.OrderByDescending(x => x.Changed).FirstOrDefaultAsync() ?? new T(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public async Task<T> Update(T updated)
    {
        switch (_versionBy)
        {
            case VersionByEnum.NoVersion when await _repository.AnyAsync():
                var existing = await Get();
                updated.Id = existing.Id;
                updated.Changed = DateTimeOffset.Now;
                _dbContext.Entry(existing).CurrentValues.SetValues(updated);
                await _dbContext.SaveChangesAsync();
                return updated;
            case VersionByEnum.NoVersion:
            case VersionByEnum.TimeStamp:
                updated.Id = 0;
                updated.Changed = DateTimeOffset.Now;
                await _repository.AddAsync(updated);
                await _dbContext.SaveChangesAsync();
                return updated;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
