﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Authentication.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
    {
        UserManager<AuthenticationUser> _userManager;
        private readonly SignInManager<AuthenticationUser> _signInManager;
        ILogger<ResetAuthenticatorModel> _logger;

        public ResetAuthenticatorModel(
            UserManager<AuthenticationUser> userManager,
            SignInManager<AuthenticationUser> signInManager,
            ILogger<ResetAuthenticatorModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Kimlik doğrulama uygulama anahtarınız sıfırlandı, yeni anahtarı kullanarak kimlik doğrulama uygulamanızı yapılandırmanız gerekecek.";

            return RedirectToPage("./EnableAuthenticator");
        }
    }
}