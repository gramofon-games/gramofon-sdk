using System;
using UnityEngine;
using GRAMOFON.Enums;

namespace GRAMOFON.Components
{
    public class BaseComponent : MonoBehaviour
    {
        #region Public Fields

        public Action<object[]> Initialized;

        #endregion
        #region Serializable Fields
        
        [Header("Base")]
        [SerializeField] private EInitializationType m_initializationType;

        #endregion

        /// <summary>
        /// This function called when before first frame.
        /// </summary>
        protected virtual void Awake()
        {
            if (GetInitializationType() == EInitializationType.AWAKE)
                Initialize();
        }
        
        /// <summary>
        /// This function called when first frame.
        /// </summary>
        protected virtual void Start()
        {
            if (GetInitializationType() == EInitializationType.START)
                Initialize();
        }

        /// <summary>
        /// This function helper for initialize this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual bool Initialize(params object[] parameters)
        {
            Initialized?.Invoke(parameters);
            return true;
        }

        /// <summary>
        /// This function returns related initialization type.
        /// </summary>
        /// <returns></returns>
        public EInitializationType GetInitializationType()
        {
            return m_initializationType;
        }

        /// <summary>
        /// This function called when related game object destroyed.
        /// </summary>
        protected virtual void OnDestroy() {}
    }
}
