using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using BibliotecaWeb.Data;
using BibliotecaWeb.Services;

namespace BibliotecaWeb.Controllers //Aqui significa que o controller vai receber uma API.
{
    [ApiController]
    [Route("api/[controller]")] // Aqui vamos determinar o nosso caminho da API. o controller atual se chama : LivrosApiController e o [controller] significa : LivrosApi.
    public class LivrosApiController : ControllerBase // Aqui tem a diferença entre o Controller e o ControllerBase, pois quer dizer/mostrar para que usuaremos uma API, pois a API usa o ControllerBase.
    {
        private readonly AppDbContext _context;
        private readonly LivroApiService _livroApiService;

        public LivrosApiController(AppDbContext context, LivroApiService livroApiService)
        {
            _context = context;
            _livroApiService = livroApiService;
        }

        //Abaixo esta todo nosso método GET => Consultar os livros.
        [HttpGet]
        public IActionResult GetLivros()
        {
            var obras = _context.Obras.ToList();

            return Ok(obras);
        }

        [HttpGet("{id:int}")] // Aqui significa que estamos fazendo uma consulta ou querendo pegar algo. exemplo de GET /api/LivrosApi
        public IActionResult GetLivro(int id)
        { // Aqui signifca que criamos um método, quando alguem executar esse GET.

            var obra = _context.Obras.Find(id);

            if (obra == null)
            {
                return NotFound("Obra não encontrada, verifique as informações sobre a obra e tente novamente.");
            }

            return Ok(obra);
            // Acima estamos respondendo que a API foi consultada com sucesso.
        }

        //Abaixo esta todo nosso método POST => Inserir Novas Obras.
        [HttpPost]
        public IActionResult CadastrarObra(Obra obra)
        {
            _context.Obras.Add(obra);
            _context.SaveChanges();

            return Ok(obra);
        }

        //Abaixo esta nosso metodo PUT -> Editar uma obra
        //Abaixo esta todo nosso método PUT => Editar uma obra.
        [HttpPut("{id:int}")]
        public IActionResult EditarObra(int id, Obra obra)
        {
            var obraBanco = _context.Obras.Find(id);

            if (obraBanco == null)
            {
                return NotFound("Obra não encontrada, verifique as informações sobre a obra e tente novamente.");
            }

            obraBanco.Titulo = obra.Titulo;
            obraBanco.Autor = obra.Autor;
            obraBanco.FotoUrl = obra.FotoUrl;
            obraBanco.Tipo = obra.Tipo;
            obraBanco.Modalidade = obra.Modalidade;
            obraBanco.PrecoCompra = obra.PrecoCompra;
            obraBanco.PrecoAluguel = obra.PrecoAluguel;
            obraBanco.LojaOuPlataforma = obra.LojaOuPlataforma;

            _context.SaveChanges();

            return Ok(obraBanco);
        }

        [HttpGet("externa-externa")]
        public async Task<IActionResult> BuscarNaApiExterna(string busca)
        {
            var resultado = await _livroApiService.BuscarLivros(busca);

            return Ok(resultado.Docs);
        }

        //Abaixo esta nosso metodo DELETE -> Excluir uma obra
        //Abaixo esta todo nosso método DELETE => Excluir uma obra.
        [HttpDelete("{id:int}")]
        public IActionResult ExcluirObra(int id)
        {
            var obra = _context.Obras.Find(id);

            if (obra == null)
            {
                return NotFound("Obra não encontrada, verifique as informações sobre a obra e tente novamente.");
            }

            _context.Obras.Remove(obra);
            _context.SaveChanges();

            return Ok("Obra excluída com sucesso.");
        }
    }
}