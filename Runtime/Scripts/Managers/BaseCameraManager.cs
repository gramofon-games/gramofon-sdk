using UnityEngine;
using Cinemachine;

namespace GRAMOFON
{
    public class BaseCameraManager : BaseManager
    {
        #region Serializable Fields

        [Header("General")] 
        [SerializeField] private Camera m_camera;
        [SerializeField] private CinemachineVirtualCamera m_virtualCamera;

        #endregion

        /// <summary>
        /// This function returns related camera component.
        /// </summary>
        /// <returns></returns>
        public virtual Camera GetCamera()
        {
            return m_camera;
        }
        
        /// <summary>
        /// This function returns related virtual camera component.
        /// </summary>
        /// <returns></returns>
        public virtual CinemachineVirtualCamera GetVirtualCamera()
        {
            return m_virtualCamera;
        }
    }  
}

