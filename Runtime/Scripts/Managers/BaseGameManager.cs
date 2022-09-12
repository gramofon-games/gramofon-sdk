using UnityEngine;
using GRAMOFON.Misc;
using GRAMOFON.Enums;
using GRAMOFON.Models;
using GRAMOFON.Services;
using GRAMOFON.Components;

namespace GRAMOFON.Managers
{
    public class BaseGameManager : BaseManager
    {
        #region Serializable Fields

        [Header("Components")] 
        [SerializeField] private BasePlayerComponent m_basePlayerComponent;
        
        #endregion
        #region Private Fields

        private EGameState gameState = EGameState.NONE;
    
        #endregion

        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Initialize(params object[] parameters)
        {
            Application.targetFrameRate = GRAMOFONCommonTypes.DEFAULT_FPS;
            InitializeWorld();
            
            return base.Initialize(parameters);
        }
        
        /// <summary>
        /// This function helper for initialize world.
        /// </summary>
        protected virtual void InitializeWorld()
        {
            BaseLevel currentLevel = LevelService.GetCurrentLevel();
            GramofonSDK.ThemeManager.Initialize(currentLevel.Theme);
            
            ChangeGameState(EGameState.STAND_BY);
        }

        /// <summary>
        /// This function helper for start game.
        /// </summary>
        public virtual void StartGame()
        {
            ChangeGameState(EGameState.STARTED);
        }

        /// <summary>
        /// This function helper for change current game state.
        /// </summary>
        /// <param name="gameState"></param>
        public virtual void ChangeGameState(EGameState gameState)
        {
            if(this.gameState == EGameState.WIN)
                return;
            
            if(this.gameState == EGameState.LOSE)
                return;
            
            if(this.gameState == EGameState.STAND_BY && (gameState == EGameState.WIN || gameState == EGameState.LOSE))
                return;
            
            this.gameState = gameState;

            GameStateChangedEvent gameStateChangedEvent = new GameStateChangedEvent()
            {
                GameState = GetGameState()
            };

            EventService.DispatchEvent(gameStateChangedEvent);
        }

        /// <summary>
        /// This function returns related game state.
        /// </summary>
        /// <returns></returns>
        public virtual EGameState GetGameState()
        {
            return gameState;
        }
        
        /// <summary>
        /// This function returns related player component.
        /// </summary>
        /// <returns></returns>
        public virtual BasePlayerComponent GetPlayerComponent()
        {
            return m_basePlayerComponent;
        }

        #region EDITOR

        #if UNITY_EDITOR
        
        public virtual void Update()
        {
            if(Input.GetKeyDown(KeyCode.N))
                LevelService.NextLevel();
        
            if(Input.GetKeyDown(KeyCode.R))
                LevelService.RetryLevel();
            
            if(Input.GetKeyDown(KeyCode.M))
                GetPlayerComponent().IncreaseCurrency(100);
            
            if(Input.GetKeyDown(KeyCode.M))
                GramofonSDK.InterfaceManager.FlyCurrencyFromWorld(Vector3.zero);
            
            if(Input.GetKeyDown(KeyCode.O))
                ChangeGameState(EGameState.WIN);
            
            if(Input.GetKeyDown(KeyCode.P))
                ChangeGameState(EGameState.LOSE);
        }
        #endif

        #endregion
    }
}