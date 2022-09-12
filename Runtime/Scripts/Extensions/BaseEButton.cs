using UnityEngine;
using GRAMOFON.Misc;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

namespace GRAMOFON.Extensions
{
    public class BaseEButton : Button
    {
        #region Serializable Fields

        [Header("General")] 
        [SerializeField] private string m_soundTag = GRAMOFONCommonTypes.SFX_CLICK;

        #endregion

        /// <summary>
        /// This function called when first frame.
        /// </summary>
        protected override void Start()
        {
            onClick.AddListener(() =>
            {
                if(!IsInteractable())
                    return;
        
                GramofonSDK.SoundManager.Play(m_soundTag);
                GramofonSDK.VibrationManager.Haptic(HapticTypes.HeavyImpact);
            });
        
            base.Start();
        }
    }
}

