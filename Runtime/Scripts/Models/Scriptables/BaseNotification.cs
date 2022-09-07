#region IOS

#if UNITY_IOS

using System;
using Unity.Notifications.iOS;

#endif

#endregion
#region Android

#if UNITY_ANDROID

using System;
using Unity.Notifications.Android;

#endif

#endregion

using UnityEngine;

namespace GRAMOFON
{
    [CreateAssetMenu(menuName = "GRAMOFON/Notification", fileName = "Notification", order = 2)]
    public class BaseNotification : BaseScriptableObject
    {
        #region IOS

        #if UNITY_IOS

        [Header("General")]
        public string Id;
        public string Title;
        public string Body;
        public string Subtitle;
        public string ThreadId;
        public string CategoryId;
        public bool IsShowInForeground;
        public bool IsRepeat;
            
        [Header("IOS")] 
        public PresentationOption PresentationOption;
        
        public iOSNotification GetIOSNotification()
        { 
            iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(Hour, Minute, Second),
                Repeats = IsRepeat
            };

            return new iOSNotification()
            {
                Identifier = Id,
                Title = Title,
                Body = Body,
                Subtitle = Subtitle,
                ShowInForeground = IsShowInForeground,
                ForegroundPresentationOption = PresentationOption,
                CategoryIdentifier = CategoryId,
                ThreadIdentifier = ThreadId,
                Trigger = timeTrigger,
            };
        }
        
        #endif

        #endregion
        #region ANDROID

        #if UNITY_ANDROID
        
        [Header("General")]
        public int Id;
        public string Title;
        public string Text;
        public string Channel;
        public string SmallIcon;
        public string LargeIcon;
        public bool IsRepeat;

        public AndroidNotification GetAndroidNotification()
        {
            DateTime dateTime = DateTime.Now + new TimeSpan(Hour,Minute,Second);

            return new AndroidNotification()
            {
                FireTime = dateTime,
                Title = Title,
                Text = Text,
                SmallIcon = SmallIcon,
                LargeIcon = LargeIcon,
                RepeatInterval = IsRepeat ? new TimeSpan(Hour, Minute, Second) : TimeSpan.Zero
            };
        }
        
        #endif

        #endregion

        [Header("Time")]
        public int Hour;
        public int Minute;
        public int Second;
    }
}