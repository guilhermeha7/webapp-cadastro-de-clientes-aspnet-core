using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaDeCadastroDeClientes.Services
{
    public interface IFornecedorService
    {
        List<SelectListItem> GetSegmentos();
    }
}
