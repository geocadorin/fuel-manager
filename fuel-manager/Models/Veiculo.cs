using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fuel_manager.Models
{
    [Table("Veiculos")]
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Placa { get; set; }
        [Required]
        public int anoFabricacao { get; set; }
        [Required]
        public int anoModelo { get; set; }
        public ICollection<Consumo> Consumos { get; set; }

    }
}
