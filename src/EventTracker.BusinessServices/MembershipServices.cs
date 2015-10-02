#region directives

using System;
using System.Collections.Generic;
using System.Transactions;
using AutoMapper;
using EventTracker.BusinessModel.Membership;
using EventTracker.DataModel.UnitOfWork;
using System.Linq;
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

        public void UpdateMember(Member aMember) {
            using (var scope = new TransactionScope()) {
                //var dbMember = new DataModel.Generated.Member() {
                //    FirstName = aMember.FirstName,
                //    LastName = aMember.LastName,
                //    DOB = aMember.DateOfBirth,
                //    Gender = aMember.Gender.ToString(),
                //    Phone = aMember.Phone,
                //    Email = aMember.Email
                //};

                Mapper.CreateMap<Member, DataModel.Generated.Member > ()
    .ForMember(dest => dest.DOB,
        opts => opts.MapFrom(src => src.DateOfBirth));
                var dbMember = Mapper.Map<Member, DataModel.Generated.Member>(aMember);

                _unitOfWork.MemberRepository.Update(dbMember);
                _unitOfWork.Save();
                scope.Complete();
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
                //AT: Custom mapping a field
                Mapper.CreateMap<DataModel.Generated.Member, Member>()
                    .ForMember(dest => dest.DateOfBirth,
                        opts => opts.MapFrom(src => src.DOB));
                var aMember = Mapper.Map<DataModel.Generated.Member, Member>(dbMember);
                return aMember;
            }
            return null;
        }

        public IEnumerable<Member> GetMembers(int pageIndex = 0, int pageSize = 10)
        {
            using (var context = _unitOfWork.DbContext)
            {
                var members = (from m in context.Members
                    join mmTemp in context.MemberMemberships on m.MemberId equals mmTemp.MemberId into tempJoin
                    from mm in tempJoin.DefaultIfEmpty()
                    orderby m.LastName, m.SpouseMemberId descending, m.ParentMemberId
                    select new Member
                    {
                        MemberId = m.MemberId,
                        LastName = m.LastName,
                        FirstName = m.FirstName,
                        MemberOf = mm.MemberOf,
                        Email = m.Email,
                        Phone = m.Phone,
                        DateOfBirth = m.DOB.Value
                    }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return members;
            }
            return null;
        }


    }
}