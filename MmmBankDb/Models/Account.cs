using System;
using System.Collections.Generic;
namespace MmmBankDb.Models
{
    public class Account 
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public  List<Card> Cards { get; set; }
        public Account()
        {
            Cards = new List<Card>();
            Id = Guid.NewGuid();
        }

       
    }
}