using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers.Tests
{
    [Route("Tests")]
    public class TestsController : MatchMakerController
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new {WhatDidYouJustFuckingSayAboutMe = "You little shit?", UserId = this.User.AccountId});
        }
    }
}