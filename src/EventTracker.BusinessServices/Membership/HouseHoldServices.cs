using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EventTracker.BusinessModel.Membership;
using EventTracker.DataModel.UnitOfWork;

namespace EventTracker.BusinessServices.Membership
{
    public class HouseHoldServices:IHouseHoldServices
    {
        private readonly UnitOfWork _unitOfWork;

        public HouseHoldServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }

    public interface IHouseHoldServices
    {
        IEnumerable<HouseHold> GetHouseHolds();
    }
}