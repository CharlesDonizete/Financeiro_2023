using Entities.Enums;

namespace WebApi.Models
{
    public class DespesaModel : BaseModel
    {        
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public EnumTipoDespesa TipoDespesa { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public bool Pago { get; set; }
        public bool DespesaAtrasada { get; set; }
        public int IdCategoria { get; set; }
    }
}
