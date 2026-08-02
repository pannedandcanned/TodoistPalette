// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoistPalette.Services;
using Windows.Media.Protection.PlayReady;

namespace TodoistPalette
{
    internal sealed partial class TodoistPalettePage : ListPage
    {
        //temp for testing, will implement actual thing tmmrw
        private const string AUTH_TOKEN = "";

        private readonly ApiAuthService _authService = new ApiAuthService(AUTH_TOKEN);

        private async Task TestConnection()
        {
            try
            {
                using var client = _authService.CreateAuthenticatedClient();
                using HttpResponseMessage response = await client.GetAsync("https://api.todoist.com/api/v1/sync");
                string result = response.IsSuccessStatusCode? $"Connection OK : {(int)response.StatusCode}": $"Connection failed : {(int)response.StatusCode}";
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Connection exception: {e.Message}");
            }
        }

        public TodoistPalettePage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
            Title = "Command Palette for Todoist";
            Name = "Open";
        }

        public override IListItem[] GetItems()
        {
            return new IListItem[]
            {
                new ListItem(new AnonymousCommand(() => { Task.Run(TestConnection); }) { Result = CommandResult.Dismiss() }) { Title = "Test Connection" }
            };
        }
    }
}
