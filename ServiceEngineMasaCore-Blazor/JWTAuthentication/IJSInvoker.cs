namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{

    public interface IJSInvoker : IDisposable
    {
        Guid Id { get; }

        Task ExecuteAsync(params Func<Task>[] callbacks);

        Task ExecuteAsync(params Func<ValueTask>[] callbacks);

        Task<T> ExecuteAsync<T>(T defaultValue, Func<Task<T>> callback);

        Task<T> ExecuteAsync<T>(T defaultValue, Func<ValueTask<T>> callback);

        Task<IReadOnlyList<T>> ExecuteAsync<T>(params Func<Task<T>>[] callbacks);

        Task<IReadOnlyList<T>> ExecuteAsync<T>(params Func<ValueTask<T>>[] callbacks);
    }
}
