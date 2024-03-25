using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace fuel_manager.Models
{
    [Table("Usuarios")]
    public class Usuario : LinksHATEOS
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [JsonIgnore]
        public string Password { get; set; }
        [Required]
        public Perfil Perfil { get; set; }

        public ICollection<VeiculoUsuarios> Veiculos { get; set; }
       
    }

    public enum Perfil
    {
        [Display(Name ="Administrador")]
        Administrador,
        [Display(Name = "Usuario")]
        Usuario

    }
}
