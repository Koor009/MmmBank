using System;

namespace MmmBankDb.Models
{
    public class Operation 
    {
        public Guid Id { get; set; }

        public int NumberOfTransfer { get; set; }

        public string CardId { get; set; }
        public  Card Card { get; set; }
        public decimal Amount { get; set; }
        public string Transfer { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Operation()
        {
            Id = Guid.NewGuid();
        }
    }
}