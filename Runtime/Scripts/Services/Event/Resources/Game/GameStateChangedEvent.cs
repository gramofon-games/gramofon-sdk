using GRAMOFON.Enums;

namespace GRAMOFON.Services
{
    public class GameStateChangedEvent : BaseEvent
    {
        public EGameState GameState;
    }
}