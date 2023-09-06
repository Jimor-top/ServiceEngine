using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;


namespace ServiceEngine.Core.Util
{
    public class DownloadFileUtil
    {
        [Inject]
        IJSRuntime _jsRuntime { get; set; }
        private static object _lock = new object();

        public DownloadFileUtil(IJSRuntime jsRuntime) {
            _jsRuntime = jsRuntime;
        }
        public async Task DownloadFile(HttpResponseMessage httpResponse) {
            // 获取 Content-Disposition 头部信息
            ContentDispositionHeaderValue.TryParse(httpResponse.Content.Headers.ContentDisposition.ToString(), out var contentDisposition);
            // 判断文件名的编码方式
            // 解析 filename=
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(contentDisposition.FileName))
                fileName = contentDisposition.FileName;
            // 解析 filename*=UTF-8''
            const string fileNameStarPrefix = "UTF-8''";
            if (contentDisposition.Parameters.Any(p => p.Name.Equals("filename*")))
            {
                var fileNameStarParam = contentDisposition.Parameters.First(p => p.Name.Equals("filename*"));
                if (fileNameStarParam.Value.StartsWith(fileNameStarPrefix))
                {
                    var fileNameStarValue = fileNameStarParam.Value.Substring(fileNameStarPrefix.Length);
                    var decodedFileName = Uri.UnescapeDataString(fileNameStarValue);
                    var encodedBytes = Encoding.UTF8.GetBytes(decodedFileName);
                    fileName = Encoding.UTF8.GetString(encodedBytes);
                }
            }
            if (fileName != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await httpResponse?.Content?.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    // 调用 JavaScript interop 方法下载文件
                    await _jsRuntime.InvokeVoidAsync("downloadFile", fileName, Convert.ToBase64String(memoryStream.ToArray()));
                }
            }
        }
    }
}
