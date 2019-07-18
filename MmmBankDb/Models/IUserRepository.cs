using System.Collections.Generic;

namespace MmmBankDb.Models
{
    public interface IUserRepository
    {
        IEnumerable<Account> UsersInfo(string Id);
        IEnumerable<string> UserCards(string Id);
        IEnumerable<Account> ManageCards(string id);
        IEnumerable<Operation> OperationsTransfer(string userId);
        void BlockedCard(bool blockedCard, string userId, string card);
        void BlockedCard(string cardNumber);
        void UnlockCard(string cardNumber);
        IEnumerable<Account> GetAllUsersTransfer();
        bool AddCard(string id);
        void BlockedUser(string id);
        void UnlockUser(string id);
        void SendRequestUser(string userId,bool request);
        bool CheckAccount(string email);
    }
}