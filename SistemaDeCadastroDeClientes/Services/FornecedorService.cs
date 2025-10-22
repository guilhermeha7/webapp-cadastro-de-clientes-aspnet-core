using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaDeCadastroDeClientes.Models.Enums;

namespace SistemaDeCadastroDeClientes.Services
{
    public class FornecedorService : IFornecedorService
    {
        public List<SelectListItem> GetSegmentos()
        {
            return Enum.GetValues(typeof(Segmento)) //Enum.GetValues transforma uma enumeração em um array do tipo object.
                .Cast<Segmento>() //É um método do LINQ que converte cada elemento de uma coleção para o tipo T
                .Select(s => new SelectListItem { Value = ((int)s).ToString(), Text = s.ToString() })
                .ToList();
        }
    }
}
