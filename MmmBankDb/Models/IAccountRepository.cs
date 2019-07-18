using System.Collections.Generic;

namespace MmmBankDb.Models
{
    public interface IAccountRepository
    {
        void AddCardAndAccount(string addUser);
        IEnumerable<Account> GetAccount(string Id);
        void Payment(string sendCash, string getCash, string Id, decimal sum);
        IEnumerable<Card> GetContextCard(string id);
        void Repository(string getCash, string id, decimal sum);
        void EnableNotification(string userId, bool notification);
        bool СardСheck(string number);
        bool CheckCardOnBlock(string number);
    }
}