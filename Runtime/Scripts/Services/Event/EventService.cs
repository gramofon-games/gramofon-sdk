using System;
using System.Collections.Generic;

namespace GRAMOFON
{
    public static class EventService
    {
        #region Public Fields

        public delegate void EventDelegate<T> (T e) where T : BaseEvent;

        #endregion
        #region Private Fields

        private delegate void EventDelegate (BaseEvent e);
        private static readonly Dictionary<Type, EventDelegate> delegates = new Dictionary<Type, EventDelegate>();
        private static readonly Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();

        #endregion

        /// <summary>
        /// This function helper for dispatch event.
        /// </summary>
        /// <param name="baseEvent"></param>
        public static void DispatchEvent(BaseEvent baseEvent)
        {
            if (delegates.TryGetValue(baseEvent.GetType(), out EventDelegate eventDelegate))
                eventDelegate.Invoke(baseEvent);
        }

        /// <summary>
        /// This function helper for add lister.
        /// </summary>
        /// <param name="eventDelegate"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddListener<T>(EventDelegate<T> eventDelegate) where T : BaseEvent
        {
            if(delegateLookup.ContainsKey(eventDelegate))
                return;

            void InternalDelegate(BaseEvent e) => eventDelegate((T) e);
            delegateLookup[eventDelegate] = InternalDelegate;
            
            if (delegates.TryGetValue(typeof(T), out EventDelegate tempDelegate))
            {
                delegates[typeof(T)] += InternalDelegate;
            }
            else
            {
                delegates[typeof(T)] = InternalDelegate;
            }
        }

        /// <summary>
        /// This function helper for remove listener.
        /// </summary>
        /// <param name="eventDelegate"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveListener<T>(EventDelegate<T> eventDelegate) where T : BaseEvent
        {
            if (delegateLookup.TryGetValue(eventDelegate, out EventDelegate internalDelegate)) {
                
                if (delegates.TryGetValue(typeof(T), out EventDelegate tempDelegate))
                {
                    tempDelegate -= internalDelegate;
                    
                    if (tempDelegate == null)
                    {
                        delegates.Remove(typeof(T));
                    } 
                    else 
                    {
                        delegates[typeof(T)] = tempDelegate;
                    }
                }

                delegateLookup.Remove(eventDelegate);
            }
        }

        /// <summary>
        /// This function helper for remove all listener.
        /// </summary>
        public static void RemoveAllListener()
        {
            delegates.Clear();
            delegateLookup.Clear();
        }
    }
}


