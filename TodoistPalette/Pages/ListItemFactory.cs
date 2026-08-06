using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.CommandPalette.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using TodoistPalette.Services;

namespace TodoistPalette.Pages
{
    internal static class ListItemFactory
    {
        public static IListItem[] CreateFromSyncJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return Array.Empty<IListItem>();
            }

            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("items", out JsonElement itemsElement) || itemsElement.ValueKind != JsonValueKind.Array)
                {
                    return Array.Empty<IListItem>();
                }

                var items = JsonSerializer.Deserialize<List<TodoItem>>(itemsElement.GetRawText());
                if (items == null || items.Count == 0)
                    return Array.Empty<IListItem>();

                var result = new List<IListItem>(items.Count);
                foreach (var task in items)
                {
                    var page = new TaskViewPage(task);
                    var li = new ListItem(page) { Title = task.content ?? "(No title)" };
                    result.Add(li);
                }

                return result.ToArray();
            }

            catch (Exception)
            {
                return Array.Empty<IListItem>();
            }
        }
    }
}
