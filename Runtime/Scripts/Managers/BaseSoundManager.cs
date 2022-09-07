using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GRAMOFON
{
    public class BaseSoundManager : BaseManager
    {
        #region Private Fields

        private bool state;
        private AudioSource backgroundAudioSource;

        #endregion

        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Initialize(params object[] parameters)
        {
            EventService.AddListener<GameStateChangedEvent>(OnGameStateChanged);
            
            foreach (Sound sound in GramofonSDK.BaseGameSettings.Sounds)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                
                if(sound.Clips.Length == 0)
                    continue;
                
                AudioClip audioClip = sound.Clips[Random.Range(0, sound.Clips.Length)];
                
                source.clip = audioClip;
                source.pitch = sound.Pitch;
                source.volume = sound.Volume;
                source.loop = sound.IsLoop;

                sound.Source = source;  
            }

            state = PlayerPrefs.GetInt(GRAMOFONCommonTypes.SOUND_STATE_KEY) == 0;
            ChangeState(state);
            
            return base.Initialize(parameters);
        }

        /// <summary>
        /// This function helper for play sound with tag.
        /// </summary>
        /// <param name="tag"></param>
        public virtual void Play(string tag)
        {
            Sound targetSound = GramofonSDK.BaseGameSettings.Sounds.SingleOrDefault(x => x.Tag == tag);
            AudioClip targetClip = null;
            
            if (targetSound == null)
                return;

            if(targetSound.Clips.Length == 0)
                return;
            
            targetClip = targetSound.Clips[Random.Range(0, targetSound.Clips.Length)];

            if (targetClip == null)
            {
                return;
            }
            
            targetSound.Source.PlayOneShot(targetClip);
        }

        /// <summary>
        /// This function helper for change sound state.
        /// </summary>
        /// <param name="state"></param>
        public virtual void ChangeState(bool state)
        {
            AudioListener.volume = state ? 1 : 0;
            PlayerPrefs.SetInt(GRAMOFONCommonTypes.SOUND_STATE_KEY, state ? 0 : 1);

            SoundStateChangedEvent soundStateChangedEvent = new SoundStateChangedEvent()
            {
                State = state
            };
            
            EventService.DispatchEvent(soundStateChangedEvent);
        }
        
        /// <summary>
        /// This function helper for change background state.
        /// </summary>
        /// <param name="audioClip"></param>
        public virtual void PlayBackgroundMusic(AudioClip audioClip)
        {
            if(audioClip == null)
                return;
            
            if (backgroundAudioSource == null)
                backgroundAudioSource = gameObject.AddComponent<AudioSource>();
            
            if(backgroundAudioSource.clip == audioClip)
                return;

            backgroundAudioSource.clip = audioClip;
            backgroundAudioSource.Play();
        }

        /// <summary>
        /// This function called when game state changed.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGameStateChanged(GameStateChangedEvent e)
        {
            switch (e.GameState)
            {
                case EGameState.STAND_BY:

                    BaseLevel currentLevel = LevelService.GetCurrentLevel();
                    PlayBackgroundMusic(currentLevel.BackgroundMusic);
                    
                    break;
            }
        }
        
        /// <summary>
        /// This function returns related state.
        /// </summary>
        /// <returns></returns>
        public virtual bool GetState()
        {
            return state;
        }

        /// <summary>
        /// This function called when related game object destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            EventService.RemoveListener<GameStateChangedEvent>(OnGameStateChanged);

            base.OnDestroy();
        }
    }
}


