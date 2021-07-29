using IdentityServer.Services.Ìnterfaces;
using System;

namespace IdentityServer.Services.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime CurrentDateTime => DateTime.Now;
    }
}
