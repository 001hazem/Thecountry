namespace CrudOpreations.Models
{
    public class Region: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int CityId { get; set; }
        public  City Cities { get; set; }

    }
}
