﻿using CorTabernaclChoir.Attributes;
using CorTabernaclChoir.Common;
using System.Web.Mvc;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.Services;
using CorTabernaclChoir.Common.ViewModels;
using CorTabernaclChoir.Interfaces;

namespace CorTabernaclChoir.Controllers
{
    [Title(nameof(Resources.AdminTitle), nameof(Resources.AdminTitle))]
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly IEmailService _emailService;

        public AdminController(IEmailService emailService, ILogger logger, IMessageContainer messageContainer)
            : base(logger, messageContainer)
        {
            _emailService = emailService;
        }

        [Route("~/Admin/")]
        public ActionResult Index()
        {
            var model = new AdminViewModel
            {
                ContactEmailAddresses = _emailService.GetAddresses()
            };

            return View(model);
        }

        [HttpPost]
        [Route("~/Admin/AddEmailAddress")]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult AddEmailAddress(ContactEmail email)
        {
            _emailService.AddAddress(email);

            MessageContainer.AddSaveSuccessMessage();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("~/Admin/RemoveEmailAddress")]
        [ValidateAntiForgeryToken]
        public ViewResult RemoveEmailAddress(ContactEmail email)
        {
            return View(string.Empty, string.Empty, email);
        }

        [HttpPost]
        [Route("~/Admin/ConfirmRemoveEmailAddress")]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult ConfirmRemoveEmailAddress(int id)
        {
            _emailService.RemoveAddress(id);

            MessageContainer.AddSaveSuccessMessage();

            return RedirectToAction(nameof(Index));
        }
    }
}