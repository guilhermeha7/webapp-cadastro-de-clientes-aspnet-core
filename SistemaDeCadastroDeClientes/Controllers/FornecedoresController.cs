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

        public FornecedoresController(IRepository<Fornecedor> repository, IFornecedorService fornecedorService)
        {
            _repository = repository;
            _fornecedorService = fornecedorService;
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

            FornecedorViewModel fornecedorViewModel = new FornecedorViewModel { Fornecedor = fornecedor, Segmentos = segmentos};

            return View(fornecedorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cnpj,Segmento,Cep,Endereco,CaminhoDaFoto")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                await _repository.CreateAsync(fornecedor);
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cnpj,Segmento,Cep,Endereco,CaminhoDaFoto")] Fornecedor fornecedor)
        {
            if (id != fornecedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.EditAsync(fornecedor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    Fornecedor fornecedorExistente = await _repository.GetByIdAsync(f => f.Id == id);
                    if (fornecedorExistente == null) 
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
            return View(fornecedor);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _repository.GetByIdAsync(f => f.Id == id);
            if (fornecedor != null)
            {
                await _repository.DeleteAsync(fornecedor);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
