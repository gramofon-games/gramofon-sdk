using UnityEngine;
using System.Collections.Generic;

namespace GRAMOFON
{
    [CreateAssetMenu(menuName = "GRAMOFON/Default/GameSettings", fileName = "GameSettings", order = 0)]
    public class BaseGameSettings : BaseScriptableObject
    {
        [Header("General")] 
        public float BootstrapDelay;
        public float FlyCurrencyDuration;

        [Header("Datas")]
        [ContextMenuItem("Update","FindLevels")]
        public BaseLevel[] Levels;
        public Sound[] Sounds;
        
        [Header("Notification")] 
        public BaseNotification LoopNotification;

        #if UNITY_ANDROID

        [Header("Android Notification Settings")] 
        public CustomAndroidNotificationChannel[] NotificationChannels;

        #endif

        #if UNITY_EDITOR
             
        /// <summary>
        /// This function helper for update levels list.
        /// </summary>
        public void FindLevels()
        {
            Levels = null;

            List<BaseLevel> foundLevels = new List<BaseLevel>();
            Object[] objects = Resources.LoadAll(GRAMOFONCommonTypes.EDITOR_LEVELS_PATH);

            foreach (Object targetObject in objects)
            {
                if(targetObject is not BaseLevel)
                    continue;
                
                foundLevels.Add(targetObject as BaseLevel);
            }

            Levels = foundLevels.ToArray();
        }
        
        #endif
   
    } 
}