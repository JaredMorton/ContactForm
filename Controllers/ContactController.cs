using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ContactForm.Models;

namespace ContactForm.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Contacts(string search = "")
        {
            SeedContacts();

            using (var context = new ContactContext())
            { 
                IQueryable<Contact> rtn = string.IsNullOrEmpty(search) 
                    ? from temp in context.Contacts select temp 
                    : GetFilteredContacts(context, search);
                return View(rtn.ToList());
            }
        }

        public PartialViewResult Contact()
        {
            return PartialView("_Contact", new Contact());
        }

        public IQueryable<Contact> GetFilteredContacts(ContactContext dc, string search)
        {    
            var contacts = (from a in dc.Contacts
                                where
                                        a.ContactID.ToString().Contains(search) ||
                                        a.FirstName.Contains(search) ||
                                        a.LastName.Contains(search) ||
                                        a.Address.Street.Contains(search) ||
                                        a.Address.City.Contains(search) ||
                                        a.Address.State.Contains(search) ||
                                        a.Address.Zip.Contains(search)
                                select a);

            return contacts;           
        }

        public void SeedContacts()
        {
            using (var context = new ContactContext())
            {
                if (context.Database.Exists())
                    return;

                context.Database.CreateIfNotExists();

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
            }
        }

        public ActionResult SaveContact(Contact c)
        {
            InsertOrUpdate(c);
            return RedirectToAction("Contacts", "Contact");
        }

        public ActionResult DeleteContact(string contactID)
        {
            using (var context = new ContactContext())
            {
                var contact = context.Contacts.FirstOrDefault(x => x.ContactID.ToString() == contactID);
                if (contact != null)
                {
                    context.Entry(contact).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }

            return Json("Contact deleted successfully!", JsonRequestBehavior.AllowGet);
        }

        private void InsertOrUpdate(Contact contact)
        {
            using (var context = new ContactContext())
            {
                context.Entry(contact).State = contact.ContactID == 0 ?
                                           EntityState.Added :
                                           EntityState.Modified;

                context.SaveChanges();
            }
        }
    }
}