using Microsoft.JSInterop;

namespace ServiceEngineMasaCore.Blazor.Extensions
{

    public static class JSRuntimeExtensions
    {
        /// <summary>
        /// 显示全局加载
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static ValueTask ShowGlobalOverlay(this IJSRuntime js) => js.InvokeVoidAsync("window.showAppOverlay");

        /// <summary>
        /// 关闭全局加载
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static ValueTask HideGlobalOverlay(this IJSRuntime js) => js.InvokeVoidAsync("window.hideAppOverlay");

        /// <summary>
        /// 通知应用已经准备完成
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static ValueTask NotifyAppIsReady(this IJSRuntime js) => js.InvokeVoidAsync("window.notifyAppIsReady");
    }


}
