using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Models
{
    public class PhoneBookModel
    {
        [Display(Name = "PhoneBook ID")]
        public long id { get; set; }
        [Display(Name = "PhoneBook Name")]
        public string phonebookname { get; set; }       

    }
}
