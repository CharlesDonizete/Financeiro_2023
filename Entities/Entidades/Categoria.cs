using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entidades
{
    [Table("Categoria")]
    public class Categoria : Base
    {
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [ForeignKey("SistemaFinanceiro")]
        [Column(Order = 1)]
        public int IdSistema { get; set; }
        public virtual SistemaFinanceiro SistemaFinanceiro { get; set; }
    }
}
