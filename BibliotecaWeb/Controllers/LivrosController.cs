using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BibliotecaWeb.Data;
using BibliotecaWeb.Services;

namespace BibliotecaWeb.Controllers
{
    public class LivrosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LivroApiService _livroApiService;

        public LivrosController(
            AppDbContext context,
            LivroApiService livroApiService)
        {
            _context = context;
            _livroApiService = livroApiService;
        }

        // =====================================================
        // CATÁLOGO
        // =====================================================

        public async Task<IActionResult> Index(string busca)
        {
            ViewBag.BuscaAtual = busca;

            // =====================================================
            // OBRAS DO NOSSO BANCO DE DADOS
            // =====================================================

            var todasAsObras = _context.Obras.ToList();

            ViewBag.LivrosJson =
                JsonSerializer.Serialize(todasAsObras);


            // =====================================================
            // BUSCA NA OPEN LIBRARY
            // =====================================================

            var livrosExternos =
                new List<OpenLibraryLivro>();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                var resposta =
                    await _livroApiService.BuscarLivros(busca);

                livrosExternos =
                    resposta.Docs;
            }


            ViewBag.LivrosExternosJson =
                JsonSerializer.Serialize(
                    livrosExternos,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy =
                            JsonNamingPolicy.CamelCase
                    });


            // =====================================================
            // FILTRA O NOSSO BANCO
            // =====================================================

            List<Obra> obrasParaExibir;

            if (!string.IsNullOrWhiteSpace(busca))
            {
                obrasParaExibir =
                    todasAsObras
                        .Where(o =>
                            o != null &&
                            o.Titulo != null &&
                            o.Titulo.Contains(
                                busca,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }
            else
            {
                obrasParaExibir =
                    todasAsObras;
            }


            return View(obrasParaExibir);
        }


        // =====================================================
        // BUSCA EXTERNA
        // =====================================================

        public async Task<IActionResult> BuscaExterna(string busca)
        {
            var resultado =
                new BuscaLivrosViewModel
                {
                    Busca = busca ?? string.Empty
                };

            if (!string.IsNullOrWhiteSpace(busca))
            {
                var resposta =
                    await _livroApiService.BuscarLivros(busca);

                resultado.Livros =
                    resposta.Docs;
            }

            return View(resultado);
        }


        // =====================================================
        // CREATE
        // =====================================================

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Obra obra)
        {
            _context.Obras.Add(obra);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // =====================================================
        // EDIT
        // =====================================================

        public IActionResult Edit(int id)
        {
            var obra =
                _context.Obras.Find(id);

            return View(obra);
        }


        [HttpPost]
        public IActionResult Edit(Obra obra)
        {
            _context.Obras.Update(obra);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // =====================================================
        // DELETE
        // =====================================================

        public IActionResult Delete(int id)
        {
            var obra =
                _context.Obras.Find(id);

            return View(obra);
        }


        [HttpPost]
        public IActionResult Delete(Obra obra)
        {
            _context.Obras.Remove(obra);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // =====================================================
        // DETALHES
        // =====================================================

        public IActionResult Detalhes(int id)
        {
            var obra =
                _context.Obras.Find(id);

            return View(obra);
        }
    }
}