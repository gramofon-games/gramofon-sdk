using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace GRAMOFON
{
    public class BaseBootstrapManager : BaseManager
    {
        #region Private Fields

        protected CancellationTokenSource skipCancellation;

        #endregion

        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Initialize(params object[] parameters)
        {
            Application.targetFrameRate = GRAMOFONCommonTypes.DEFAULT_FPS;
            _= Skip();
            
            return base.Initialize(parameters);
        }

        /// <summary>
        /// This function helper for skip bootstrap level.
        /// </summary>
        protected virtual async UniTask Skip()
        {
            try
            {
                if (skipCancellation == null)
                    skipCancellation = new CancellationTokenSource();
                
                await UniTask.Delay(TimeSpan.FromSeconds(GramofonSDK.BaseGameSettings.BootstrapDelay), DelayType.DeltaTime, PlayerLoopTiming.Update, skipCancellation.Token);
            
                LevelService.InitializeLevel();
            }
            catch (Exception e)
            {
                //ignored
            }
        }

        /// <summary>
        /// This function called when related game object destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            skipCancellation?.Cancel();
            
            base.OnDestroy();
        }
    }
}