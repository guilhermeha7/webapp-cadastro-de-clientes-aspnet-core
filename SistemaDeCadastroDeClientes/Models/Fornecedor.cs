using SistemaDeCadastroDeClientes.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeCadastroDeClientes.Models
{
    public class Fornecedor
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [Required]
        public Segmento Segmento { get; set; }

        [Required]
        [StringLength(8)]
        public string Cep { get; set; }

        [Required]
        [StringLength(255)]
        public string Endereco { get; set; }

        //Campo opcional
        [StringLength(500)]
        public string CaminhoDaFoto { get; set; }

    }
}
