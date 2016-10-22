using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Kickit.Models
{
    public class Invitor
    {
        public int Id { get; set; }  
            
       [Required(ErrorMessage = "Please enter your name"), Display(Name = "Who are you?"),]
       public string FromName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+",
  ErrorMessage = "Please enter a valid email address")]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "Please enter their name"), Display(Name ="Who do you want to Kickit with?")] 
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Please enter their email address")]
        [RegularExpression(".+\\@.+\\..+",
   ErrorMessage = "Please enter a valid email address")]
        public string ReceiverEmail { get; set; }

        [Required(ErrorMessage = "Please enter a date and time"),  Display(Name = "Enter Date and Time"),]
        public string DateTime1 { get; set; }

        [Required(ErrorMessage = "Please enter a date and time"),  Display(Name = "Enter Date and Time"),]
        public string DateTime2 { get; set; }

        [Required(ErrorMessage = "Please enter a date and time"),  Display(Name = "Enter Date and Time"),]
        public string DateTime3 { get; set; }

    }
}