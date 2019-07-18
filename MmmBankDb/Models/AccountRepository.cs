using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace MmmBankDb.Models
{
    /// <summary>
    /// class AccountRepository
    /// describes the interaction with the database and operations
    /// </summary>
    public sealed class AccountRepository : IAccountRepository
    {
        /// <summary>
        /// method AddCardAndAccount
        /// created new card
        /// </summary>
        public void AddCardAndAccount(string addUser)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var accountBank = new Account() { Balance = 1000, UserId = addUser };
                db.Accounts.Add(accountBank);

                Random rand = new Random();
                int n = 16;
                string num = "";
                for (int i = 0; i < n; i++)
                {
                    num += Convert.ToString(rand.Next(0, 10));
                }

                var card = new Card() { Blocker = false, Number = num, Account = accountBank };
                num = "";
                db.Cards.Add(card);
                var operation = new Operation() { Card = card, Transfer = "Get 1000", Amount = 1000,CreateDateTime = DateTime.UtcNow, NumberOfTransfer = 1 };
                db.Operations.Add(operation);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// method GetContextCard
        /// set id account
        /// </summary>
        /// <returns>
        /// card
        /// </returns>
        public IEnumerable<Card> GetContextCard(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Cards.Include(a => a.Account).Where(u => u.AccountId == id);
            }
        }
        /// <summary>
        /// method GetAccount
        /// set Id user
        /// </summary>
        /// <returns>
        /// get user
        /// </returns>
        public IEnumerable<Account> GetAccount(string Id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Accounts.Include(u => u.Cards).Include(a=>a.User).Where(u => u.UserId == Id).ToList();
            }
        }

        /// <summary>
        /// method Payment
        /// money transfer
        /// </summary>
        public void Payment(string sendCash, string getCash, string id,decimal sum)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var sendCart = db.Cards.Include(b=>b.Account).Where(n => n.Number == sendCash).FirstOrDefault(a => a.Account.UserId == id);
                var getCard = db.Cards.Include(b => b.Account).FirstOrDefault(n => n.Number == getCash);
                

                if (sendCart != null&& getCard!=null)
                {
                    if (getCard.Blocker == false&&sendCart.Blocker==false)
                    {
                        if (sendCart.Account.Balance - sum >= 0)
                        {
                            sendCart.Account.Balance -= sum;
                            getCard.Account.Balance += sum;
                            db.Operations.Add(new Operation() { Card = sendCart, Amount = sendCart.Account.Balance, Transfer = $"From card:{sendCart.Number}, transfer to card: {getCard.Number}: {sum}",CreateDateTime = DateTime.UtcNow, NumberOfTransfer = db.Operations.Count(n=>n.Card.Number==sendCash)+1});
                            db.Operations.Add(new Operation() { Card = getCard, Amount = getCard.Account.Balance, Transfer = $"Get with card{getCard.Number}: amount of money {sum} To card {sendCart.Number}", CreateDateTime = DateTime.UtcNow, NumberOfTransfer= db.Operations.Count(n=>n.Card.Number==getCash)+1});
                            db.SaveChanges();
                        }
                    }            
                }
            }
        }

        /// <summary>
        /// method Repository
        /// gets all the news
        /// </summary>

        public void Repository(string getCash, string id, decimal sum)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var getCard = db.Cards.Include(b => b.Account).FirstOrDefault(n => n.Number == getCash);
                if (getCard != null)
                {
                    if (getCard.Blocker == false)
                    {
                            getCard.Account.Balance += sum;
                            db.Operations.Add(new Operation() { Card = getCard, Amount = getCard.Account.Balance, Transfer = $"Amount of money {sum} To card {getCard.Number}", CreateDateTime = DateTime.UtcNow, NumberOfTransfer = db.Operations.Count(n => n.Card.Number == getCash) + 1 });
                            db.SaveChanges();

                    }
                }
            }
        }

        /// <summary>
        /// method ShowNotification
        /// </summary>
        /// <returns>
        /// all notify all users
        /// </returns>
        public IEnumerable<Account> ShowNotification()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Accounts.Include(u => u.Cards).Include(a => a.User).Where(a => a.User.SendUnblock == true).OrderBy(o => o.User.Email);
            }
        }
        /// <summary>
        /// method EnableNotification
        /// send notification for unblock card
        /// </summary>
        public void EnableNotification(string userId,bool notification)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Users.Where(u => u.Id == userId).FirstOrDefault().SendUnblock = notification;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// method СardСheck
        /// set number of card
        /// </summary>
        /// <returns>
        /// have a card
        /// </returns>
        public bool СardСheck(string number)
        {
            using (ApplicationDbContext db =new ApplicationDbContext())
            {
                if (db.Cards.Where(c => c.Number == number).Count()==1)
                {
                    return true;
                }
                else
                {
                    return false;   
                }
            }
        }

        /// <summary>
        /// method CheckCardOnBlock
        /// set number of card
        /// </summary>
        /// <returns>
        /// card was find
        /// </returns>
        public bool CheckCardOnBlock(string number)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Cards.Where(c => c.Number == number).FirstOrDefault().Blocker==false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



    }
}