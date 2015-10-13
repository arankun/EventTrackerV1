using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using EventTracker.BusinessModel.Common;
using EventTracker.BusinessModel.Criterias;
using EventTracker.BusinessModel.Membership;
using EventTracker.DataModel.Generated;
using EventTracker.DataModel.UnitOfWork;
using PagedList;
using HouseHold = EventTracker.BusinessModel.Membership.HouseHold;
using Member = EventTracker.BusinessModel.Membership.Member;

namespace EventTracker.BusinessServices.Membership
{
    public class HouseHoldServices:IHouseHoldServices
    {
        private readonly UnitOfWork _unitOfWork;

        public HouseHoldServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public HouseHold GetHouseHold(int? houseHoldId)
        {
            var hhDbModel = _unitOfWork.HouseHoldRepository.GetByID(houseHoldId);
            Mapper.CreateMap<DataModel.Generated.HouseHold, HouseHold>();
            var aHouseHold = Mapper.Map<DataModel.Generated.HouseHold, HouseHold >(hhDbModel);
            return aHouseHold;
        }

        public IEnumerable<HouseHold> GetHouseHolds()
        {
            var list = _unitOfWork.HouseHoldRepository.GetAll().ToList();
            if (list.Any()) {
                Mapper.CreateMap<DataModel.Generated.HouseHold, HouseHold>();
                var modelList = Mapper.Map<List<DataModel.Generated.HouseHold>, List<HouseHold>>(list);
                return modelList;
            }
            return null;
        }

        public IPagedList<HouseHold> GetHouseHolds(HouseHoldCriteria crit, PagingInfo paging) {
            using (var context = _unitOfWork.DbContext) {
                var houseHolds = (from hh in context.HouseHolds
                                  join m in context.Members on hh.HouseHoldLeaderMemberId equals m.MemberId
                                  where (!crit.HouseHoldId.HasValue || (hh.HouseHoldId == crit.HouseHoldId))
                                        && (string.IsNullOrEmpty(crit.Area) || (!string.IsNullOrEmpty(crit.Area) && hh.Area == crit.Area))
                                  orderby hh.Name
                                  select new HouseHold {
                                      HouseHoldId = hh.HouseHoldId,
                                      Name = hh.Name,
                                      Area = hh.Area,
                                      State = hh.State,
                                      HouseHoldLeader = m.FirstName + " " + m.LastName
                                  });//.ToPagedList(paging.CurrentPage, paging.ItemsPerPage);

                var pagedList = houseHolds.ToPagedList(paging.CurrentPage, paging.ItemsPerPage);
                return pagedList;
            }
        }

        public IPagedList<Member> GetHouseHoldMemers(int houseHoldId, int pageNumber, int pageSize) {
            using (var context = _unitOfWork.DbContext) {
                var houseHolds = (from hhm in context.HouseHoldMembers
                                  join m in context.Members on hhm.MemberId equals m.MemberId
                                  where (hhm.HouseHoldId == houseHoldId && hhm.EndDate == null)
                                  orderby m.LastName, m.IsHeadOfFamily descending, m.DOB
                                  select new Member {
                                      MemberId = m.MemberId,
                                      LastName = m.LastName,
                                      FirstName = m.FirstName,
                                      DateOfBirth = m.DOB.Value
                                  }).ToPagedList(pageNumber, pageSize);

                return houseHolds;
            }
        }

        public HouseHoldDetailsViewModel GetHouseHoldViewModel(int houseHoldId) {
            //var dbEntity = _unitOfWork.HouseHoldRepository.GetByID(houseHoldId);
            //if (dbEntity != null && dbEntity.HouseHoldId > 0) {
            //    //AT: Custom mapping a field
            //    Mapper.CreateMap<DataModel.Generated.HouseHold, HouseHold>();
            //    var model = Mapper.Map<DataModel.Generated.HouseHold, HouseHold>(dbEntity);

            //    return model;
            //}
            //return null;
            var crit = new HouseHoldCriteria() { HouseHoldId = houseHoldId };
            using (var context = _unitOfWork.DbContext) {
                var houseHolds = (from hh in context.HouseHolds
                                  join m in context.Members on hh.HouseHoldLeaderMemberId equals m.MemberId
                                  where hh.HouseHoldId == crit.HouseHoldId
                                  orderby hh.Name
                                  select new HouseHold {
                                      HouseHoldId = hh.HouseHoldId,
                                      HouseHoldLeaderMemberId = hh.HouseHoldLeaderMemberId,
                                      Name = hh.Name,
                                      Area = hh.Area,
                                      State = hh.State,
                                      HouseHoldLeader = m.FirstName + " " + m.LastName
                                  }).FirstOrDefault();

                var hhMembers = (from hhm in context.HouseHoldMembers
                                 join m in context.Members on hhm.MemberId equals m.MemberId
                                 where (hhm.HouseHoldId == houseHoldId && hhm.EndDate == null)
                                 orderby m.LastName, m.IsHeadOfFamily descending, m.DOB
                                 select new Member {
                                     MemberId = m.MemberId,
                                     LastName = m.LastName,
                                     FirstName = m.FirstName,
                                     DateOfBirth = m.DOB.Value
                                 });//.ToPagedList(paging.CurrentPage, paging.ItemsPerPage);
                var vm = new HouseHoldDetailsViewModel() { HouseHold = houseHolds, HouseHoldMembers = hhMembers.ToList() };

                return vm;
            }

        }

        public int AddMemberToHousehold(NewHouseholdMember newhhMember) {
            var familyMembers = GetFamilyMembersByHeadOfFamilyMemberId(newhhMember.MemberId);
            int membersAdded = 0;
            using (var scope = new TransactionScope()) {
                //var dbHhMember = BuildHouseHoldMember(newhhMember.HouseHoldId, newhhMember.MemberId);
                //_unitOfWork.HouseHoldMemberRepository.Insert(dbHhMember);
                //_unitOfWork.Save();
                //membersAdded++;
                foreach (var member in familyMembers) {
                    HouseHoldMember dbHhMember;
                    switch (member.MemberOf) {
                        case "KFC":
                        case "YFC":
                            dbHhMember = BuildHouseHoldMember(newhhMember.HouseHoldId, member.MemberId);
                            _unitOfWork.HouseHoldMemberRepository.Insert(dbHhMember);
                            _unitOfWork.Save();
                            membersAdded++;
                            break;
                        default:
                            if (member.MemberId == newhhMember.MemberId) {
                                dbHhMember = BuildHouseHoldMember(newhhMember.HouseHoldId, member.MemberId);
                                _unitOfWork.HouseHoldMemberRepository.Insert(dbHhMember);
                                _unitOfWork.Save();
                            }
                            else if (member.SpouseMemberId == newhhMember.MemberId) {
                                dbHhMember = BuildHouseHoldMember(newhhMember.HouseHoldId, member.MemberId);
                                _unitOfWork.HouseHoldMemberRepository.Insert(dbHhMember);
                                _unitOfWork.Save();
                                membersAdded++;
                            }
                            break;
                    }
                }
                scope.Complete();
                return membersAdded;
            }
        }

        private HouseHoldMember BuildHouseHoldMember(int houseHoldId, int memberId) {
            var dbHhMember = new DataModel.Generated.HouseHoldMember {
                HouseHoldId = houseHoldId,
                MemberId = memberId,
                StartDate = DateTime.Now.Date
            };
            return dbHhMember;
        }
        public IEnumerable<Member> GetFamilyMembersByHeadOfFamilyMemberId(int headOfFamilyMemberId) {
            //using (var context = _unitOfWork.DbContext)
            //{
            //    var mm = (from m in context.Members
            //        where m.MemberId == headOfFamilyMemberId
            //        select new Member() {MemberId = m.MemberId, LastName = m.LastName, FirstName = m.FirstName, SpouseMemberId = m.SpouseMemberId})
            //        .Union(from m in context.Members
            //            where m.SpouseMemberId == headOfFamilyMemberId
            //            select new Member() {MemberId = m.MemberId, LastName = m.LastName, FirstName = m.FirstName, SpouseMemberId = m.SpouseMemberId })
            //        .Union(from mC in context.Members
            //            join mmTemp in context.MemberMemberships on mC.MemberId equals mmTemp.MemberId into tempJoin
            //            from mmTempJoin in tempJoin.DefaultIfEmpty()
            //            join mMo in context.Members on mC.MotherMemberId equals mMo.MemberId into tempJoinMo
            //            from mJoinMo in tempJoin.DefaultIfEmpty()
            //            join mFa in context.Members on mC.FatherMemberId equals mFa.MemberId into tempJoinFa
            //            from mJoinFa in tempJoinFa.DefaultIfEmpty()
            //            where (mC.FatherMemberId == headOfFamilyMemberId || mC.MotherMemberId == headOfFamilyMemberId)
            //                  && mmTempJoin.EndDate == null
            //            select new Member() {MemberId = mC.MemberId, LastName = mC.LastName, FirstName = mC.FirstName, SpouseMemberId = mC.SpouseMemberId });
            //    var list = mm.ToList();
            //    return list;
            //}
            var context = _unitOfWork.DbContext;
            var mm = (from m in context.Members
                      where m.MemberId == headOfFamilyMemberId
                      select new Member() { MemberId = m.MemberId, LastName = m.LastName, FirstName = m.FirstName, SpouseMemberId = m.SpouseMemberId })
                .Union(from m in context.Members
                       where m.SpouseMemberId == headOfFamilyMemberId
                       select new Member() { MemberId = m.MemberId, LastName = m.LastName, FirstName = m.FirstName, SpouseMemberId = m.SpouseMemberId })
                .Union(from mC in context.Members
                       join mmTemp in context.MemberMemberships on mC.MemberId equals mmTemp.MemberId into tempJoin
                       from mmTempJoin in tempJoin.DefaultIfEmpty()
                       join mMo in context.Members on mC.MotherMemberId equals mMo.MemberId into tempJoinMo
                       from mJoinMo in tempJoin.DefaultIfEmpty()
                       join mFa in context.Members on mC.FatherMemberId equals mFa.MemberId into tempJoinFa
                       from mJoinFa in tempJoinFa.DefaultIfEmpty()
                       where (mC.FatherMemberId == headOfFamilyMemberId || mC.MotherMemberId == headOfFamilyMemberId)
                                 && mmTempJoin.EndDate == null
                       select new Member() { MemberId = mC.MemberId, LastName = mC.LastName, FirstName = mC.FirstName, SpouseMemberId = mC.SpouseMemberId });
            var list = mm.ToList();
            return list;
        }

        public IEnumerable<Member> GetHeadOfFamilyMembers(int houseHoldId, int houseHoldLeaderMemberId) {
            //AT: Get HeadOfFamilyMembers that are not in current householdid
            using (var context = _unitOfWork.DbContext) {
                var mm = (from m in context.Members
                          join hhm in context.HouseHoldMembers
on m.MemberId equals hhm.MemberId into tempJoin
                          from hhmTemp in tempJoin.DefaultIfEmpty()
                          where m.IsHeadOfFamily == "Y"
                          && hhmTemp.HouseHoldId != houseHoldId
                          && m.MemberId != houseHoldLeaderMemberId
                          select new Member() { MemberId = m.MemberId, LastName = m.LastName, FirstName = m.FirstName });
                var list = mm.ToList();
                return list;
            }
        }

    }

    public interface IHouseHoldServices
    {
        HouseHold GetHouseHold(int? houseHoldId);
        IEnumerable<HouseHold> GetHouseHolds();
        IPagedList<HouseHold> GetHouseHolds(HouseHoldCriteria houseHoldCriteria, PagingInfo pagingInfo);

        IPagedList<Member> GetHouseHoldMemers(int houseHoldId, int pageNumber, int pageSize);

        HouseHoldDetailsViewModel GetHouseHoldViewModel(int houseHoldId);
        IEnumerable<Member> GetHeadOfFamilyMembers(int houseHoldId, int houseHoldLeaderMemberId);

        int AddMemberToHousehold(NewHouseholdMember newhhMember);
    }
}