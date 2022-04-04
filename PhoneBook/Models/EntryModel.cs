using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Models
{
    public class EntryModel
    {
        [Display(Name = "Entry ID")]
        public long id { get; set; }
        [Display(Name = "PhoneBook ID")]
        public long? phonebookid { get; set; }
        [Display(Name = "Person")]
        public string name { get; set; }
        [Display(Name = "Phone Number")]
        public string phonenumber { get; set; }
    }
}

