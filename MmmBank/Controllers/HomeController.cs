using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MmmBankDb.Models;
using Ninject;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace MmmBank.Controllers
{
    public class HomeController : Controller
    {
        IAccountRepository accountRepository;
        IUserRepository userRepository;
        public HomeController()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IAccountRepository>().To<AccountRepository>();
            accountRepository = ninjectKernel.Get<IAccountRepository>();
            ninjectKernel.Bind<IUserRepository>().To<UserRepository>();
            userRepository = ninjectKernel.Get<IUserRepository>();
        }

        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "user")]
        public ActionResult Payments()
        {
            //SelectList card = new SelectList(accountRepository.GetContextCard(User.Identity.GetUserId()), "Number");
            //ViewBag.Card = card;
            return View(accountRepository.GetAccount(User.Identity.GetUserId()));
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult Payments(string send, string get, decimal sum)
        {
            if (accountRepository.CheckCardOnBlock(send))
            {
                if (accountRepository.СardСheck(get))
                {
                    accountRepository.Payment(send, get, User.Identity.GetUserId(), sum);
                    return View("StatysPayOk");
                }
                
                return View("StatysCardError");
            }
            else
            {
                return View("StatysPayError");
            }
           

        }
        public ActionResult StatysPayOk()
        {
            return View();
        }
        public ActionResult StatysCardError()
        {
            return View();
        }
        public ActionResult StatysPayError()
        {
            return View();
        }

        public ActionResult StatysOk()
        {
            return View();
        }
        public ActionResult StatysError()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public ActionResult Employees()
        {

            return View();

        }

        public ActionResult Prepared(string send = null, string get = null, decimal sum = 0)
        {
            ViewBag.Message = $"Whith the card: {send}, transfer to rard: {get}, sum: {sum}";
            ViewBag.Send = send;
            ViewBag.Get = get;
            ViewBag.Sum = sum;

            return View();
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult UsersNotifications(string sort, int? page)
        {

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Sort = sort;
            ViewBag.Page = pageNumber;
            switch (sort)
            {
                case "last name":
                    return View(userRepository.GetAllUsersTransfer().Where(a => a.User.SendUnblock == true).OrderBy(u => u.User.Surname).ToPagedList(pageNumber, pageSize));
                case "last name descending":
                    return View(userRepository.GetAllUsersTransfer().Where(a => a.User.SendUnblock == true).OrderByDescending(u => u.User.Surname).ToPagedList(pageNumber, pageSize));

                default:
                    return View(userRepository.GetAllUsersTransfer().Where(a => a.User.SendUnblock == true).OrderByDescending(u => u.User.Surname).ToPagedList(pageNumber, pageSize));
                }
        }

        [HttpPost]
        [Authorize(Roles = "admin, moderator")]
        public ActionResult UsersNotifications(string userId, string sort, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Sort = sort;
            ViewBag.Page = pageNumber;
            switch (sort)
            {
                case "last name":
                    accountRepository.EnableNotification(userId, false);
                    return View(userRepository.GetAllUsersTransfer().Where(a => a.User.SendUnblock == true).OrderBy(u => u.User.Surname).ToPagedList(pageNumber, pageSize));
                case "last name descending":
                    accountRepository.EnableNotification(userId, false);
                    return View(userRepository.GetAllUsersTransfer().Where(a => a.User.SendUnblock == true).OrderByDescending(u => u.User.Surname).ToPagedList(pageNumber, pageSize));
                default:
                    accountRepository.EnableNotification(userId, false);
                    return View(userRepository.GetAllUsersTransfer().Where(a => a.User.SendUnblock == true).OrderByDescending(u => u.User.Surname).ToPagedList(pageNumber, pageSize));
            }
        }

        [Authorize(Roles = "user")]
        public ActionResult AccountReplenishment()
        {
            return View(accountRepository.GetAccount(User.Identity.GetUserId()));
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult AccountReplenishment(string get, decimal sum)
        {
            if (accountRepository.CheckCardOnBlock(get))
            {
                accountRepository.Repository(get, User.Identity.GetUserId(), sum);
                return View("StatysReplenishmentOk");
            }
            return View("StatysReplenishmentError");
        }
        public ActionResult StatysReplenishmentOk()
        {
            return View();
        }
        public ActionResult StatysReplenishmentError()
        {
            return View();
        }
    }
}