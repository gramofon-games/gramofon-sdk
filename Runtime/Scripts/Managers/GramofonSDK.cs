using GRAMOFON;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GramofonSDK : Singleton<GramofonSDK>
{
    #region Public Fields

    public static BaseGameManager GameManager;
    public static BaseTouchManager TouchManager;
    public static BaseThemeManager ThemeManager;
    public static BaseSoundManager SoundManager;
    public static BaseCameraManager CameraManager;
    public static BaseVibrationManager VibrationManager;
    public static BaseInterfaceManager InterfaceManager;
    public static BaseAnalyticsManager AnalyticsManager;
    public static BaseBootstrapManager BootstrapManager;
    public static BaseNotificationManager NotificationManager;
    
    public static BaseGameSettings BaseGameSettings;

    #endregion
    
    /// <summary>
    /// This function called when before first frame.
    /// </summary>
    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        BaseGameSettings = Resources.Load<BaseGameSettings>(GRAMOFONCommonTypes.RESOURCES_GAME_SETTINGS_PATH);
        
        BootstrapManager = gameObject.GetComponent<BaseBootstrapManager>();
        SoundManager =  gameObject.GetComponent<BaseSoundManager>();
        AnalyticsManager =  gameObject.GetComponent<BaseAnalyticsManager>();
        NotificationManager = gameObject.GetComponent<BaseNotificationManager>();
        
        BootstrapManager.Initialize();
        SoundManager.Initialize();
        AnalyticsManager.Initialize();
        NotificationManager.Initialize();

        base.Awake();
    }

    /// <summary>
    /// This function called when any scene loaded.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager = FindObjectOfType<BaseGameManager>();
        TouchManager = FindObjectOfType<BaseTouchManager>();
        ThemeManager = FindObjectOfType<BaseThemeManager>();
        CameraManager = FindObjectOfType<BaseCameraManager>();
        VibrationManager = FindObjectOfType<BaseVibrationManager>();
        InterfaceManager = FindObjectOfType<BaseInterfaceManager>();
    }

    /// <summary>
    /// This function called when related game object destroyed.
    /// </summary>
    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        base.OnDestroy();
    }
}
