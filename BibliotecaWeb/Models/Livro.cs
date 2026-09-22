namespace BibliotecaWeb.Models
{
    public class Livro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }

        public string FotoUrl { get; set; } = string.Empty;
        public int Id { get; set; }

        //Aqui estamos criando nossa classe Livro;
    }
}
