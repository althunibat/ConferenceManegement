using System.ComponentModel.DataAnnotations;

namespace ConferenceManagementSystem.Conference.Model
{
    public class Attendee {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // NOTE: we validate incoming data (this is filled from an event coming 
        // from the registration BC) so that when EF saves it will fail if it's invalid.
        [RegularExpression(@"[\w-]+(\.?[\w-])*\@[\w-]+(\.[\w-]+)+")]
        public string Email { get; set; }
    }
}