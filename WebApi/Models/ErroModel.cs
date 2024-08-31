namespace WebApi.Models
{
    public class ErroModel
    {
        public int Status { get; set; }
        public string? Titulo { get; set; }
        public Dictionary<string, string[]> Erros { get; set; }
        public string? TraceId { get; set; }

        public ErroModel()
        {
            Erros = new Dictionary<string, string[]>();
        }
    }
}
