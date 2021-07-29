using IdentityServer.Services.Ìnterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DateTimesController : ControllerBase
    {
        private readonly IDateTimeService _dateTimeService;

        public DateTimesController(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        [HttpGet]
        public string Get()
        {
            DateTime currentDateTime = _dateTimeService.CurrentDateTime;
            
            return currentDateTime.ToString("f");
        }
    }
}
