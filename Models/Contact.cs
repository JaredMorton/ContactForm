using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactForm.Models
{
    public class ContactContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
    }

    public class Contact
    {
        [Key]
        public int ContactID { get; set; }

        [Description("First Name")]
        public string FirstName { get; set; }

        [Description("Last Name")]
        public string LastName { get; set; }

        public Address Address { get; set; }

    }
}