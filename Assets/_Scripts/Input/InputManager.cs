using _Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Input
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : Singleton<InputManager>
    {
        public delegate void PressDown(Vector3 position);
        public event PressDown OnPressDown;
    
        public delegate void PressUp(Vector3 position);
        public event PressUp OnPressUp;
        
        public delegate void GoBack();
        public event GoBack OnGoBackPress;
    
        private GameInput _gameInput;
        private Camera _mainCamera;

        private void Awake()
        {
            _gameInput = new GameInput();
        }
        

        private void Start()
        {
            _gameInput.Draw.Tap.started += StartTouchPosition;
            _gameInput.Draw.Tap.canceled += EndTouchPosition;

            _gameInput.Draw.GoBack.started += GoBackPress;
        }

        public void Enable()
        {
            _mainCamera = Camera.main;
            _gameInput.Enable();
        }
        
        public void Disable()
        {
            _gameInput.Disable();
        }

        private void GoBackPress(InputAction.CallbackContext obj)
        {
            OnGoBackPress?.Invoke();
        }
        
        private void StartTouchPosition(InputAction.CallbackContext ctx)
        {
            OnPressDown(Utils.Utils.ScreenToWorld(_mainCamera, _gameInput.Draw.Position.ReadValue<Vector2>()));
        }
    
        private void EndTouchPosition(InputAction.CallbackContext ctx)
        {
            OnPressUp(Utils.Utils.ScreenToWorld(_mainCamera, _gameInput.Draw.Position.ReadValue<Vector2>()));
        }

        public Vector3 CurrentPosition() => 
            Utils.Utils.ScreenToWorld(_mainCamera, _gameInput.Draw.Position.ReadValue<Vector2>());
    }
}
