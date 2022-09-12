using UnityEngine;
using GRAMOFON.Services;

namespace GRAMOFON.Managers
{
    public class BaseAnalyticsManager : BaseManager
    {
        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Initialize(params object[] parameters)
        {
            EventService.AddListener<LevelStartEvent>(OnLevelStart);
            EventService.AddListener<LevelDoneEvent>(OnLevelDone);
            EventService.AddListener<LevelFailedEvent>(OnLevelFailed);
            
            return base.Initialize(parameters);
        }

        /// <summary>
        /// This function called when level started.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLevelStart(LevelStartEvent e)
        {
            Debug.Log($"Level Started. ID : {e.Id}");
        }
        
        /// <summary>
        /// This function called when current level done.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLevelDone(LevelDoneEvent e)
        {
            Debug.Log($"Level Done. ID : {e.Id}");
        }
        
        /// <summary>
        /// This function called when current level failed.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLevelFailed(LevelFailedEvent e)
        {
            Debug.Log($"Level Failed. ID : {e.Id}");
        }
    
        /// <summary>
        /// This function called when this component destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            EventService.RemoveListener<LevelStartEvent>(OnLevelStart);
            EventService.RemoveListener<LevelDoneEvent>(OnLevelDone);
            EventService.RemoveListener<LevelFailedEvent>(OnLevelFailed);
            
            base.OnDestroy();
        }
    }
}