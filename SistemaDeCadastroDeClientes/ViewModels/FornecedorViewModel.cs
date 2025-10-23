using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaDeCadastroDeClientes.Models;

namespace SistemaDeCadastroDeClientes.ViewModels
{
    public class FornecedorViewModel
    {
        public Fornecedor Fornecedor { get; set; }
        public List<SelectListItem>? Segmentos { get; set; }
        public IFormFile? Foto { get; set; }


    }
}
