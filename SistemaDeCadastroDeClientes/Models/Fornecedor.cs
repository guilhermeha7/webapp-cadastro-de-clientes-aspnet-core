using SistemaDeCadastroDeClientes.Models.Enums;

namespace SistemaDeCadastroDeClientes.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public Segmento Segmento { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }

    }
}
