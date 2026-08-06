using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TodoistPalette.Pages;

namespace TodoistPalette.Services
{
    internal class SyncData
    {
        #region Properties
        private string _jsonString;
        public TodoItem currentItems;

        public record Properties(
            TodoItem[] items
        );
        #endregion

        public SyncData(string jsonString)
        {
            _jsonString = jsonString;
            try
            {
                currentItems = JsonSerializer.Deserialize<TodoItem>(jsonString);
            }
            catch (Exception e) 
            {
                CommandResult.ShowToast($"Connection failed : {e.Message}");
            }
        }

    }
}
