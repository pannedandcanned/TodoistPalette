using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using Windows.ApplicationModel.Email;

namespace TodoistPalette.Pages
{
    internal sealed partial class ConfirmationPage : ContentPage
    {
        public string? body; 

        public ConfirmationPage(string bodyInput = null)
        {
            Icon = new("\uE8A5");
            Title = "API Response";
            Name = "Preview";
            body = bodyInput;
        }

        public override IContent[] GetContent()
        {
            if (string.IsNullOrEmpty(body))
            {
                return [
                new MarkdownContent("# No Response\n Waiting on HTML response."),
                ];
            }
            else
            {
                return [
                new MarkdownContent(body),
                ];
            }
        }
    }
}
