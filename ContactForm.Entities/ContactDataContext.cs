using ContactForm.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ContactForm.Entities
{
    public class ContactDataContext : DbContext
    {
        public ContactDataContext() : base("ContactDB")
        {
            Database.SetInitializer(new ContactDBInitializer());
        }

        public DbSet<Contact> Contacts { get; set; }

        public static void InsertOrUpdate(Contact contact)
        {
            using (var context = new ContactDataContext())
            {
                context.Database.CreateIfNotExists();

                context.Entry(contact).State = contact.ContactID == 0 ?
                                           EntityState.Added :
                                           EntityState.Modified;

                context.SaveChanges();
            }
        }

        public static void Delete(string contactID)
        {
            using (var context = new ContactDataContext())
            {
                var contact = context.Contacts.FirstOrDefault(x => x.ContactID.ToString() == contactID);
                if (contact != null)
                {
                    context.Entry(contact).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
        }
    }

    public class ContactDBInitializer : CreateDatabaseIfNotExists<ContactDataContext>
    {
        protected override void Seed(ContactDataContext context)
        {
            IList<Contact> defaultContacts = new List<Contact>
            {
                new Contact()
                {
                    FirstName = "Jared",
                    LastName = "Morton",
                    Address = new Address
                    {
                        Street = "15624 Dasher",
                        City = "Allen Park",
                        State = "MI",
                        Zip = "48101"
                    }
                },

                new Contact()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Address = new Address
                    {
                        Street = "98 Forest Green",
                        City = "Somewhere",
                        State = "MI",
                        Zip = "48182"
                    }
                }
            };

            context.Contacts.AddRange(defaultContacts);

            base.Seed(context);
        }

    }
}