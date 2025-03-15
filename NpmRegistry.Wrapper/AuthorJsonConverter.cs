using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace NpmRegistry.Wrapper
{
    public class AuthorJsonConverter : JsonConverter<Author>
    {
        public override Author? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);

            var root = document.RootElement;
            if(root.ValueKind == JsonValueKind.String)
            {
                var author = new Author();
                string? name = root.GetString();
                if(!string.IsNullOrEmpty(name))
                {
                    author.Name = name;
                }
                return author;
            }
            else if(root.ValueKind == JsonValueKind.Object)
            {
                return root.Deserialize<Author>();
            }

            return new Author();
        }

        public override void Write(Utf8JsonWriter writer, Author value, JsonSerializerOptions options)
        {
        }
    }
}
