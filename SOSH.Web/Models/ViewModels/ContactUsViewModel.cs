using SOSH.Web.Models;

namespace SOSH.Web.Models.ViewModels;

public class ContactUsViewModel
{
    public List<Contact> contactList { get; set; }
    public Contact contact { get; set; }
}
