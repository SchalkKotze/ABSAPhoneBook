
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Models
{
    public class ViewPhoneBookModel
    {
        [Display(Name = "PhoneBook ID")]
        public long? id { get; set; }
        [Display(Name = "PhoneBook Name")]
        public string phonebookname { get; set; }
        [Display(Name = "Entry ID")]
        public long? id_1 { get; set; }
        [Display(Name = "Entry PhoneBook ID")]
        public long? phonebookid { get; set; }
        [Display(Name = "Person")]
        public string name { get; set; }
        [Display(Name = "Phone Number")]
        public string phonenumber { get; set; }
    }
}
