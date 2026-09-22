using System.ComponentModel.DataAnnotations;

namespace BibliotecaWeb.Models
{
    public enum TipoObra
    {
        Livro = 1,
        HQ = 2,
        Manga = 3,
        Revista = 4
    }

    public enum ModalidadeDisponivel
    {
        ApenasCompra = 1,
        ApenasAluguel = 2,
        CompraEAluguel = 3,
        Indisponivel = 4
    }

    public class Obra
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Autor { get; set; } = string.Empty;

        public string FotoUrl { get; set; } = string.Empty;

        public TipoObra Tipo { get; set; }
        public ModalidadeDisponivel Modalidade { get; set; }

        public decimal PrecoCompra { get; set; }
        public decimal PrecoAluguel { get; set; }
        public string LojaOuPlataforma { get; set; } = "Biblioteca Digital";

        public int ContadorPesquisas { get; set; } = 0;
        public int ContadorVendas { get; set; } = 0;
    }
}