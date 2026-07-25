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
using Windows.Media.Protection.PlayReady;

namespace TodoistPalette
{
    internal sealed partial class TodoistPalettePage : ListPage
    {
        private const string AUTH_TOKEN = "<YOUR_AUTH_TOKEN>";

        private static readonly HttpClient client = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var c = new HttpClient();
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AUTH_TOKEN);
            return c;
        }

        private static async Task Sync()
        {
            try
            {
                using HttpResponseMessage response = client.Get("https://api.todoist.com/api/v1/sync");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.Write("\nException Caught!");
                Debug.Write("Message :{0} ", e.Message);
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
            var sync = Task.Run(Sync);
            var command = new OpenUrlCommand("https://learn.microsoft.com/windows/powertoys/command-palette/adding-commands");
            return new IListItem[]
            {
                new ListItem(sync) {Title = "Sync" }
            };
        }
    }
}
