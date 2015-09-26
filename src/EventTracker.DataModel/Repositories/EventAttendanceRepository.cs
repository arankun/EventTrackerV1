#region directives

using System;
using System.Diagnostics;
using EventTracker.DataModel.Generated;

#endregion

namespace EventTracker.DataModel.Repositories
{
    //AT: Need to prototype this. Basically we want to create a custom repository 
    public class EventAttendanceRepository:IDisposable
    {
        private readonly EventTrackerDBContext _dbContext;
        private bool disposed = false;

        public EventAttendanceRepository()
        {
            _dbContext = new EventTrackerDBContext(); ;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
    }
}
