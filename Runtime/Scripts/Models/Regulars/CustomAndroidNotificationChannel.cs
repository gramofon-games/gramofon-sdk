#if UNITY_ANDROID

using System;
using Unity.Notifications.Android;

namespace GRAMOFON.Models
{
    [Serializable]
    public class CustomAndroidNotificationChannel : BaseModel
    {
        public string Id;
        public string Name;
        public string Description;
        public Importance Importance;
    } 
}

#endif