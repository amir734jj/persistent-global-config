namespace PersistentGlobalConfig;

public interface IGlobalConfigDal<T>
    where T: class, IGlobalConfig, new()
{
    Task<T> Get();

    Task<T> Update(T updated);
}