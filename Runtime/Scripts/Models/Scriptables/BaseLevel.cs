using UnityEngine;

namespace GRAMOFON.Models
{
    public class BaseLevel : BaseScriptableObject
    {
        [Header("General")]
        public int Id;
        public string SceneName;
        public BaseTheme Theme;
        public AudioClip BackgroundMusic;

        [Header("Misc")]
        public int EndGameBonusCurrency;
    }
}