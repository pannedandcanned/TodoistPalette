using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using TodoistPalette.Services;
using Windows.Media.Capture.Core;


public sealed partial class ApiKeyPage : ContentPage
{
    private readonly SecretStore _secretStore = new();

    private readonly AuthForm ApiKeyForm;

    public override IContent[] GetContent() => [ApiKeyForm];

    public ApiKeyPage()
    {
        Name = "Open";
        Title = "Authenticate";
        Icon = new IconInfo("\uE72E");
        ApiKeyForm = new AuthForm(_secretStore);
    }

    internal sealed partial class AuthForm : FormContent
    {
        private readonly SecretStore _secret;

        public AuthForm(SecretStore secret = null)
        {
            _secret = secret ?? new SecretStore();
            TemplateJson = $$"""
{
    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
    "type": "AdaptiveCard",
    "version": "1.6",
    "body": [
        {
            "type": "TextBlock",
            "size": "medium",
            "weight": "bolder",
            "text": "Enter your API key. \n Where to find: <a href='https://app.todoist.com/app/settings/integrations' \a>",
            "horizontalAlignment": "center",
            "wrap": true,
            "style": "heading"
        },
        {
            "type": "Input.Text",
            "label": "AuthKey",
            "style": "text",
            "id": "SimpleVal",
            "isRequired": true,
            "errorMessage": "Auth key is required",
            "placeholder": "Enter your auth key from Todoist"
        }
    ],
    "actions": [
        {
            "type": "Action.Submit",
            "title": "Submit",
            "data": {
                "id": "1234567890"
            }
        }
    ]
}
""";

        }
        private void SaveKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            _secret.SaveApiKey(key);
        }

        public override CommandResult SubmitForm(string payload)
        {
            var formInput = JsonNode.Parse(payload)?.AsObject();
            Debug.WriteLine($"Form submitted with formInput: {formInput}");
            if (formInput == null)
            {
                return CommandResult.ShowToast("Please input an Auth Token!");
            }
            var key = formInput["SimpleVal"]?.GetValue<string>();
            #region Auth
            ApiService _authService = new ApiService(key);
            using var client = _authService.CreateAuthClient(); //need to change so that client is saved and reused to avoid crowding ports
            var data = new[] // thank you https://briancaos.wordpress.com/2024/11/26/c-post-x-www-form-urlencoded-using-httpclient/
            {
                    new KeyValuePair<string, string>("sync_token", "*"),
                    new KeyValuePair<string, string>("resource_types", """["all"]""")
            };
            var content = new FormUrlEncodedContent(data); //cheeky way to convert to json
            using HttpResponseMessage response = Task.Run(async() => await client.PostAsync("https://api.todoist.com/api/v1/sync", content)).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                CommandResult.ShowToast($"Connection OK : Status Code {(int)response.StatusCode}");
                SaveKey(key);
                return CommandResult.GoBack();
            }
            else
            {
                return CommandResult.ShowToast($"Connection failed : {(int)response.StatusCode}");
                return CommandResult.GoBack();
            }
            #endregion
        }
    }
}