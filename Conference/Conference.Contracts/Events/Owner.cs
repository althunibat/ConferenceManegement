namespace ConferenceManagementSystem.Conference.Contracts.Events
{
    public class Owner {
        public Owner(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get;  }
        public string Email { get;  }

    }
}