namespace EventTracker.DataModel.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Save method.
        /// </summary>
        void Save();
    }
}