using System.Collections.Generic;

namespace BibliotecaWeb.Models
{
    public class BuscaLivrosViewModel
    {
        public string Busca { get; set; } = string.Empty;

        public List<OpenLibraryLivro> Livros { get; set; } = new();
    }
}