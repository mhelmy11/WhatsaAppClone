using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WhatsappClone.Service.Helpers
{
    public static class CursorHelper
    {
        public static string Encode<T>(T cursorData)
        {
            var json = JsonSerializer.Serialize(cursorData);
            var bytes = Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

        public static T? Decode<T>(string? base64Cursor)
        {
            if (string.IsNullOrWhiteSpace(base64Cursor)) return default;

            try
            {
                var bytes = Convert.FromBase64String(base64Cursor);
                var json = Encoding.UTF8.GetString(bytes);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default; 
            }
        }
    }
}
