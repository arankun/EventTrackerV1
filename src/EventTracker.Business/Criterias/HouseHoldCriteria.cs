namespace EventTracker.BusinessModel.Criterias
{
    public class GenericCriteria<TCrit> where TCrit : class
    {
    }



    public class HouseHoldCriteria
    {
         public string Area { get; set; }
         public string State{ get; set; }
        public int? HouseHoldId { get; set; }
    }
}