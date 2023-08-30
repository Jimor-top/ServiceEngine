using Microsoft.JSInterop;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceEngine.Core.Util
{
    public class SessionStorageUtil: ISessionStorageUtil
    {
        private readonly IJSRuntime _jsRuntime;

        public SessionStorageUtil(IJSRuntime jsRuntime) {
            _jsRuntime = jsRuntime;
        }
        
        public string SetKey(string key)
            => $"ServiceEngine_{key}";

        public async Task Set<T>(string key, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", (string)SetKey(key), serializedValue);
        }

        public async Task<T> Get<T>(string key)
        {
            string serializedValue = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", (string)SetKey(key));
            if (serializedValue != null)
            {
                return JsonConvert.DeserializeObject<T>(serializedValue);
            }
            return default(T);
        }

        public void Remove(string key)
        {
            _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", SetKey(key));
        }

        public void Clear()
        {
            _jsRuntime.InvokeVoidAsync("sessionStorage.clear");
        }
    }
    public interface ISessionStorageUtil
    {
        public string SetKey(string key);
        public Task Set<T>(string key, T value);
        public Task<T> Get<T>(string key);
        public void Remove(string key);
        public void Clear();
    }
}
