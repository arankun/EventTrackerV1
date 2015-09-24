﻿using System.Security.Principal;

namespace EventTrackerAPI.Filters
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public int UserId { get; set; }
        public BasicAuthenticationIdentity(string userName, string password)
            : base(userName, "Basic")
        {
            UserName = userName;
            Password = password;
        }
    }
}