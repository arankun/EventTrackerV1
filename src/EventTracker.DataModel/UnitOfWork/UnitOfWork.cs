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
            DbContext = new EventTrackerDBContext();
            DbContext.Configuration.LazyLoadingEnabled = false;
        }

        public EventTrackerDBContext DbContext { get; } = null;

        #region Public member methods...

        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                DbContext.SaveChanges();
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

        private GenericRepository<User> _userRepository;
        private GenericRepository<Event> _productRepository;
        private GenericRepository<Token> _tokenRepository;
        private GenericRepository<EventAttendance> _eventAttendanceRepository;
        private GenericRepository<HouseHold> _householdRepository;
        private GenericRepository<HouseHoldMember> _householdMemberRepository;

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
                    _productRepository = new GenericRepository<Event>(DbContext);
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
                    _userRepository = new GenericRepository<User>(DbContext);
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
                    _tokenRepository = new GenericRepository<Token>(DbContext);
                return _tokenRepository;
            }
        }

        public GenericRepository<EventAttendance> EventAttendanceRepository
        {
            get
            {
                if (_eventAttendanceRepository == null)
                    _eventAttendanceRepository = new GenericRepository<EventAttendance>(DbContext);
                return _eventAttendanceRepository;
            }
        }

        public GenericRepository<Member> MemberRepository
        {
            get
            {
                if (_memberRepository == null)
                    _memberRepository = new GenericRepository<Member>(DbContext);
                return _memberRepository;
            }
        }

        public GenericRepository<MemberMembership> MemberMembershipRepository
        {
            get
            {
                if (_memberMembershipRepository == null)
                    _memberMembershipRepository = new GenericRepository<MemberMembership>(DbContext);
                return _memberMembershipRepository;
            }
        }

        public GenericRepository<HouseHold> HouseHoldRepository
        {
            get {
                if (_householdRepository == null)
                    _householdRepository = new GenericRepository<HouseHold>(DbContext);
                return _householdRepository;
            }

        }

        public GenericRepository<HouseHoldMember> HouseHoldMemberRepository {
            get {
                if (_householdMemberRepository == null)
                    _householdMemberRepository = new GenericRepository<HouseHoldMember>(DbContext);
                return _householdMemberRepository;
            }

        }
        
        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...

        private bool disposed = false;
        private GenericRepository<Member> _memberRepository;
        private GenericRepository<MemberMembership> _memberMembershipRepository;

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
                    DbContext.Dispose();
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
