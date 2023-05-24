using System.Text.Json.Serialization;

namespace URLShortenerAPI.Models.URLAddress
{
    public class URLAddressesCount
    {
        public int AllURLAddressesCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? UserURLAddressesCount { get; set; }
    }
}
