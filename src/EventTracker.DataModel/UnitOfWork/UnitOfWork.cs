#region directives

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using EventTracker.DataModel.Generated;
using EventTracker.DataModel.GenericRepository;

#endregion

namespace EventTracker.DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        public UnitOfWork()
        {
            _dbContext = new EventTrackerDBContext();
            _dbContext.Configuration.LazyLoadingEnabled = false;
        }

        public EventTrackerDBContext DbContext
        {
            get { return _dbContext; }
        }

        #region Public member methods...

        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Private member variables...

        private readonly EventTrackerDBContext _dbContext = null;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Event> _productRepository;
        private GenericRepository<Token> _tokenRepository;
        private GenericRepository<EventAttendance> _eventAttendanceRepository;

        #endregion

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for product repository.
        /// </summary>
        public GenericRepository<Event> EventRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new GenericRepository<Event>(_dbContext);
                return _productRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new GenericRepository<User>(_dbContext);
                return _userRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Token> TokenRepository
        {
            get
            {
                if (_tokenRepository == null)
                    _tokenRepository = new GenericRepository<Token>(_dbContext);
                return _tokenRepository;
            }
        }

        public GenericRepository<EventAttendance> EventAttendanceRepository
        {
            get
            {
                if (_eventAttendanceRepository == null)
                    _eventAttendanceRepository = new GenericRepository<EventAttendance>(_dbContext);
                return _eventAttendanceRepository;
            }
        }

        //public GenericRepository<EventAttendanceLog> EventAttendanceLogRepository
        //{
        //    get
        //    {
        //        if (this._eventAttendanceLogRepository == null)
        //            this._eventAttendanceLogRepository = new GenericRepository<EventAttendanceLog>(_dbContext);
        //        return _eventAttendanceLogRepository;
        //    }
        //}

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...

        private bool disposed = false;

        #endregion

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

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
