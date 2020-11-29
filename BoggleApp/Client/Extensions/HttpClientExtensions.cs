using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BoggleApp.Shared.Helpers;

namespace BoggleApp.Client.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<Result<T>> PostAsJsonWithResultAsync<T, TContent>(this HttpClient httpClient, string path, TContent content)
        {
            var result = await httpClient.PostAsJsonAsync(path, content);

            return await GetResultFromHttpResponseMessage<T>(result);
        }


        public static async Task<Result<T>> GetAsResult<T>(this HttpClient httpClient, string path)
        {
            var result = await httpClient.GetAsync(path);

            return await GetResultFromHttpResponseMessage<T>(result);
        }

        private static async Task<Result<T>> GetResultFromHttpResponseMessage<T>(HttpResponseMessage response)
        {
            var isValid = response.IsSuccessStatusCode;
            var value = default(T);
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadFromJsonAsync<T>();
            }

            return new Result<T>
            {
                Value = value,
                IsValid = isValid
            };
        }
    }

}
