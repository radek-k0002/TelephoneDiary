namespace TelephoneDiary
{
    public class Contact
    {
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string info;
        private readonly uint id;

        public Contact(uint id, string firstName, string lastName, string phoneNumber,
                        string email, string info)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.info = info;
            this.id = id;
        }

        public string FirstName { get { return firstName; } }
        public string LastName { get { return lastName; } }
        public string PhoneNumber { get { return phoneNumber; } }
        public string Email { get { return email; } }
        public string Info { get { return info; } }
        public uint ID { get { return id; } }
    }
}