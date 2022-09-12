using UnityEngine;

namespace GRAMOFON.Managers
{
    public class BaseTouchManager : BaseManager
    {
        #region Serializable Fields

        [Header("General")] 
        [SerializeField] private VariableJoystick m_variableJoystick;

        #endregion
        #region Private Fields

        private float touchVelocity;
        private Vector3 lastTouchPosition;

        #endregion

        /// <summary>
        /// This function called when per frame.
        /// </summary>
        protected virtual void Update()
        {
            touchVelocity = GetTouchDirection().magnitude;
            lastTouchPosition = Input.mousePosition;
        }

        /// <summary>
        /// This function helper for change related joystick active state.
        /// </summary>
        /// <param name="state"></param>
        public virtual void ChangeJoystickState(bool state)
        {
            GetVariableJoystick().gameObject.SetActive(state);
        }

        /// <summary>
        /// This function returns horizontal axis of related variable joystick.
        /// </summary>
        /// <returns></returns>
        public virtual float GetJoystickHorizontal()
        {
            return GetVariableJoystick().Horizontal;
        }
        
        /// <summary>
        /// This function returns vertical axis of related variable joystick.
        /// </summary>
        /// <returns></returns>
        public virtual float GetJoystickVertical()
        {
            return GetVariableJoystick().Vertical;
        }

        /// <summary>
        /// This function returns direction of related direction.
        /// </summary>
        /// <returns></returns>
        public virtual Vector2 GetJoystickDirection()
        {
            return GetVariableJoystick().Direction;
        }
        
        /// <summary>
        /// This function returns related variable joystick component.
        /// </summary>
        /// <returns></returns>
        public virtual VariableJoystick GetVariableJoystick()
        {
            return m_variableJoystick;
        }

        /// <summary>
        /// This function returns related touch velocity.
        /// </summary>
        /// <param name="isNormalized"></param>
        /// <returns></returns>
        public virtual float GetTouchVelocity(bool isNormalized = false)
        {
            return Mathf.Clamp(touchVelocity,0,isNormalized ? 1 : Mathf.Infinity);
        }

        /// <summary>
        /// This function returns related touch direction.
        /// </summary>
        /// <param name="isNormalized"></param>
        /// <returns></returns>
        public virtual Vector3 GetTouchDirection(bool isNormalized = false)
        {
            if(isNormalized)
                return (Input.mousePosition - lastTouchPosition).normalized;

            return Input.mousePosition - lastTouchPosition;
        }

        /// <summary>
        /// This function returns true if player touching screen.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTouching()
        {
            return Input.GetMouseButton(0);
        }

        /// <summary>
        /// This function returns true if player touching up.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTouchUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        /// <summary>
        /// This function returns true if player touching down.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTouchDown()
        {
            return Input.GetMouseButtonDown(0);
        }
    } 
}