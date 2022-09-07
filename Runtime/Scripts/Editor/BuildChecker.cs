using GRAMOFON;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

#if GRAMOFON_GAME_ANALYTICS
using GameAnalyticsSDK;
#endif
#if GRAMOFON_FACEBOOK
using Facebook.Unity.Settings;
#endif
#if GRAMOFON_URP
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

public class BuildChecker : IPreprocessBuildWithReport
{
    #region Public Fields

    public int callbackOrder { get; set; }
    
    #endregion
    
    /// <summary>
    /// This function called when build process started.
    /// </summary>
    /// <param name="report"></param>
    public void OnPreprocessBuild(BuildReport report)
    {
        CheckBundleId();
        CheckURPSettings();
        CheckGameSettings();
        CheckFacebookSettings();
        CheckGameAnalyticsSettings();
    }

    /// <summary>
    /// This function helper for check bundle id of this project.
    /// </summary>
    private void CheckBundleId()
    {
        string bundleId = Application.identifier;

        if (string.IsNullOrEmpty(bundleId))
        {
            EditorUtility.DisplayDialog("Gramofon SDK", "Bundle Id is empty.", "OK");
            throw new BuildFailedException("Bundle Id is empty.");
        }

        if (!bundleId.Contains("gramofon"))
        {
            EditorUtility.DisplayDialog("Gramofon SDK", "Bundle Id is not contain 'gramofon' field.", "OK");
            throw new BuildFailedException("Bundle Id is not contain 'gramofon' field.");
        }
    }

    /// <summary>
    /// This function helper for check URP Settings.
    /// </summary>
    private void CheckURPSettings()
    {
        #if GRAMOFON_URP

        UniversalRenderPipelineAsset renderSettings = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

        if (renderSettings == null)
        {
            EditorUtility.DisplayDialog("Gramofon SDK", "URP Setup not correct on Graphic Settings.", "OK");
            throw new BuildFailedException("URP Setup not correct on Graphic Settings.");
        }

        string summaryText = "";

        summaryText += $"HDR Status : {(renderSettings.supportsHDR ? "Active" : "Disabled")}\n";
        summaryText += $"Depth Texture Status : {(renderSettings.supportsCameraDepthTexture ? "Active" : "Disabled")}\n";
        summaryText += $"Opaque Texture Status : {(renderSettings.supportsCameraOpaqueTexture ? "Active" : "Disabled")}\n";
        summaryText += $"SRP Batcher Status : {(renderSettings.useSRPBatcher ? "Active" : "Disabled")}\n";
        summaryText += $"Dynamic Batcher Status : {(renderSettings.supportsDynamicBatching ? "Active" : "Disabled")}\n";
        summaryText += $"Render Scale : {renderSettings.renderScale}\n";
        summaryText += $"Anti Alliasing Method : x{renderSettings.msaaSampleCount}\n";
        summaryText += $"Shadows Status : {(renderSettings.supportsMainLightShadows ? "Active" : "Disabled")}\n";
        summaryText += $"Soft Shadows Status : {(renderSettings.supportsSoftShadows ? "Active" : "Disabled")}\n";
        summaryText += $"Shadow Distance : {renderSettings.shadowDistance}\n";
        summaryText += $"Shadow Resolution : {renderSettings.mainLightShadowmapResolution}\n";
        summaryText += $"Shadow Cascade Count : {renderSettings.shadowCascadeCount}\n";
        
        bool isCanceled = !EditorUtility.DisplayDialog("Gramofon SDK",summaryText,"OK", "Cancel");

        if (isCanceled) 
            throw new BuildFailedException("Build canceled by Developer.");

        #endif
    }

    /// <summary>
    /// This function helper for check game settings.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BuildFailedException"></exception>
    public void CheckGameSettings()
    {
        BaseGameSettings gameSettings = Resources.Load(GRAMOFONCommonTypes.RESOURCES_GAME_SETTINGS_PATH) as BaseGameSettings;

        if (gameSettings != null)
        {
            if(gameSettings.Levels.Length == 0)
            {
                EditorUtility.DisplayDialog("Gramofon SDK", "Level list is empty.", "OK");
                throw new BuildFailedException("Level list is empty.");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Gramofon SDK", "Game Settings not found.", "OK");
            throw new BuildFailedException("Game Settings not found.");
        }
    }

    /// <summary>
    /// This function helper for check Facebook settings.
    /// </summary>
    private void CheckFacebookSettings()
    {
        #if GRAMOFON_FACEBOOK

        if(string.IsNullOrEmpty(FacebookSettings.AppId))
        {
            EditorUtility.DisplayDialog("Gramofon SDK", "Facebook App Id is empty.", "OK");
            throw new BuildFailedException("Facebook Settings not found.");
        }
        
        #endif
    }

    /// <summary>
    /// This function helper for check Game Analytics settings.
    /// </summary>
    private void CheckGameAnalyticsSettings()
    {
        #if GRAMOFON_GAME_ANALYTICS
        
        if (!GameAnalytics.SettingsGA.Platforms.Contains(RuntimePlatform.Android))
        {
            EditorUtility.DisplayDialog("Gramofon SDK", "Game Analytics platforms not included Android platform.", "OK");
            throw new BuildFailedException("Game Analytics platforms not included Android platform.");
        }
        
        if (!GameAnalytics.SettingsGA.Platforms.Contains(RuntimePlatform.IPhonePlayer))
        {
            EditorUtility.DisplayDialog("Gramofon SDK", "Game Analytics platforms not included iOS platform.", "OK");
            throw new BuildFailedException("Game Analytics platforms not included iOS platform.");
        }
        
        int targetPlatformCount = 2;

        for (int i = 0; i < targetPlatformCount; i++)
        {
            if (string.IsNullOrEmpty(GameAnalytics.SettingsGA.GetGameKey(i)))
            {
                EditorUtility.DisplayDialog("Gramofon SDK", $"Game Analytics Index : {i} game key is empty.", "OK");
                throw new BuildFailedException($"Game Analytics Index : {i} game key is empty.");
            }
            
            if (string.IsNullOrEmpty(GameAnalytics.SettingsGA.GetSecretKey(i)))
            {
                EditorUtility.DisplayDialog("Gramofon SDK", $"Game Analytics Index : {i} game secret is empty.", "OK");
                throw new BuildFailedException($"Game Analytics Index : {i} secret key is empty.");
            }
        }
        
        #endif
    }
}
