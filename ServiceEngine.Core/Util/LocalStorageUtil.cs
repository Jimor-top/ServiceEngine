using Microsoft.JSInterop;

namespace ServiceEngine.Core.Util
{
    public class LocalStorageUtil: ILocalStorageUtil
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageUtil(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public string SetKey(string key) 
          =>  $"ServiceEngine:{key}";

        public async Task Set<T>(string key, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", (string)SetKey(key), serializedValue);
        }

        public async Task<T> Get<T>(string key)
        {
            string serializedValue = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", (string)SetKey(key));
            if (serializedValue != null)
            {
                return JsonConvert.DeserializeObject<T>(serializedValue);
            }
            return default(T);
        }

        public void Remove(string key)
        {
            _jsRuntime.InvokeVoidAsync("localStorage.removeItem", SetKey(key));
        }

        public void Clear()
        {
            _jsRuntime.InvokeVoidAsync("localStorage.clear");
        }
    }
    public interface ILocalStorageUtil
    {
        public string SetKey(string key);
        public Task Set<T>(string key, T value);
        public Task<T> Get<T>(string key);
        public void Remove(string key);
        public void Clear();
    }
}
