namespace GRAMOFON.Services
{
    public class CurrencyUpdatedEvent : BaseEvent
    {
        public int Currency;
        public float UIDelay;
        public float UIDuration;
    }   
}