﻿#region directives

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Management.Instrumentation;
using System.Net.Sockets;
using System.Security.Policy;
using System.Transactions;
using AutoMapper;
using EventTracker.BusinessModel;
using EventTracker.BusinessModel.Common;
using EventTracker.BusinessModel.Criterias;
using EventTracker.BusinessModel.Membership;
using EventTracker.DataModel.Generated;
using EventTracker.DataModel.UnitOfWork;
using PagedList;
using HouseHold = EventTracker.BusinessModel.Membership.HouseHold;
using Member = EventTracker.BusinessModel.Membership.Member;

#endregion

namespace EventTracker.BusinessServices.Membership
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
                var dbMember = new DataModel.Generated.Member() {
                    FirstName = aMember.FirstName,
                    LastName = aMember.LastName,
                    DOB = aMember.DateOfBirth,
                    Gender = aMember.Gender.ToString(),
                    Phone = aMember.Phone,
                    Email = aMember.Email
                };

                //AT: We need to do manual mapping above. We can use code below but need to add the Ignore fields that we don't want updated
                //            Mapper.CreateMap<Member, DataModel.Generated.Member > ()
                //.ForMember(dest => dest.DOB,
                //    opts => opts.MapFrom(src => src.DateOfBirth));
                //            var dbMember = Mapper.Map<Member, DataModel.Generated.Member>(aMember);

                _unitOfWork.MemberRepository.Update(dbMember);
                _unitOfWork.Save();
                scope.Complete();
            }
        }

        public Common.PagedList GetMembers(SearchParameter param)
        {
            var pagedRecord = new Common.PagedList();
            using (var context = _unitOfWork.DbContext)
            {
                Func<Member, object> orderByFunc = null;
                if (param.SortBy == "Lastname")
                    orderByFunc = item => item.Name;
                else if (param.SortBy == "Firstname")
                    orderByFunc = item => item.FirstName;
                else 
                    orderByFunc = item => item.LastName;

                pagedRecord.Content = (from m in context.Members
                    join mmTemp in context.MemberMemberships on m.MemberId equals mmTemp.MemberId into tempJoin
                    from mm in tempJoin.DefaultIfEmpty()
                    join hhmTemp in context.HouseHoldMembers on m.MemberId equals hhmTemp.MemberId into hhmTempJoin
                    from hhm in hhmTempJoin.DefaultIfEmpty()
                    join hhTemp in context.HouseHolds on hhm.HouseHoldId equals hhTemp.HouseHoldId into hhTempJoin
                    from hh in hhTempJoin.DefaultIfEmpty()
                    where mm.EndDate == null 
                    select new Member
                    {
                        MemberId = m.MemberId,
                        LastName = m.LastName,
                        FirstName = m.FirstName,
                        MemberOf = mm.MemberOf,
                        Email = m.Email,
                        Phone = m.Phone,
                        DateOfBirth = m.DOB.Value,
                        HouseholdName = hh.Name
                    }).Where(x => param.SearchText == null ||
                        ((x.LastName.Contains(param.SearchText)) 
                    )).OrderBy(orderByFunc)
                    .Skip((param.PageNumber - 1) * param.PageSize)
                    .Take(param.PageSize)
                    .ToList();

                pagedRecord.TotalRecords = (from m in context.Members
                    join mmTemp in context.MemberMemberships on m.MemberId equals mmTemp.MemberId into tempJoin
                    from mm in tempJoin.DefaultIfEmpty()
                    join hhmTemp in context.HouseHoldMembers on m.MemberId equals hhmTemp.MemberId into hhmTempJoin
                    from hhm in hhmTempJoin.DefaultIfEmpty()
                    join hhTemp in context.HouseHolds on hhm.HouseHoldId equals hhTemp.HouseHoldId into hhTempJoin
                    from hh in hhTempJoin.DefaultIfEmpty()
                    where mm.EndDate == null
                    select m).Count();

                pagedRecord.CurrentPage = param.PageNumber;
                pagedRecord.PageSize = param.PageSize ;

                return pagedRecord;
            }
        }

        //public IPagedList<HouseHold> GetHouseHolds(HouseHoldCriteria crit, PagingInfo paging)
        //{
        //    using (var context = _unitOfWork.DbContext)
        //    {
        //        var houseHolds = (from hh in context.HouseHolds
        //                          join m in context.Members on hh.HouseHoldLeaderMemberId equals m.MemberId
        //                          where (!crit.HouseHoldId.HasValue || (hh.HouseHoldId == crit.HouseHoldId))
        //                                && (string.IsNullOrEmpty(crit.Area) || (!string.IsNullOrEmpty(crit.Area) && hh.Area == crit.Area))
        //                          orderby hh.Name
        //                          select new HouseHold {
        //                              HouseHoldId = hh.HouseHoldId,
        //                              Name = hh.Name,
        //                              Area = hh.Area,
        //                              State = hh.State,
        //                              HouseHoldLeader = m.FirstName + " "  + m.LastName
        //                          });//.ToPagedList(paging.CurrentPage, paging.ItemsPerPage);

        //        var pagedList = houseHolds.ToPagedList(paging.CurrentPage, paging.ItemsPerPage);
        //        return pagedList;
        //    }
        //}

        //public IEnumerable<Member> GetHouseHoldMemers(int houseHoldId)
        //{
        //    using (var context = _unitOfWork.DbContext) {
        //        var houseHolds = (from hhm in context.HouseHoldMembers
        //                          join m in context.Members on hhm.MemberId equals m.MemberId
        //                          where (hhm.HouseHoldId == houseHoldId && hhm.EndDate == null)
        //                          orderby m.LastName, m.IsHeadOfFamily descending , m.DOB
        //                          select new Member {
        //                              MemberId = m.MemberId,
        //                              LastName = m.LastName,
        //                              FirstName = m.FirstName,
        //                              DateOfBirth = m.DOB.Value
        //                          });//.ToPagedList(paging.CurrentPage, paging.ItemsPerPage);

        //        return houseHolds.ToList();
        //    }
        //}

        //public IPagedList<Member> GetHouseHoldMemers(int houseHoldId, int pageNumber, int pageSize)
        //{
        //    using (var context = _unitOfWork.DbContext) {
        //        var houseHolds = (from hhm in context.HouseHoldMembers
        //                          join m in context.Members on hhm.MemberId equals m.MemberId
        //                          where (hhm.HouseHoldId == houseHoldId && hhm.EndDate == null)
        //                          orderby m.LastName, m.IsHeadOfFamily descending, m.DOB
        //                          select new Member {
        //                              MemberId = m.MemberId,
        //                              LastName = m.LastName,
        //                              FirstName = m.FirstName,
        //                              DateOfBirth = m.DOB.Value
        //                          }).ToPagedList(pageNumber, pageSize);

        //        return houseHolds;
        //    }
        //}





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
        //((DisplayType)oRoleRightsPage.DisplayType).HasFlag(displayType)
        public Member GetMember(int memberId) {
            using (var context = _unitOfWork.DbContext) {
                var member = (from m in context.Members
                              join mmTemp in context.MemberMemberships on m.MemberId equals mmTemp.MemberId into tempJoin
                                 from mm in tempJoin.DefaultIfEmpty()
                                 join hhmTemp in context.HouseHoldMembers on m.MemberId equals hhmTemp.MemberId into hhmTempJoin
                                 from hhm in hhmTempJoin.DefaultIfEmpty()
                                 join hhTemp in context.HouseHolds on hhm.HouseHoldId equals hhTemp.HouseHoldId into hhTempJoin
                                 from hh in hhTempJoin.DefaultIfEmpty()
                              join mSpouseTemp in context.Members on m.SpouseMemberId equals mSpouseTemp.MemberId into tempSpouseJoin
                              from mSpouse in tempSpouseJoin.DefaultIfEmpty()
                              where mm.EndDate == null && m.MemberId == memberId
                              select new Member {
                                     MemberId = m.MemberId,
                                     LastName = m.LastName,
                                     FirstName = m.FirstName,
                                     Gender = m.Gender,
                                     MemberOf = mm.MemberOf,
                                     Email = m.Email,
                                     Phone = m.Phone,
                                     DateOfBirth = m.DOB.Value,
                                     HouseholdName = hh.Name,
                                     SpouseMemberId = m.SpouseMemberId,
                                     SpouseName = mSpouse.FirstName,
                                     HouseHoldId = hh.HouseHoldId
                                 }).FirstOrDefault();
                return member;
            }
            return null;
        }

        public IPagedList<Member> GetMembers(int pageIndex, int pageSize)
        {
            using (var context = _unitOfWork.DbContext)
            {
                var pagedList = (from m in context.Members
                    join mmTemp in context.MemberMemberships on m.MemberId equals mmTemp.MemberId into tempJoin
                    from mm in tempJoin.DefaultIfEmpty()
                    join hhmTemp in context.HouseHoldMembers on m.MemberId equals hhmTemp.MemberId into hhmTempJoin
                    from hhm in hhmTempJoin.DefaultIfEmpty()
                    join hhTemp in context.HouseHolds on hhm.HouseHoldId equals hhTemp.HouseHoldId into hhTempJoin
                    from hh in hhTempJoin.DefaultIfEmpty()
                               where mm.EndDate == null
                    orderby m.LastName, m.SpouseMemberId descending, m.FatherMemberId, m.MotherMemberId
                    select new Member
                    {
                        MemberId = m.MemberId,
                        LastName = m.LastName,
                        FirstName = m.FirstName,
                        MemberOf = mm.MemberOf,
                        Email = m.Email,
                        Phone = m.Phone,
                        DateOfBirth = m.DOB.Value,
                        HouseholdName = hh.Name
                    }).ToPagedList(pageIndex, pageSize);

                return pagedList;
            }
        }

        //AT: This is a different implementation of paging. Consider this for performance
        public IEnumerable<Member> GetMembers(int pageIndex, int pageSize, out int numberOfPages) {
            using (var context = _unitOfWork.DbContext) {
                var members = (from m in context.Members
                               join mmTemp in context.MemberMemberships on m.MemberId equals mmTemp.MemberId into tempJoin
                               from mm in tempJoin.DefaultIfEmpty()
                               join hhmTemp in context.HouseHoldMembers on m.MemberId equals hhmTemp.HouseHoldMemberId into hhmTempJoin
                               from hhm in hhmTempJoin.DefaultIfEmpty()
                               join hh in context.HouseHolds on hhm.MemberId equals hh.HouseHoldLeaderMemberId
                               orderby m.LastName, m.SpouseMemberId descending, m.FatherMemberId, m.MotherMemberId
                               select new Member {
                                   MemberId = m.MemberId,
                                   LastName = m.LastName,
                                   FirstName = m.FirstName,
                                   MemberOf = mm.MemberOf,
                                   Email = m.Email,
                                   Phone = m.Phone,
                                   DateOfBirth = m.DOB.Value,
                                   HouseholdName = hh.Name
                               }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var totalCount = (from m in context.Members
                                  join mmTemp in context.MemberMemberships on m.MemberId equals mmTemp.MemberId into tempJoin
                                  from mm in tempJoin.DefaultIfEmpty()
                                  select m.MemberId).Count();
                numberOfPages = (int)(totalCount / pageSize);
                return members;
            }
            return null;
        }

        
    }
}