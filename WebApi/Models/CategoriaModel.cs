namespace WebApi.Models
{
    public class CategoriaModel : BaseModel
    {
        public int IdSistema { get; set; }

        public required string Nome { get; set; }
    }
}
