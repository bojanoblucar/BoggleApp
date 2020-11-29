using System;
using Microsoft.AspNetCore.Components;

namespace BoggleApp.Client.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static void NavigateToIndex(this NavigationManager navigationManager)
        {
            navigationManager.NavigateTo("/");
        }

        public static void NavigateToRoom(this NavigationManager navigationManager, string roomId)
        {
            navigationManager.NavigateTo($"room/{roomId}");
        }
    }
}
