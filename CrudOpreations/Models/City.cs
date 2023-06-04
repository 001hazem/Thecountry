namespace CrudOpreations.Models
{
    public class City: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int CountryId { get; set; }
        public  Country Countries { get; set; }
        public List<Region> Regions { get; set; }

    }
}
