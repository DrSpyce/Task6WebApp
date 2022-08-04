namespace Task6WebApp
{
    public class Person
    {
        public Person(int personId)
        {
            this.PersonId = personId;
        }
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Guid { get; set; }
        public string Adress { get; set; }
        public string Gender { get; set; }
    }
}
