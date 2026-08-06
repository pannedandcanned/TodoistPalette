using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using TodoistPalette.Services;
using Windows.UI.ViewManagement;
using static TodoistPalette.Services.SyncData;

namespace TodoistPalette.Pages
{
    internal sealed partial class TaskViewPage : ContentPage
    {
        public string title, project, due_date, description;
        public TaskViewPage(TodoItem task)
        {
            Icon = new("\uE8A5"); // Document icon

            // Populate fields from the provided TodoItem so the page shows the
            // data produced by ParseItems/SyncData.
            title = task.content;
            project = task.project_id;
            due_date = task.due;
            description = task.description;

            // Use the task title for the page metadata
            Title = title ?? "Task";
            Name = title ?? "Task";
        }

        public override IContent[] GetContent()
        {
            return new IContent[] {
                new MarkdownContent($"# {Title} \n ## {project} {due_date} \n ### Description \n {description}"),
            };
        }
    }
}
