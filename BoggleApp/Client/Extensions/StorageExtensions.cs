using System;
using System.Threading.Tasks;
using BlazorBrowserStorage;

namespace BoggleApp.Client.Extensions
{

    public static class StorageExtensions
    {
        public static async Task<T> GetItemModified<T>(this ISessionStorage storage, string key)
        {
            try
            {
                return await storage.GetItem<T>(key).AsTask();
            }
            catch
            {
                return default;
            }
        }

        public static async Task<T> GetItemModified<T>(this ILocalStorage storage, string key)
        {
            try
            {
                return await storage.GetItem<T>(key);
            }
            catch
            {
                return default;
            }
        }
    }
}
