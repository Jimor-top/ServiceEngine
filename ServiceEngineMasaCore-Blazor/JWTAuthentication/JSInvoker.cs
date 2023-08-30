using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using IComponent = Microsoft.AspNetCore.Components.IComponent;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{

    /// <summary>
    /// 函数组件，用于客户端执行
    /// </summary>
    public sealed class JSInvoker : IComponent, IHandleAfterRender, IJSInvoker
    {
        private bool _isDisposed;
        private RenderHandle _renderHandle;
        private readonly ConcurrentQueue<Func<Task>> _funcs;
        private readonly RenderFragment _fragment;
        private readonly Guid _id;

        public Guid Id => _id;

        public JSInvoker()
        {
            _id = Guid.NewGuid();
            _funcs = new();
            _fragment = new RenderFragment(__builder => { });
        }

        [Inject]
        [NotNull]
        ILogger<JSInvoker>? Logger { get; set; }

        void StateHasChanged()
        {
            try
            {
                _renderHandle.Render(_fragment);
            }
            catch
            {
                throw;
            }
        }

        void IComponent.Attach(RenderHandle renderHandle)
        {
            if (_renderHandle.IsInitialized)
            {
                throw new InvalidOperationException("The render handle is already set. Cannot initialize a ComponentBase more than once.");
            }

            _renderHandle = renderHandle;
        }

        Task IComponent.SetParametersAsync(ParameterView parameters) => Task.CompletedTask;

        async Task IHandleAfterRender.OnAfterRenderAsync()
        {
            //不稳定的执行，比较随缘，但下次肯定执行
            var i = _funcs.Count;
            while (i > 0)
            {
                if (_funcs.TryDequeue(out var func))
                {
                    try
                    {
                        await func();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, ex.Message);
                    }
                }
                i--;
            }
        }

        async Task IJSInvoker.ExecuteAsync(params Func<Task>[] callbacks)
        {
            if (_isDisposed)
            {
                _funcs.Clear();
                await Task.CompletedTask;
            }

            if (OperatingSystem.IsBrowser())
            {
                foreach (var callback in callbacks)
                {
                    await callback();
                }
            }
            else
            {
                var source = new TaskCompletionSource();

                _funcs.Enqueue(async () =>
                {
                    foreach (var callback in callbacks)
                    {
                        try
                        {
                            await callback();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, ex.Message);
                        }
                    }
                    source.SetResult();
                });

                await _renderHandle.Dispatcher.InvokeAsync(StateHasChanged);
                await source.Task;
            }
        }

        async Task IJSInvoker.ExecuteAsync(params Func<ValueTask>[] callbacks)
        {
            if (_isDisposed)
            {
                _funcs.Clear();
                await ValueTask.CompletedTask;
            }

            if (OperatingSystem.IsBrowser())
            {
                foreach (var callback in callbacks)
                {
                    await callback();
                }
            }
            else
            {
                var source = new TaskCompletionSource();

                _funcs.Enqueue(async () =>
                {
                    foreach (var callback in callbacks)
                    {
                        try
                        {
                            await callback();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, ex.Message);
                        }
                    }
                    source.SetResult();
                });

                await _renderHandle.Dispatcher.InvokeAsync(StateHasChanged);
                await source.Task;
            }
        }

        async Task<T> IJSInvoker.ExecuteAsync<T>(T defaultValue, Func<Task<T>> callback)
        {
            if (_isDisposed)
            {
                _funcs.Clear();
                return defaultValue;
            }

            if (OperatingSystem.IsBrowser())
            {
                return await callback();
            }
            else
            {
                var source = new TaskCompletionSource<T>();

                _funcs.Enqueue(async () =>
                {
                    var result = await callback();
                    source.SetResult(result);
                });

                await _renderHandle.Dispatcher.InvokeAsync(StateHasChanged);
                return await source.Task;
            }
        }

        async Task<T> IJSInvoker.ExecuteAsync<T>(T defaultValue, Func<ValueTask<T>> callback)
        {
            if (_isDisposed)
            {
                _funcs.Clear();
                return defaultValue;
            }

            if (OperatingSystem.IsBrowser())
            {
                return await callback();
            }
            else
            {
                var source = new TaskCompletionSource<T>();

                _funcs.Enqueue(async () =>
                {
                    var result = await callback();
                    source.SetResult(result);
                });

                await _renderHandle.Dispatcher.InvokeAsync(StateHasChanged);
                return await source.Task;
            }
        }

        async Task<IReadOnlyList<T>> IJSInvoker.ExecuteAsync<T>(params Func<Task<T>>[] callbacks)
        {
            if (_isDisposed)
            {
                _funcs.Clear();
                return Array.Empty<T>();
            }

            if (OperatingSystem.IsBrowser())
            {
                var array = new T[callbacks.Length];
                for (var index = 0; index < callbacks.Length; index++)
                {
                    try
                    {
                        var result = await callbacks[index]();
                        array[index] = result;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, ex.Message);
                    }
                }
                return array;
            }
            else
            {
                var source = new TaskCompletionSource<IReadOnlyList<T>>();

                _funcs.Enqueue(async () =>
                {
                    var array = new T[callbacks.Length];
                    for (var index = 0; index < callbacks.Length; index++)
                    {
                        try
                        {
                            var result = await callbacks[index]();
                            array[index] = result;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, ex.Message);
                        }
                    }
                    source.SetResult(array);
                });

                await _renderHandle.Dispatcher.InvokeAsync(StateHasChanged);
                return await source.Task;
            }
        }

        async Task<IReadOnlyList<T>> IJSInvoker.ExecuteAsync<T>(params Func<ValueTask<T>>[] callbacks)
        {
            if (_isDisposed)
            {
                _funcs.Clear();
                return Array.Empty<T>();
            }

            if (OperatingSystem.IsBrowser())
            {
                var array = new T[callbacks.Length];
                for (var index = 0; index < callbacks.Length; index++)
                {
                    try
                    {
                        var result = await callbacks[index]();
                        array[index] = result;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, ex.Message);
                    }
                }
                return array;
            }
            else
            {
                var source = new TaskCompletionSource<IReadOnlyList<T>>();

                _funcs.Enqueue(async () =>
                {
                    var array = new T[callbacks.Length];
                    for (var index = 0; index < callbacks.Length; index++)
                    {
                        try
                        {
                            var result = await callbacks[index]();
                            array[index] = result;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, ex.Message);
                        }
                    }
                    source.SetResult(array);
                });

                await _renderHandle.Dispatcher.InvokeAsync(StateHasChanged);
                return await source.Task;
            }
        }

        void IDisposable.Dispose()
        {
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}