using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MmmBankDb.Models;
using Ninject;
using PagedList;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MmmBank.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        IUserRepository userRepository;
        public ManageController()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IUserRepository>().To<UserRepository>();
            userRepository = ninjectKernel.Get<IUserRepository>();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize(Roles = ("user"))]
        public ActionResult UserInfo()
        {
            return View(userRepository.UsersInfo(User.Identity.GetUserId()));
        }
        [Authorize(Roles = ("admin, moderator"))]
        [HttpPost]
        public ActionResult UserInfo(string id)
        {
            return View(userRepository.UsersInfo(id));
        }

        [Authorize(Roles = ("user"))]
        public ActionResult ManageUserCard()
        {
            return View(userRepository.ManageCards(User.Identity.GetUserId()));
        }


        [Authorize(Roles = ("user"))]
        public ActionResult ControlCardSecurity()
        {
            return View(userRepository.ManageCards(User.Identity.GetUserId()));
        }

        [HttpPost]
        [Authorize(Roles = ("user"))]
        public ActionResult ManageUserCard(bool enubleBlocker, string card)
        {
            userRepository.BlockedCard(enubleBlocker, User.Identity.GetUserId(), card);
            return View("BlockedCard");
        }

        [Authorize(Roles = ("user"))]
        public ActionResult BlockedCard()
        {
            ViewBag.Message = "Card has been blocked";

            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult SendRequest()
        {
            return View(userRepository.UserCards(User.Identity.GetUserId()));
        }
        [Authorize(Roles = "user")]
        [HttpPost]
        public ActionResult SendRequest(bool request)
        {
            userRepository.SendRequestUser(User.Identity.GetUserId(),request);
            return RedirectToRoute(new { controller = "Manage", action = "ManageUserCard" });
        }

        [Authorize(Roles = ("user"))]
        public ActionResult ShowOperations(string sort, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Sort = sort;
            ViewBag.Page = pageNumber;
            switch (sort)
            {
                case "Data ascending":
                    return View(userRepository.OperationsTransfer(User.Identity.GetUserId()).OrderBy(d => d.CreateDateTime).ToPagedList(pageNumber, pageSize));
                case "Number ascending":
                    return View(userRepository.OperationsTransfer(User.Identity.GetUserId()).OrderBy(d => d.NumberOfTransfer).ToPagedList(pageNumber, pageSize));
                case "Amount ascending":
                    return View(userRepository.OperationsTransfer(User.Identity.GetUserId()).OrderBy(d => d.Amount).ToPagedList(pageNumber, pageSize));
                case "Data descending":
                    return View(userRepository.OperationsTransfer(User.Identity.GetUserId()).OrderByDescending(d => d.CreateDateTime).ToPagedList(pageNumber, pageSize));
                case "Number descending":
                    return View(userRepository.OperationsTransfer(User.Identity.GetUserId()).OrderByDescending(d => d.NumberOfTransfer).ToPagedList(pageNumber, pageSize));
                case "Amount descending":
                    return View(userRepository.OperationsTransfer(User.Identity.GetUserId()).OrderByDescending(d => d.Amount).ToPagedList(pageNumber, pageSize));
                default:
                    return View(userRepository.OperationsTransfer(User.Identity.GetUserId()).OrderByDescending(d => d.CreateDateTime).ToPagedList(pageNumber, pageSize));
            }
           
        }
        public ActionResult ShowOperationsOfUser(string id,string sort, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Sort = sort;
            ViewBag.Page = pageNumber;
            switch (sort)
            {
                case "Data ascending":
                    return View(userRepository.OperationsTransfer(id).OrderBy(d => d.CreateDateTime).ToPagedList(pageNumber, pageSize));
                case "Number ascending":
                    return View(userRepository.OperationsTransfer(id).OrderBy(d => d.NumberOfTransfer).ToPagedList(pageNumber, pageSize));
                case "Amount ascending":
                    return View(userRepository.OperationsTransfer(id).OrderBy(d => d.Amount).ToPagedList(pageNumber, pageSize));
                case "Data descending":
                    return View(userRepository.OperationsTransfer(id).OrderByDescending(d => d.CreateDateTime).ToPagedList(pageNumber, pageSize));
                case "Number descending":
                    return View(userRepository.OperationsTransfer(id).OrderByDescending(d => d.NumberOfTransfer).ToPagedList(pageNumber, pageSize));
                case "Amount descending":
                    return View(userRepository.OperationsTransfer(id).OrderByDescending(d => d.Amount).ToPagedList(pageNumber, pageSize));
                default:
                    return View(userRepository.OperationsTransfer(id).OrderByDescending(d => d.CreateDateTime).ToPagedList(pageNumber, pageSize));
            }

        }

        [Authorize(Roles =("admin, moderator"))]
        public ActionResult ShowUsers(string sort, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Sort = sort;
            ViewBag.Page = pageNumber;
            switch (sort)
            {
                case "name":
                    return View(userRepository.GetAllUsersTransfer().OrderBy(u => u.User.FirstName).ToPagedList(pageNumber, pageSize));
                case "last name":
                    return View(userRepository.GetAllUsersTransfer().OrderBy(u => u.User.Surname).ToPagedList(pageNumber, pageSize));
                case "email":
                    return View(userRepository.GetAllUsersTransfer().OrderBy(u => u.User.Email).ToPagedList(pageNumber, pageSize));
                default:
                    return View(userRepository.GetAllUsersTransfer().OrderBy(u => u.User.FirstName).ToPagedList(pageNumber, pageSize));
            }

        }

        [Authorize(Roles =("user"))]
        public ActionResult AddNewCard()
        {
            string message;
            if (userRepository.AddCard(User.Identity.GetUserId()))
            {
                message = "Card has been added";
            }
            else
            {
                message = "Max limit";
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize(Roles ="admin, moderator")]
        public ActionResult BlockUserCard(string numberCard)
        {
            userRepository.BlockedCard(numberCard);
            ViewBag.Message =  "Card have been blocked";
            return View();
        }
        [Authorize(Roles = "admin, moderator")]
        public ActionResult UnlockUserCard(string numberCard)
        {
            userRepository.UnlockCard(numberCard);
            ViewBag.Message = "Card have been unlocked";
            return View();
        }
        [Authorize(Roles = "admin, moderator")]
        public ActionResult BlockUser(string email)
        {
            userRepository.BlockedUser(email);
            ViewBag.Message = "User has been blocked";
            return View();
        }
        [Authorize(Roles = "admin, moderator")]
        public ActionResult UnlockUser(string email)
        {
            userRepository.UnlockUser(email);
            ViewBag.Message = "User has been unlocked";
            return View();
        }



        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            Error
        }

#endregion
    }
}