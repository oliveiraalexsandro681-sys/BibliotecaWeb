using BibliotecaWeb.Models;
using System.Text.Json;

namespace BibliotecaWeb.Services
{
    public class LivroApiService
    {
        private readonly HttpClient _httpClient;

        public LivroApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenLibraryResponse> BuscarLivros(string busca)
        {
            var resposta = await _httpClient.GetAsync(
            $"https://openlibrary.org/search.json?q={busca}&limit=10");

            var json = await resposta.Content.ReadAsStringAsync();

            var resultado = JsonSerializer.Deserialize<OpenLibraryResponse>(json);

            return resultado ?? new OpenLibraryResponse();
        }
    }
}