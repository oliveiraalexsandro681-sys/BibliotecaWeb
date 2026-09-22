using BibliotecaWeb.Models;
using BibliotecaWeb.Data;
using BibliotecaWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LivroApiService _livroApiService;

        public AdminController(
            AppDbContext context,
            LivroApiService livroApiService)
        {
            _context = context;
            _livroApiService = livroApiService;
        }

        // =====================================================
        // PÁGINA INICIAL DO ADMIN
        // =====================================================

        public IActionResult Index()
        {
            var obras = _context.Obras.ToList();

            ViewBag.Obras = obras;

            return View(new BuscaLivrosViewModel());
        }

        // =====================================================
        // BUSCAR NA OPEN LIBRARY
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Buscar(string busca)
        {
            var resultado = new BuscaLivrosViewModel
            {
                Busca = busca ?? string.Empty
            };

            // Busca as obras já cadastradas no nosso banco
            var obras = _context.Obras.ToList();

            ViewBag.Obras = obras;

            if (!string.IsNullOrWhiteSpace(busca))
            {
                var resposta =
                    await _livroApiService.BuscarLivros(busca);

                resultado.Livros =
                    resposta.Docs;
            }

            return View("Index", resultado);
        }

        // =====================================================
        // ADICIONAR OBRA AO CATÁLOGO
        // =====================================================

        [HttpPost]
        public IActionResult Adicionar(
            string Titulo,
            string Autor,
            int? CapaId)
        {
            var obra = new Obra
            {
                Titulo = Titulo,
                Autor = Autor,

                FotoUrl = CapaId.HasValue
                    ? $"https://covers.openlibrary.org/b/id/{CapaId}-M.jpg"
                    : ""
            };

            return View("Cadastrar", obra);
        }

        // =====================================================
        // SALVAR OBRA NO BANCO DE DADOS
        // =====================================================

        [HttpPost]
        public IActionResult Salvar(Obra obra)
        {
            _context.Obras.Add(obra);

            _context.SaveChanges();

            return RedirectToAction("Index", "Livros");
        }

        // =====================================================
        // EDITAR OBRA
        // =====================================================

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var obra = _context.Obras.Find(id);

            if (obra == null)
            {
                return NotFound();
            }

            return View("Editar", obra);
        }

        // =====================================================
        // SALVAR EDIÇÃO
        // =====================================================

        [HttpPost]
        public IActionResult Editar(Obra obra)
        {
            var obraBanco = _context.Obras.Find(obra.Id);

            if (obraBanco == null)
            {
                return NotFound();
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

            return RedirectToAction("Index", "Livros");
        }

        // =====================================================
        // EXCLUIR OBRA
        // =====================================================

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var obra = _context.Obras.Find(id);

            if (obra == null)
            {
                return NotFound();
            }

            _context.Obras.Remove(obra);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}