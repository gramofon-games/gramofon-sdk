using UnityEngine;
using MoreMountains.NiceVibrations;

namespace GRAMOFON
{
    public class BaseVibrationManager : BaseManager
    {
        #region Private Fields

        private bool state;

        #endregion
        
        /// <summary>
        /// This function called when before first frame.
        /// </summary>
        protected override void Awake()
        {
            state = PlayerPrefs.GetInt(GRAMOFONCommonTypes.VIBRATION_STATE_KEY) == 0;
            ChangeState(state);
            
            base.Awake();
        }

        /// <summary>
        /// This function helper for vibrate.
        /// </summary>
        public virtual void Vibrate()
        {
            if(!state)
                return;
            
            MMVibrationManager.Vibrate();
        }

        /// <summary>
        /// This function helper for haptic.
        /// </summary>
        public virtual void Haptic(HapticTypes hapticType)
        {
            if(!state)
                return;
            
            MMVibrationManager.Haptic(hapticType);
        }

        /// <summary>
        /// This function helper for change vibration state.
        /// </summary>
        /// <param name="state"></param>
        public virtual void ChangeState(bool state)
        {
            this.state = state;
            PlayerPrefs.SetInt(GRAMOFONCommonTypes.VIBRATION_STATE_KEY, state ? 0 : 1);

            VibrationStateChanged soundStateChangedEvent = new VibrationStateChanged()
            {
                State = state
            };
            
            EventService.DispatchEvent(soundStateChangedEvent);
        }
        
        /// <summary>
        /// This function returns related state.
        /// </summary>
        /// <returns></returns>
        public virtual bool GetState()
        {
            return state;
        }
    }
}