using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpendingDiary.Controllers
{
    public class HomeController : Controller
    {
        //[Route("/secret")]
        //[Authorize]
        public string Index()
        {
            return "Welcome to our WebSite";
        }
    }
}