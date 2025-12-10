namespace ApiTask.Domain.Entities.Location
{
    public class Country
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }

        public virtual List<Province>? Provinces { get; set; } = new();
    }
}