using UnityEngine;

namespace GRAMOFON
{
    public class BaseThemeManager : BaseManager
    {
        #region Private Fields

        private BaseTheme theme;

        #endregion
        
        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Initialize(params object[] parameters)
        {
            theme = (BaseTheme) parameters[0];
            RenderSettings.skybox = GetTheme().SkyBox;

            return base.Initialize(parameters);
        }

        /// <summary>
        /// This function returns related theme.
        /// </summary>
        /// <returns></returns>
        public virtual BaseTheme GetTheme()
        {
            return theme;
        }
    }
}