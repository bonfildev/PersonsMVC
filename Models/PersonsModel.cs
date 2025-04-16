namespace PersonsMVC.Models
{
    public class PersonsModel
    {
        public Persons Persons { get; set; } = new Persons();
        public List<PersonsTasks> PersonsTasks { get; set; } = new List<PersonsTasks>();
    }
}
