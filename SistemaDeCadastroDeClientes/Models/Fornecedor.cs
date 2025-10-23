using SistemaDeCadastroDeClientes.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeCadastroDeClientes.Models
{
    public class Fornecedor
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "O CNPJ está incompleto")]
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }

        [Required]
        public Segmento Segmento { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O CEP está incompleto")]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O Endereço é obrigatório")]
        [StringLength(255)]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        //Campo opcional
        [StringLength(500)]
        public string? CaminhoDaFoto { get; set; }

    }
}
