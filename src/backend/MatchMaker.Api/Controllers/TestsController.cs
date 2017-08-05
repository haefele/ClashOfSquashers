using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers
{
    [Route("Tests")]
    public class TestsController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new {WhatDidYouJustFuckingSayAboutMe = "You little shit?"});
        }
    }
}