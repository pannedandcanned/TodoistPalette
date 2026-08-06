using System;

namespace TodoistPalette.Services
{
    // Lightweight TodoItem record used by the list factory and pages.
    // Only include the fields we currently display.
    public record TodoItem(
            string id,
            string user_id,
            string project_id,
            string content,
            string description,
            string priority,
            string due,
            string deadline,
            string parent_id,
            string child_order,
            string section_id,
            string day_order,
            string is_collapsed,
            string[] labels,
            string added_by_uid,
            string assigned_by_uid,
            string responsible_uid,
            string checked_status,
            string is_deleted,
            string added_at,
            string updated_at,
            string completed_at,
            Duration duration
    );

    public record Duration(
        string amount,
        string unit
    );

}
