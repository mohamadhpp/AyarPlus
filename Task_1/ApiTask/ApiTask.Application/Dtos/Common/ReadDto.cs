namespace ApiTask.Application.Dtos.Common
{
    public class ReadDto
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}