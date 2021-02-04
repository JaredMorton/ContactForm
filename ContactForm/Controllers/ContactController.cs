using ContactForm.Data.Models;
using ContactForm.Entities;
using System.Linq;
using System.Web.Mvc;

namespace ContactForm.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Contacts(string search = "")
        {
            using (var context = new ContactDataContext())
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

        public IQueryable<Contact> GetFilteredContacts(ContactDataContext dc, string search)
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

        public ActionResult SaveContact(Contact c)
        {
            ContactDataContext.InsertOrUpdate(c);
            return RedirectToAction("Contacts", "Contact");
        }

        public ActionResult DeleteContact(string contactID)
        {
            ContactDataContext.Delete(contactID);
            return Json("Contact deleted successfully!", JsonRequestBehavior.AllowGet);
        }
    }
}