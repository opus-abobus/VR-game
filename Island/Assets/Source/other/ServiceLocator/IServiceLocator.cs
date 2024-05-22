public interface IServiceLocator
{
    void Register<T>(T service) where T : IService;
    void Unregister<T>() where T : IService;
    T Get<T>() where T : IService;
}
