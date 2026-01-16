using System;
using Common.Singleton;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class InputManager : Singleton<InputManager>
    {
        
        private InputSystem_Actions _inputActions;
        protected override void AfterAwake()
        {
            
        }

        private void Start()
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Enable();
            
            _inputActions.UI.Point.performed += ctx => OnPointMoved(ctx);
            _inputActions.UI.Press.started += ctx => OnPress(ctx);
            _inputActions.UI.Press.canceled += ctx => OnRelease(ctx);
        }
        
        
        public void OnPress(InputAction.CallbackContext context)
        {
            var position = _inputActions.UI.Point.ReadValue<UnityEngine.Vector2>();
            InteractionManager.Instance.OnPress(position);
        }
        
        public void OnPointMoved(InputAction.CallbackContext context)
        {
            var position = _inputActions.UI.Point.ReadValue<UnityEngine.Vector2>();
            InteractionManager.Instance.OnPointMoved(position);
        }
        
        public void OnRelease(InputAction.CallbackContext context)
        {
            var position = _inputActions.UI.Point.ReadValue<UnityEngine.Vector2>();
            InteractionManager.Instance.OnRelease(position);
        }
    }
}