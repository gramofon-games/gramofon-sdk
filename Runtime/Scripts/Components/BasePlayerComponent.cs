using GRAMOFON.Misc;
using GRAMOFON.Models;
using GRAMOFON.Services;

namespace GRAMOFON.Components
{
    public class BasePlayerComponent : BaseComponent
    {
        #region Private Fields

        private Player player;

        #endregion
        
        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Initialize(params object[] parameters)
        {
            player = DataService.LoadObjectWithKey<Player>(GRAMOFONCommonTypes.PLAYER_DATA_KEY);
                
            EventService.DispatchEvent(new CurrencyUpdatedEvent()
            {
                Currency = GetPlayer().Currency,
                UIDelay = 0,
                UIDuration = 0
            });
                
            return base.Initialize(parameters);
        }
        
        /// <summary>
        /// This function helper for increase currency.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="uiDelay"></param>
        /// <param name="uiDuration"></param>
        public void IncreaseCurrency(int amount, float uiDelay = 0, float uiDuration = -1)
        {
            GetPlayer().Currency += amount;
            GetPlayer().SaveData();
                
            EventService.DispatchEvent(new CurrencyUpdatedEvent()
            {
                Currency = GetPlayer().Currency,
                UIDelay = uiDelay,
                UIDuration = GramofonSDK.BaseGameSettings.FlyCurrencyDuration
            });
        }
            
        /// <summary>
        /// This function helper for decrease currency.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="uiDelay"></param>
        /// <param name="uiDuration"></param>
        public void DecreaseCurrency(int amount, float uiDelay = 0, float uiDuration = -1)
        {
            GetPlayer().Currency -= amount;
            GetPlayer().SaveData();
                
            EventService.DispatchEvent(new CurrencyUpdatedEvent()
            {
                Currency = GetPlayer().Currency,
                UIDelay = uiDelay,
                UIDuration = GramofonSDK.BaseGameSettings.FlyCurrencyDuration
            });
        }
        
        /// <summary>
        /// This function helper for check currency.
        /// </summary>
        /// <param name="targetAmount"></param>
        /// <returns></returns>
        public bool CheckCurrency(float targetAmount)
        {
            return GetPlayer().Currency >= targetAmount;
        }

        /// <summary>
        /// This function returns related player component.
        /// </summary>
        /// <returns></returns>
        public Player GetPlayer()
        {
            return player;
        }
    }
}
