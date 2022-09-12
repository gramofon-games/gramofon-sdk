using System;
using System.Linq;
using UnityEngine;
using GRAMOFON.Misc;
using GRAMOFON.Models;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace GRAMOFON.Services
{
    public static class LevelService
    {
        /// <summary>
        /// This function helper for initialize last level.
        /// </summary>
        public static void InitializeLevel()
        {
            BaseGameSettings gameSettings = GramofonSDK.BaseGameSettings;
            
            int totalLevelCount = gameSettings.Levels.Length;
            int cachedLevelId = PlayerPrefs.GetInt(GRAMOFONCommonTypes.LEVEL_ID_DATA_KEY);
            
            BaseLevel targetLevel = gameSettings.Levels.SingleOrDefault(x => x.Id == cachedLevelId % totalLevelCount);

            if (targetLevel == null)
            {
                ErrorService.DispatchError($"Target Level is null! ID: {cachedLevelId}");
                return;
            }
            
            LevelStartEvent levelStartEvent = new LevelStartEvent()
            {
                Id = cachedLevelId + 1
            };
            
            EventService.DispatchEvent(levelStartEvent);
            
            LoadLevel(targetLevel.SceneName);
        }

        /// <summary>
        /// This function helper for initialize next level.
        /// </summary>
        /// <param name="score"></param>
        public static void NextLevel(int score = 0)
        {
            BaseGameSettings gameSettings = GramofonSDK.BaseGameSettings;
            
            int totalLevelCount = gameSettings.Levels.Length;
            int cachedLevelId = PlayerPrefs.GetInt(GRAMOFONCommonTypes.LEVEL_ID_DATA_KEY);
            int nextLevelId = cachedLevelId + 1;
            
            BaseLevel targetLevel = gameSettings.Levels.SingleOrDefault(x => x.Id == nextLevelId % totalLevelCount);
            BaseLevel previousLevel = gameSettings.Levels.SingleOrDefault(x => x.Id == cachedLevelId % totalLevelCount);

            if (targetLevel == null)
            {
                ErrorService.DispatchError($"Target Level is null! ID: {nextLevelId}");
                return;
            }
            
            if (previousLevel == null)
            {
                ErrorService.DispatchError($"Previous Level is null! ID: {cachedLevelId}");
                return;
            }
            
            LevelDoneEvent levelDoneEvent = new LevelDoneEvent()
            {
                Id = cachedLevelId + 1,
                Score = score
            };
            
            PlayerPrefs.SetInt(GRAMOFONCommonTypes.LEVEL_ID_DATA_KEY, nextLevelId);
            EventService.DispatchEvent(levelDoneEvent);
            InitializeLevel();
        }

        /// <summary>
        /// This function helper for initialize current level.
        /// </summary>
        /// <param name="score"></param>
        public static void RetryLevel(int score = 0)
        {
            BaseGameSettings gameSettings = GramofonSDK.BaseGameSettings;
            
            int totalLevelCount = gameSettings.Levels.Length;
            int cachedLevelId = GetCachedLevel();

            BaseLevel targetLevel = gameSettings.Levels.SingleOrDefault(x => x.Id == cachedLevelId % totalLevelCount);

            if (targetLevel == null)
            {
                ErrorService.DispatchError($"Target Level is null! ID: {cachedLevelId}");
                return;
            }
            
            LevelFailedEvent levelFailedEvent = new LevelFailedEvent()
            {
                Id = cachedLevelId + 1,
                Score = score
            };
            
            EventService.DispatchEvent(levelFailedEvent);
            InitializeLevel();
        }

        /// <summary>
        /// This function helper for load level.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loadSceneMode"></param>
        public static async void LoadLevel(string name, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            try
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name, loadSceneMode);

                while (!asyncOperation.isDone)
                {
                    LoadLevelProgressEvent loadLevelProgressEvent = new LoadLevelProgressEvent()
                    {
                        Name = name,
                        Progress = asyncOperation.progress
                    };
                
                    EventService.DispatchEvent(loadLevelProgressEvent);
                    await Task.Delay(GRAMOFONCommonTypes.DEFAULT_THREAD_SLEEP_MS);
                }
            
                LoadLevelEvent loadLevelEvent = new LoadLevelEvent()
                {
                    Name = name
                };

                EventService.DispatchEvent(loadLevelEvent);
            }
            catch (Exception e)
            {
                ErrorService.DispatchError(e.Message);
            }
        }

        /// <summary>
        /// This function helper for unload level.
        /// </summary>
        /// <param name="name"></param>
        public static async void UnLoadLevel(string name)
        {
            try
            {
                UnLoadLevelEvent unLoadLevelEvent = new UnLoadLevelEvent()
                {
                    Name = name
                };
            
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);

                while (!asyncOperation.isDone)
                {
                    UnLoadLevelProgressEvent unLoadLevelProgressEvent = new UnLoadLevelProgressEvent()
                    {
                        Name = name,
                        Progress = asyncOperation.progress
                    };
                
                    EventService.DispatchEvent(unLoadLevelProgressEvent);
                    await Task.Delay(GRAMOFONCommonTypes.DEFAULT_THREAD_SLEEP_MS);
                }
            
                EventService.DispatchEvent(unLoadLevelEvent);
            }
            catch (Exception e)
            {
                ErrorService.DispatchError(e.Message);
            }
        }

        /// <summary>
        /// This function returns current level.
        /// </summary>
        /// <returns></returns>
        public static BaseLevel GetCurrentLevel()
        {
            BaseGameSettings gameSettings = GramofonSDK.BaseGameSettings;
            
            int totalLevelCount = gameSettings.Levels.Length;
            int cachedLevelId = GetCachedLevel();

            BaseLevel targetLevel = gameSettings.Levels.SingleOrDefault(x => x.Id == cachedLevelId % totalLevelCount);

            return targetLevel;
        }

        /// <summary>
        /// This function returns true if target level is loaded.
        /// </summary>
        /// <returns></returns>
        public static bool IsLevelLoaded(string name)
        {
            bool isValid = false;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.name == name)
                {
                    isValid = true;
                    break;
                }
            }
            
            return isValid;
        }
        
        /// <summary>
        /// This function returns cached level id.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int GetCachedLevel(int offset = 0)
        {
            return PlayerPrefs.GetInt(GRAMOFONCommonTypes.LEVEL_ID_DATA_KEY) + offset;
        }
    }
}