using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaDeCadastroDeClientes.Context;
using SistemaDeCadastroDeClientes.Models;
using SistemaDeCadastroDeClientes.Repositories;
using SistemaDeCadastroDeClientes.Services;
using SistemaDeCadastroDeClientes.ViewModels;

namespace SistemaDeCadastroDeClientes.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly IRepository<Fornecedor> _repository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FornecedoresController(IRepository<Fornecedor> repository, IFornecedorService fornecedorService, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _fornecedorService = fornecedorService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _repository.GetByIdAsync(f => f.Id == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        public IActionResult Create()
        {
            Fornecedor fornecedor = new Fornecedor();
            List<SelectListItem> segmentos = _fornecedorService.GetSegmentos();

            FornecedorViewModel fornecedorViewModel = new FornecedorViewModel { Fornecedor = fornecedor, Segmentos = segmentos };

            return View(fornecedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel viewModel)
        {
            viewModel.Fornecedor.Cnpj = viewModel.Fornecedor.Cnpj.Replace("/", "").Replace(".", "").Replace("-", "");
            viewModel.Fornecedor.Cep = viewModel.Fornecedor.Cep.Replace("-", "");

            if (ModelState.IsValid)
            {
                if (viewModel.Foto != null)
                {
                    if (viewModel.Foto.ContentType == "image/png")
                    {
                        string pastaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "fotos_fornecedores");
                        if (!Directory.Exists(pastaUploads))
                        {
                            Directory.CreateDirectory(pastaUploads);
                        }

                        string nomeUnico = Guid.NewGuid().ToString() + ".png";
                        string caminhoFisico = Path.Combine(pastaUploads, nomeUnico);

                        using (var fileStream = new FileStream(caminhoFisico, FileMode.Create))
                        {
                            await viewModel.Foto.CopyToAsync(fileStream);
                        }

                        viewModel.Fornecedor.CaminhoDaFoto = "/uploads/fotos_fornecedores/" + nomeUnico;
                    }
                    else
                    {
                        ModelState.AddModelError("Foto", "O arquivo deve ser uma imagem .PNG.");
                        viewModel.Segmentos = _fornecedorService.GetSegmentos();
                        return View(viewModel);
                    }
                }

                await _repository.CreateAsync(viewModel.Fornecedor);
                return RedirectToAction(nameof(Index));
            }

            viewModel.Segmentos = _fornecedorService.GetSegmentos();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _repository.GetByIdAsync(f => f.Id == id);
            var segmentos = _fornecedorService.GetSegmentos();

            if (fornecedor == null)
            {
                return NotFound();
            }

            FornecedorViewModel fornecedorViewModel = new FornecedorViewModel { Fornecedor = fornecedor, Segmentos = segmentos };
            return View(fornecedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FornecedorViewModel viewModel)
        {
            //Retira a máscara
            viewModel.Fornecedor.Cnpj = viewModel.Fornecedor.Cnpj.Replace("/", "").Replace(".", "").Replace("-", "");
            viewModel.Fornecedor.Cep = viewModel.Fornecedor.Cep.Replace("-", "");

            if (id != viewModel.Fornecedor.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                viewModel.Segmentos = _fornecedorService.GetSegmentos();
                return View(viewModel);
            }

            var fornecedorExistente = await _repository.GetByIdAsync(f => f.Id == id);
            if (fornecedorExistente == null)
            {
                return NotFound();
            }

            fornecedorExistente.Nome = viewModel.Fornecedor.Nome;
            fornecedorExistente.Cnpj = viewModel.Fornecedor.Cnpj;
            fornecedorExistente.Segmento = viewModel.Fornecedor.Segmento;
            fornecedorExistente.Cep = viewModel.Fornecedor.Cep;
            fornecedorExistente.Endereco = viewModel.Fornecedor.Endereco;

            if (viewModel.Foto != null && viewModel.Foto.Length > 0)
            {
                if (viewModel.Foto.ContentType != "image/png")
                {
                    ModelState.AddModelError("Foto", "A foto deve estar no formato PNG.");
                    viewModel.Segmentos = _fornecedorService.GetSegmentos();
                    return View(viewModel);
                }

                var pastaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "fotos_fornecedores");

                if (!Directory.Exists(pastaUploads))
                    Directory.CreateDirectory(pastaUploads);

                string nomeArquivo;
                if (!string.IsNullOrEmpty(fornecedorExistente.CaminhoDaFoto))
                {
                    nomeArquivo = Path.GetFileName(fornecedorExistente.CaminhoDaFoto);
                    if (string.IsNullOrWhiteSpace(nomeArquivo))
                        nomeArquivo = Guid.NewGuid().ToString() + ".png";
                }
                else
                {
                    nomeArquivo = Guid.NewGuid().ToString() + ".png";
                }

                var caminhoNovo = Path.Combine(pastaUploads, nomeArquivo);

                try
                {
                    await using var stream = new FileStream(caminhoNovo, FileMode.Create);
                    await viewModel.Foto.CopyToAsync(stream);

                    fornecedorExistente.CaminhoDaFoto = "/uploads/fotos_fornecedores/" + nomeArquivo;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao salvar a nova imagem: " + ex.Message);
                    viewModel.Segmentos = _fornecedorService.GetSegmentos();
                    return View(viewModel);
                }
            }

            try
            {
                await _repository.EditAsync(fornecedorExistente);
            }
            catch (DbUpdateConcurrencyException)
            {
                var fornecedorAindaExiste = await _repository.GetByIdAsync(f => f.Id == id);
                if (fornecedorAindaExiste == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _repository.GetByIdAsync(f => f.Id == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var fornecedor = await _repository.GetByIdAsync(f => f.Id == id);
            if (fornecedor != null)
            {
                if (!string.IsNullOrWhiteSpace(fornecedor.CaminhoDaFoto))
                {
                    try
                    {
                        var nomeArquivo = Path.GetFileName(fornecedor.CaminhoDaFoto);
                        if (!string.IsNullOrEmpty(nomeArquivo))
                        {
                            var caminhoNaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/fotos_fornecedores", nomeArquivo);

                            if (System.IO.File.Exists(caminhoNaUploads))
                            {
                                System.IO.File.Delete(caminhoNaUploads);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Não bloqueia a exclusão do registro caso ocorra erro ao apagar o arquivo.
                    }
                }

                await _repository.DeleteAsync(fornecedor);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
