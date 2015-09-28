#region directives

using System;
using System.Transactions;
using AutoMapper;
using EventTracker.BusinessModel.Membership;
using EventTracker.DataModel.UnitOfWork;

#endregion

namespace EventTracker.BusinessServices
{
    public class MembershipServices : IMembershipServices
    {
        private readonly UnitOfWork _unitOfWork;

        public MembershipServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int AddMember(NewMember newMember)
        {
            using (var scope = new TransactionScope())
            {
                var dbMember = new DataModel.Generated.Member()
                {
                    FirstName = newMember.FirstName,
                    LastName = newMember.LastName,
                    DOB = newMember.DateOfBirth,
                    Gender = newMember.Gender.ToString(),
                    Phone = newMember.Phone,
                    Email = newMember.Email
                };
                _unitOfWork.MemberRepository.Insert(dbMember);
                _unitOfWork.Save();
                scope.Complete();
                return dbMember.MemberId;
            }
        }

        public int AddSpouseOfMember(int spouseMemberId, NewMember newMember)
        {
            using (var scope = new TransactionScope())
            {
                var dbMember = new DataModel.Generated.Member()
                {
                    FirstName = newMember.FirstName,
                    LastName = newMember.LastName,
                    DOB = newMember.DateOfBirth,
                    Gender = newMember.Gender.ToString(),
                    Phone = newMember.Phone,
                    Email = newMember.Email,
                    SpouseMemberId = spouseMemberId
                };
                _unitOfWork.MemberRepository.Insert(dbMember);
                _unitOfWork.Save();
                var newMemberId = dbMember.MemberId;

                var spouse = _unitOfWork.MemberRepository.Get(u => u.MemberId == spouseMemberId);
                spouse.SpouseMemberId = newMemberId;
                _unitOfWork.MemberRepository.Update(spouse);
                _unitOfWork.Save();

                scope.Complete();
                return dbMember.MemberId;
            }
        }

        public int AddKfcMember(int parentId, NewMember newMember)
        {
            using (var scope = new TransactionScope())
            {
                var dbMember = new DataModel.Generated.Member()
                {
                    FirstName = newMember.FirstName,
                    LastName = newMember.LastName,
                    DOB = newMember.DateOfBirth,
                    Gender = newMember.Gender.ToString(),
                    Phone = newMember.Phone,
                    Email = newMember.Email
                };
                _unitOfWork.MemberRepository.Insert(dbMember);
                _unitOfWork.Save();

                var dbMemberMembership = new DataModel.Generated.MemberMembership()
                {
                    MemberId = dbMember.MemberId,
                    MemberOf = "KFC",
                    StartDate = DateTime.Now.Date
                };
                _unitOfWork.MemberMembershipRepository.Insert(dbMemberMembership);
                _unitOfWork.Save();
                scope.Complete();
                return dbMember.MemberId;
            }
        }


        public Member GetMember(int memberId)
        {
            var dbMember = _unitOfWork.MemberRepository.Get(u => u.MemberId == memberId);
            if (dbMember != null && dbMember.MemberId > 0)
            {
                Mapper.CreateMap<DataModel.Generated.Member, Member>();
                var aMember = Mapper.Map<DataModel.Generated.Member, Member>(dbMember);
                return aMember;
            }
            return null;
        }
    }
}