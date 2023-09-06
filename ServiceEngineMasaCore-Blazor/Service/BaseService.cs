using Furion.UnifyResult;
using System.Linq.Dynamic.Core;
using System.Net.Sockets;
using WebApiClientCore.Exceptions;
using WebApiClientCore;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Common;
using ServiceEngineMasaCore.Blazor.Option;

namespace ServiceEngineMasaCore.Blazor.Service
{

    /// <summary>
    /// 业务方法封装
    /// </summary>
    public abstract class BaseService
    {
        protected readonly IPopupService Popup;

        public BaseService(IPopupService popup) => Popup = popup;

        /// <summary>
        /// 后续用 DispatchProxy 代替
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        protected async Task<AdminResult<T>> HandleErrorAsync<T>(ITask<AdminResult<T>> task)
        {
            try
            {
                AdminResult<T>? result = await task;
                if (result.Type != "success")
                {
                    await Popup.EnqueueSnackbarAsync(result.Message ?? "Requert error.");
                }
                return result;
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiInvalidConfigException configException)
            {
                await Popup.EnqueueSnackbarAsync("请求配置异常!", AlertTypes.Error);
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiResponseStatusException statusException)
            {
                await Popup.EnqueueSnackbarAsync("响应状态码异常!", AlertTypes.Error);
            }
            catch (HttpRequestException ex) when (ex.InnerException is ApiException apiException)
            {
                await Popup.EnqueueSnackbarAsync("抽象的api异常!", AlertTypes.Error);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                await Popup.EnqueueSnackbarAsync("socket连接层异常!", AlertTypes.Error);
            }
            catch (HttpRequestException ex)
            {
                await Popup.EnqueueSnackbarAsync(ex.Message, AlertTypes.Error);
            }
            catch (Exception ex)
            {
                await Popup.EnqueueSnackbarAsync(ex.Message, AlertTypes.Error);
            }
           
            return new AdminResult<T>();
        }

        protected async Task<bool> ExecuteAsync<T>(ITask<AdminResult<T>> task)
            => (await HandleErrorAsync(task)).Type == "success";
        protected async Task<T?> QueryAsync<T>(ITask<AdminResult<T>> task)
        => (await HandleErrorAsync(task)).Result;

        protected async Task<(int, IEnumerable<T>)> PageAsync<T>(ITask<AdminResult<PageResult<T>>> task)
        {
            var data = await HandleErrorAsync(task);

            return (data.Result?.TotalRows ?? 0, data.Result?.Rows ?? Enumerable.Empty<T>());
        }

        protected async Task<bool> DeleteAsync(IEnumerable<ITask<AdminResult<string>>> tasks)
        {
            var result = new List<AdminResult<string>>();
            foreach (var task in tasks)
            {
                result.Add(await task);
            }

            if (result.Count == 0)
            {
                await Popup.EnqueueSnackbarAsync("操作失败，没有删除数据。");
                return false;
            }
            else if (result.Count == 1)
            {
                if (result[0].Type == "success")
                {
                    await Popup.EnqueueSnackbarAsync(result[0].Message?.ToString() ?? "Requert error.");
                }
                return result[0].Type == "success";
            }
            else
            {
                foreach (var item in result)
                {
                    if (item.Type == "success")
                    {
                        await Popup.EnqueueSnackbarAsync(item.Message?.ToString() ?? "删除失败。");
                    }
                }
                return true;
            }
        }

        protected static T QuickAssign<T>(IEnumerable<Filter>? filters) where T : class, new()
        {
            var result = new T();

            if (filters is not null)
            {
                var type = typeof(T);
                foreach (var item in filters)
                {
                    if (string.IsNullOrWhiteSpace(item.Key))
                    {
                        continue;
                    }

                    var field = type.GetField(item.Key);
                    if (field == null)
                    {
                        continue;
                    }

                    field.SetValue(result, item.Value);
                }
            }
            return result;
        }
    }
}
