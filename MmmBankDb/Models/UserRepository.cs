using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MmmBankDb.Models
{
    /// <summary>
    /// class UserRepository
    /// describes the interaction with the database and operations
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// method UsersInfo
        /// get user by id
        /// </summary>
        /// <returns>
        /// Account
        /// </returns>
        public IEnumerable<Account> UsersInfo(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Accounts.Include(a=>a.User).Include(c=>c.Cards).Where(u=>u.UserId== id).ToList();
            }
        }
        /// <summary>
        /// method UserCards
        /// get user by account
        /// </summary>
        /// <returns>
        /// Account
        /// </returns>
        public IEnumerable<string> UserCards(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Cards.Include(a => a.Account.UserId == id).Select(s => s.Number).ToList();
            }
        }

        /// <summary>
        /// method ManageCards
        /// get user by id
        /// </summary>
        /// <returns>
        /// account from DB
        /// </returns>
        public IEnumerable<Account> ManageCards(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Accounts.Include(u => u.Cards).Include(a => a.User).Where(u => u.UserId == id).ToList();
            }
        }
        /// <summary>
        /// method BlockedCard
        /// block card by number
        /// </summary>
        public void BlockedCard(bool blockedCard,string userId, string card)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               db.Cards.Where(o => o.Account.UserId == userId && o.Number == card).FirstOrDefault().Blocker= blockedCard;
               db.SaveChanges();
            }
        }
        /// <summary>
        /// method BlockedCard
        /// block card by number
        /// </summary>
        public void BlockedCard(string cardNumber)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Cards.Where(o => o.Number==cardNumber).FirstOrDefault().Blocker = true;
                db.SaveChanges();
            }
        }
        /// <summary>
        /// method BlockedUser
        /// block user by id
        /// </summary>
        public void BlockedUser(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Users.Where(u=>u.Email == id).FirstOrDefault().StatusBlockedUser = true;
                db.SaveChanges();
            }
        }
        /// <summary>
        /// method UnlockUser
        /// unlock user by id
        /// </summary>
        public void UnlockUser(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Users.Where(u => u.Email == id).FirstOrDefault().StatusBlockedUser = false;
                db.SaveChanges();
            }
        }


        public void UnlockCard(string cardNumber)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Cards.Where(o => o.Number == cardNumber).FirstOrDefault().Blocker = false;
                db.SaveChanges();
            }
        }

        public IEnumerable<Operation> OperationsTransfer(string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Operations.Where(o => o.Card.Account.UserId == userId).ToList();
            }
        }
        public IEnumerable<Account> GetAllUsersTransfer()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Accounts.Include(u=>u.User).Include(u => u.Cards).ToList();
            }
        }
        public bool AddCard(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                Random rand = new Random();
                int n = 16;
                string num = "";
                for (int i = 0; i < n; i++)
                {
                    num += Convert.ToString(rand.Next(0, 10));
                }
                
                db.Cards.Add(new Card { Blocker = true,Number = num, Account = db.Accounts.Where(u => u.UserId == id).FirstOrDefault() });
                num = "";
                if (db.Cards.Count(c=>c.Account.UserId==id) <= 5)
                {
                    db.SaveChanges();
                    return true;
                }               
            }
            return false;
        }
        public void SendRequestUser(string userId,bool request)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Users.Where(u => u.Id == userId).FirstOrDefault().SendUnblock = request;
                db.SaveChanges();
            }
        }
        class RegisterViewModel
        { }
        public bool CheckAccount(string email)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Accounts.Include(u => u.User).Where(u => u.User.Email == email).FirstOrDefault()!=null)
                {
                    if (db.Accounts.Include(u => u.User).Where(u => u.User.Email == email).FirstOrDefault().User.StatusBlockedUser == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

    }
}