#region directives

using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using EventTracker.BusinessModel;
using EventTracker.DataModel.UnitOfWork;
using DBObject = EventTracker.DataModel.Generated;

#endregion

namespace EventTracker.BusinessServices
{
    public interface IEventAttendanceLogServices
    {
        int CreateEventAttendance(int eventId, int userId, int logBy);
    }

    public class EventAttendanceServices : IEventAttendanceLogServices
    {
        private readonly UnitOfWork _unitOfWork;

        public EventAttendanceServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CreateEventAttendance(int eventId, int userId, int logBy)
        {
            using (var scope = new TransactionScope())
            {
                var dbEventAttendance = new DataModel.Generated.EventAttendance()
                {
                    UserId = logBy,
                    EventId = eventId
                };

                _unitOfWork.EventAttendanceRepository.Insert(dbEventAttendance);
                _unitOfWork.Save();
                scope.Complete();
                return dbEventAttendance.EventId;
            }
        }

        public IEnumerable<EventAttendance> GetByEventId(int eventId)
        {
            //var list = _unitOfWork.EventAttendanceLogRepository.GetMany(e => e.EventId == eventId).ToList();
            var list = _unitOfWork.EventAttendanceRepository.GetMany(e => e.EventId == eventId).ToList();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    _unitOfWork.EventAttendanceRepository.Context.Entry(item).Reference<DBObject.User>(x => x.User).Load();
                }
                Mapper.CreateMap<DBObject.EventAttendance, EventAttendance>();
                Mapper.CreateMap<DBObject.User, AppUser>();
                var events = Mapper.Map<List<DBObject.EventAttendance>, List<EventAttendance>>(list);
                return events;
            }
            return null;
        }
    }
}