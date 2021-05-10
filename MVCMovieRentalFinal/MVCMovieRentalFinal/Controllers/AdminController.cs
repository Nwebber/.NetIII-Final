using DataObjectsLayer;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCMovieRentalFinal.Controllers
{
    public class AdminController : Controller
    {
        private UserManager _userManager = new UserManager();
        private AccountManager _accountManager = new AccountManager();


        //---------------------------------- User Section ----------------------------------
        
        public ActionResult Users()
        {
            return View(_userManager.RetrieveUsersByActive());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User u = _userManager.RetrieveUsersByActive().Find(user => user.UserID == id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        //---------------------------------- Edit User Section ----------------------------------

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<UserViewModel> users = _userManager.RetrieveUsersByActive();
            var oldUser = users.Where(ou => ou.UserID == id).FirstOrDefault();

            return View(oldUser);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel newUser)
        {
            List<UserViewModel> users = _userManager.RetrieveUsersByActive();
            var oldUser = users.Where(ou => ou.UserID == newUser.UserID).FirstOrDefault();
            users.Remove(oldUser);
            users.Add(newUser);
            _userManager.UpdateUserProfile(oldUser, newUser);

            return RedirectToAction("Users");

        }

        //---------------------------------- Create User Section ----------------------------------

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel newUser, string password)
        {
            if (ModelState.IsValid)
            {
                _userManager.AddWebUser(newUser, password);

                return RedirectToAction("Users");
            }
            else
            {
                return View(newUser);
            }
        }
        // Not Working Properly

        //---------------------------------- Delete User Section ----------------------------------

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User du = _userManager.RetrieveUsersByActive().Find(u => u.UserID == id);
            if (du == null)
            {
                return HttpNotFound();
            }
            return View(du);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _userManager.DeleteUser(id);

            return RedirectToAction("Users");
        }

        //---------------------------------- Disposal Section ----------------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager = null;
            }
            base.Dispose(disposing);
        }
    }
}