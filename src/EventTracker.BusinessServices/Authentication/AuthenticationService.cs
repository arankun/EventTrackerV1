﻿#region directives

using System.Collections.Generic;
using EventTracker.BusinessModel.Authentication;
using EventTracker.DataModel.UnitOfWork;

#endregion

namespace EventTracker.BusinessServices.Authentication
{
    public class AuthenticationService:IAuthenticationService
    {
        private readonly UnitOfWork _unitOfWork;

        public AuthenticationService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CustomPrincipalSerializeModel AuthenticateUser(string username, string password)
        {
            var aUser = _unitOfWork.UserRepository.GetFirst(x => x.UserName == username && x.Password == password);
            if (aUser != null)
            {
                var member = _unitOfWork.MemberRepository.GetByID(aUser.MemberId);
                //TODO: @integrate role based security
                var appUser = new CustomPrincipalSerializeModel()
                {
                    LastName = member.LastName,
                    FirstName = member.FirstName,
                    Email = member.Email,
                    UserId = aUser.UserId,
                    Username = aUser.UserName,
                    MemberId = member.MemberId
                };

                //TODO: manually map role for now
                if (appUser.LastName.ToUpper().StartsWith("TEJ") && appUser.FirstName.ToUpper().StartsWith("ALL"))
                {
                    appUser.Roles = new List<string>() { "Admin" };

                }
                else if (appUser.LastName.ToUpper().StartsWith("PO") && appUser.FirstName.ToUpper().StartsWith("MAN"))
                {
                    appUser.Roles = new List<string>() { "HHLeader" };
                }
                else
                {
                    appUser.Roles = new List<string>() { "Member" };
                }
                return appUser;
            }
                
            return null;
        }
    }
}