﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace yt_Dot6Identity.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles ="admin")]
        public IActionResult Display()
        {
            return View();
        }
    }
}
