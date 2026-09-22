using System.Text.Json.Serialization;

namespace BibliotecaWeb.Models
{
    public class OpenLibraryResponse
    {
        [JsonPropertyName("numFound")]
        public int NumFound { get; set; }

        [JsonPropertyName("docs")]
        public List<OpenLibraryLivro> Docs { get; set; } = new();
    }

    public class OpenLibraryLivro
    {
        [JsonPropertyName("title")]
        public string Titulo { get; set; } = string.Empty;

        [JsonPropertyName("author_name")]
        public List<string> Autores { get; set; } = new();

        [JsonPropertyName("first_publish_year")]
        public int? AnoPublicacao { get; set; }

        [JsonPropertyName("cover_i")]
        public int? CapaId { get; set; }
    }
}