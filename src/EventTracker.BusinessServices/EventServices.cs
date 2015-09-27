#region directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public List<EventAttendanceSummary> GetEventWithAttendanceUsingProjection(int eventId)
        {
            using (var context = _unitOfWork.DbContext)
            {
                var eventAttendances = new List<EventAttendanceSummary>(context.Events.Project().To<EventAttendanceSummary>()) ;

                Console.WriteLine("SQL: \n\n{0}\n\n", eventAttendances);
                return eventAttendances;
            };
            return null;
        }

        public List<Event> GetEventWithAttendance(int eventId)
        {
            using (var context = _unitOfWork.DbContext)
            {
                var eventAttendances = (from e in context.Events
                                        join a in context.EventAttendances on e.EventId equals a.EventId
                                        join u in context.Users on a.UserId equals u.UserId
                                        join l in context.Users on a.LogBy equals l.UserId into inJoin
                                        from outJoin in inJoin.DefaultIfEmpty()
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
                                                    },
                                                    LogBy = (int?)outJoin.UserId,
                                                    LogByUser = (outJoin.Equals(null)?null:new AppUser()
                                                    {
                                                        UserId = outJoin.UserId,
                                                        Name = outJoin.Name
                                                    })
                                                }

                                            }
                                        }).ToList();


                return eventAttendances;
            };
            return null;
        }

        public List<EventAttendanceSummary> GetEventAttendanceSummary(int eventId)
        {
//            from r in ctx.Rs
//join p in ctx.Ps.DefaultIfEmpty() on r.RK equals p.RK
//group r by r.QK into gr
//select new { QK = (int)gr.Key, Num = gr.Count(x => x.RK != null) }

  //select e.eventid, e.eventname, u.Name, count(*) as user_count
  //from events e join EventAttendances ev
  //on e.eventid=ev.eventid
  //join users u on ev.userid=u.userid
  //where e.eventid=1
  //group by e.eventid, e.EventName, u.Name

            using (var context = _unitOfWork.DbContext)
            {
                var eventAttendances = (from a in context.EventAttendances
                                        join e in context.Events on a.EventId equals e.EventId
                                        join u in context.Users on a.UserId equals u.UserId
                                        where e.EventId == eventId
                                        group a by new { e.EventId, e.EventName, e.EventDate, u.Name } into g
                                        select new EventAttendanceSummary
                                        {
                                            EventId = g.Key.EventId,
                                            EventName = g.Key.EventName,
                                            EventDate = g.Key.EventDate,
                                            AttendeeName = g.Key.Name,
                                            AttendeeLogCount = g.Count()
                                        }).ToList();


                return eventAttendances;
            };
            return null;
        }
    }
}