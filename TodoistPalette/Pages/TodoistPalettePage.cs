// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoistPalette.Pages;
using TodoistPalette.Services;
using Windows.Media.Protection.PlayReady;
using Microsoft.VisualBasic.FileIO;

namespace TodoistPalette
{
    internal sealed partial class TodoistPalettePage : ListPage
    {
        // secret storage and API client
        private readonly SecretStore _secretStore = new SecretStore();

        private readonly ApiService _authService;

        private string? result;

        public TodoistPalettePage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
            Title = "Command Palette for Todoist";
            Name = "Open";
            // initialize ApiService with token from SecretStore (may be null)
            _authService = new ApiService(_secretStore.GetApiKey());
        }
        private async Task<string> TestConnection()
        {
            try
            {
                using var client = _authService.CreateAuthClient(); //need to change so that client is saved and reused to avoid crowding ports
                var data = new[] // thank you https://briancaos.wordpress.com/2024/11/26/c-post-x-www-form-urlencoded-using-httpclient/
                {
                    new KeyValuePair<string, string>("sync_token", "*"),
                    new KeyValuePair<string, string>("resource_types", """["all"]""")
                };
                var content = new FormUrlEncodedContent(data); 
                using HttpResponseMessage response = await client.PostAsync("https://api.todoist.com/api/v1/sync", content);
                string httpResult = response.IsSuccessStatusCode? $"Connection OK : {(int)response.StatusCode}": $"Connection failed : {(int)response.StatusCode}";
                result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                result = $"Connection exception: {e.Message}";
                return result; 
            }
        }

        public override IListItem[] GetItems()
        {
            #region Deletion Confirmation
            ConfirmationArgs confirmArgs = new()
            {
                PrimaryCommand = new AnonymousCommand(
                () =>
                {
                    if (!_secretStore.HasApiKey())
                    {
                        ToastStatusMessage t = new("Deletion successful!");
                        t.Show();
                    }
                    else
                    {
                        ToastStatusMessage t = new("Deletion unsuccessful. Try again.");
                        t.Show();
                    }
                })
                {
                    Name = "Confirm",
                    Result = CommandResult.KeepOpen(),
                },
                Title = "API Key Deletion Confirmation",
                Description = "Are you want to delete your key? This will effectively log you out.",
            };
            #endregion
            if (!_secretStore.HasApiKey())
            {
                // need to implement proper startup page
                return new IListItem[]
                {
                    new ListItem(new ApiKeyPage()) {Title = "Authenticate to get Data"},
                };
            }
            else
            {
                // just need it for testing will implement proper async routine
                return new IListItem[]
                {
                    new ListItem(new ResponsePage(TestConnection().GetAwaiter().GetResult())) {Title = "Results?"}, 
                    new ListItem(new AnonymousCommand(() => _secretStore.DeleteApiKey()) { Result = CommandResult.Confirm(confirmArgs) }) {Title = "Reset API Key"}
                };
            }
        }
    }
}
