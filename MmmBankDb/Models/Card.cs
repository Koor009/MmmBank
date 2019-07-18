using System.Collections.Generic;
using System;

namespace MmmBankDb.Models
{
    public class Card 
    {
        public Guid Id { get; set; }

        public string Number { get; set; }
        public bool Blocker { get; set; }

        public string AccountId { get; set; }
        public  Account Account { get; set; }
        public  List<Operation> Operations { get; set; }
        public Card()
        {
            Id = Guid.NewGuid();
            Operations = new List<Operation>();
        }



    }
}