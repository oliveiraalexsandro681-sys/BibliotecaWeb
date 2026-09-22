
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using BibliotecaWeb.Data;
using BibliotecaWeb.Services;

namespace BibliotecaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LivroApiService _livroApiService;

        public HomeController(
            AppDbContext context,
            LivroApiService livroApiService)
        {
            _context = context;
            _livroApiService = livroApiService;
        }

        public async Task<IActionResult> Index(string busca)
        {
            ViewBag.BuscaAtual = busca;

            // Busca as obras que já estão no nosso SQLite
            var todasAsObras = _context.Obras.ToList();

            // Envia todas as obras para o JavaScript do Modal
            ViewBag.LivrosJson = JsonSerializer.Serialize(todasAsObras);

            // =====================================================
            // BUSCA NA OPEN LIBRARY
            // =====================================================

            var livrosExternos = new List<OpenLibraryLivro>();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                var resposta = await _livroApiService.BuscarLivros(busca);

                livrosExternos = resposta.Docs;

                Console.WriteLine("======================================");
                Console.WriteLine("BUSCA: " + busca);
                Console.WriteLine("RESULTADOS OPEN LIBRARY: " + livrosExternos.Count);
                Console.WriteLine("======================================");
            }

            // Envia os resultados da Open Library para a View
            ViewBag.LivrosExternosJson = JsonSerializer.Serialize(
                livrosExternos,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            // =====================================================
            // FILTRA AS OBRAS DO NOSSO BANCO
            // =====================================================

            List<Obra> obrasParaExibir;

            if (!string.IsNullOrWhiteSpace(busca))
            {
                obrasParaExibir = todasAsObras
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
                // Quando não pesquisou nada, mostra os 5 lançamentos
                obrasParaExibir = todasAsObras
                    .Take(5)
                    .ToList();
            }

            return View(obrasParaExibir);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                            HttpContext.TraceIdentifier
            });
        }
    }
}

