using GRAMOFON.Models;

#region IOS

#if UNITY_IOS
using System.Collections;
using Unity.Notifications.iOS;

#endif

#endregion
#region Android

using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;

#endif

#endregion

namespace GRAMOFON.Managers
{
    public class BaseNotificationManager : BaseManager
    {
        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Initialize(params object[] parameters)
        {
            BaseGameSettings gameSettings = GramofonSDK.BaseGameSettings;

            #if UNITY_IOS
            
            StartCoroutine(RequestIOSAuthorization());
            
            #endif

            #if UNITY_ANDROID
            
            foreach (CustomAndroidNotificationChannel customAndroidNotificationChannel in gameSettings.NotificationChannels)
            {
                AndroidNotificationChannel androidNotificationChannel = new AndroidNotificationChannel()
                {
                    Id = customAndroidNotificationChannel.Id,
                    Name = customAndroidNotificationChannel.Name,
                    Description = customAndroidNotificationChannel.Description,
                    Importance = customAndroidNotificationChannel.Importance
                };
                
                AndroidNotificationCenter.RegisterNotificationChannel(androidNotificationChannel);
            }
            
            #endif
            
            ScheduleNotification(gameSettings.LoopNotification);
            return base.Initialize(parameters);
        }

        /// <summary>
        /// This function helper for schedule notification.
        /// </summary>
        /// <param name="notification"></param>
        public virtual void ScheduleNotification(BaseNotification notification)
        {
            if(notification == null)
                return;
            
            #if UNITY_IOS

            iOSNotification iOSNotification = notification.GetIOSNotification();
            iOSNotificationCenter.ScheduleNotification(iOSNotification);
                
            #endif
            
            #if UNITY_ANDROID

            AndroidNotification androidNotification = notification.GetAndroidNotification();
            NotificationStatus notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(notification.Id);

            switch (notificationStatus)
            {
                case NotificationStatus.Unknown:
                    
                    AndroidNotificationCenter.SendNotificationWithExplicitID(androidNotification, notification.Channel, notification.Id);

                    break;
                case NotificationStatus.Scheduled:
                    
                    AndroidNotificationCenter.UpdateScheduledNotification(notification.Id, androidNotification, notification.Channel);
                    
                    break;
                case NotificationStatus.Delivered:
                    
                    AndroidNotificationCenter.CancelNotification(notification.Id);
                    
                    break;
            }

            #endif
        }
        
        #region IOS

        #if UNITY_IOS
        
        /// <summary>
        /// This function helper for cancel target notification.
        /// </summary>
        /// <param name="id"></param>
        public virtual void CancelNotification(string id)
        {
            iOSNotificationCenter.RemoveScheduledNotification(id);
            iOSNotificationCenter.RemoveDeliveredNotification(id);
        }
        
        /// <summary>
        /// This function helper for cancel all notifications.
        /// </summary>
        public virtual void CancelAllNotification()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }
        
        /// <summary>
        /// This function helper for take permission for notification.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator RequestIOSAuthorization()
        {
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using var req = new AuthorizationRequest(authorizationOption, true);
            
            while (!req.IsFinished)
            {
                yield return null;
            };
        }
        
        #endif
        
        #endregion
        #region Android

        #if UNITY_ANDROID

        /// <summary>
        /// This function helper for cancel target notification.
        /// </summary>
        /// <param name="id"></param>
        public virtual void CancelNotification(int id)
        {
            AndroidNotificationCenter.CancelNotification(id);
        }
        
        /// <summary>
        /// This function helper for cancel all notifications.
        /// </summary>
        public virtual void CancelAllNotificationAND()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }
        
        #endif

        #endregion
    }
}

