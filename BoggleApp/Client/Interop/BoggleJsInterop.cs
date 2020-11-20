using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BoggleApp.Client.Interop
{
    public  static class BoggleJsInterop
    {
        public static async Task InitializeInputField(IJSRuntime js)
        {
            await js.InvokeVoidAsync("initializeInput");
        }

        public static async Task SetBoggleBoardValues(IJSRuntime js, string[] boggleBoardValues)
        {
            var inputList = boggleBoardValues.ToList();
            inputList = inputList.ConvertAll(l => l.ToLower());
            if (inputList.Contains("lj"))
            {
                inputList.Add("l");
                inputList.Add("j");
            }

            if (inputList.Contains("nj"))
            {
                inputList.Add("n");
                inputList.Add("j");
            }

            if (inputList.Contains("dž"))
            {
                inputList.Add("d");
                inputList.Add("ž");
            }
                
            await js.InvokeVoidAsync("setBoggleBoard", inputList);
        }

        public static async Task ClearWordInput(IJSRuntime jS)
        {
            await jS.InvokeVoidAsync("clearBoggleWordInput");
        }
    }
}
