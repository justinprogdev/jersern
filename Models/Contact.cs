namespace Editor.Models
{

    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
        public Phonenumber[] PhoneNumbers { get; set; }
    }

    public class Address
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    public class Phonenumber
    {
        public string Type { get; set; }
        public string Number { get; set; }
    }

}
