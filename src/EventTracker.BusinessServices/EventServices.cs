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
    public class EventServices:IEventServices
    {
        private readonly UnitOfWork _unitOfWork;

        public EventServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Event GetEventById(int eventId)
        {
            var dbEvent = _unitOfWork.EventRepository.GetByID(eventId);
            if (dbEvent != null)
            {
                Mapper.CreateMap<DataModel.Generated.Event, Event>();
                var anEvent = Mapper.Map<DataModel.Generated.Event, Event>(dbEvent);
                return anEvent;
            }
            return null;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var list = _unitOfWork.EventRepository.GetAll().ToList();
            if (list.Any())
            {
                Mapper.CreateMap<DataModel.Generated.Event, Event>();
                var events = Mapper.Map<List<DataModel.Generated.Event>, List<Event>>(list);
                return events;
            }
            return null;
        }

        public int CreateEvent(Event anEvent)
        {
            using (var scope = new TransactionScope())
            {
                var dbEvent = new DataModel.Generated.Event
                {
                    EventName = anEvent.EventName,
                    EventDate = anEvent.EventDate
                };
                _unitOfWork.EventRepository.Insert(dbEvent);
                _unitOfWork.Save();
                scope.Complete();
                return dbEvent.EventId;
            }
        }

        public bool UpdateEvent(int eventId, Event anEvent)
        {
            var success = false;
            if (anEvent != null)
            {
                using (var scope = new TransactionScope())
                {
                    var dbEvent = _unitOfWork.EventRepository.GetByID(eventId);
                    if (dbEvent != null)
                    {
                        dbEvent.EventName = anEvent.EventName;
                        dbEvent.EventDate = anEvent.EventDate;
                        _unitOfWork.EventRepository.Update(dbEvent);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool DeleteEvent(int eventId)
        {
            var success = false;
            if (eventId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var dbEvent = _unitOfWork.EventRepository.GetByID(eventId);
                    if (dbEvent != null)
                    {
                        _unitOfWork.EventRepository.Delete(dbEvent);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public List<Event> GetEventWithAttendance(int eventId)
        {
            using (var context = _unitOfWork.DbContext)
            {
                var eventAttendances = (from e in context.Events
                                        join a in context.EventAttendances on e.EventId equals a.EventId
                                        join u in context.Users on a.UserId equals u.UserId
                                        where e.EventId == eventId
                                        select new Event
                                        {
                                            EventId = e.EventId,
                                            EventName = e.EventName,
                                            EventDate = e.EventDate,
                                            EventAttendances  = new List<EventAttendance>()
                                            {
                                                new EventAttendance()
                                                {
                                                    EventAttendanceId = a.EventAttendanceId,
                                                    Attendee = new AppUser()
                                                    {
                                                        UserId = u.UserId,
                                                        Name =  u.Name
                                                    }
                                                }

                                            }
                                        }).ToList();


                return eventAttendances;
            };
            return null;
        }
    }
}