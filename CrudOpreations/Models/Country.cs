namespace CrudOpreations.Models
{
    public class Country: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<City>Cities{ get; set; }

    }
}
