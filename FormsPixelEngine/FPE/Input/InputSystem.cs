using System.Numerics;

namespace FormsPixelEngine.FPE.Input
{
    /*! \class InputSystem
    \brief Class for handling user Input

    This class allows the subscribing to keys. if a subscribed key is being pressed the KeyPressed method would return true.  
    */
    public class InputSystem
    {
        public Vector2 MousePosition { get; private set; } //!< the current mouse position 

        private Dictionary<KeyCode, bool> KeyStates;
        private Dictionary<MouseButton, bool> MouseButtonStates;

        public InputSystem() {
            MousePosition = new Vector2(0,0);
            KeyStates = new Dictionary<KeyCode, bool>();
            MouseButtonStates = new Dictionary<MouseButton, bool>();
        }

        /// @private
        public void UpdateKey(uint keyValue, bool _state) {
            if (!KeyStates.ContainsKey((KeyCode)keyValue)) return;
            KeyStates[(KeyCode)keyValue] = _state;
        }
        /// @private
        public void UpdateMousePosition(uint x, uint y) { 
            MousePosition = new Vector2(x, y);
        }
        /// @private
        public void UpdateMouseButtons(uint ButtonValue, bool _state) {
            if (!MouseButtonStates.ContainsKey((MouseButton)ButtonValue)) return;
            MouseButtonStates[(MouseButton)ButtonValue] = _state;
        }

        //! Returns true if the given key was pressed.
        /*!
          \param Key The Key that will be checked
          \sa KeyCode.cs
        */
        public bool KeyPressed(KeyCode _key) {
            if(!KeyStates.ContainsKey(_key)) return false; 
            return KeyStates[_key];
        }

        //! Adds the key to the Subscriptions and checks its state every Frame. 
        /*!
          \param Key The Key that will be added
          \sa KeyCode.cs
        */
        public void SubscribeKey(KeyCode _key) {
            if (!KeyStates.ContainsKey(_key)) KeyStates.Add(_key, false);
        }

        //! Removes the key to the Subscriptions and stops checking its state every Frame. 
        /*!
          \param Key The Key that will be removed
          \sa KeyCode.cs
        */
        public void UnsubscribeKey(KeyCode _key) {
            KeyStates.Remove(_key);
        }


        //! Returns true if the given mouse button was pressed.
        /*!
          \param Button The mouse button that will be checked
          \sa KeyCode.cs
        */
        public bool ButtonPressed(MouseButton _button) {
            if (!MouseButtonStates.ContainsKey(_button)) return false;
            return MouseButtonStates[_button];
        }

        //! Adds the mouse button to the Subscriptions and checks its state every Frame. 
        /*!
          \param Button The mouse button that will be added
          \sa KeyCode.cs
        */
        public void SubscribeButton(MouseButton _button) {
            MouseButtonStates.Add(_button, false);
        }

        //! Removes the mouse button to the Subscriptions and stops checking its state every Frame. 
        /*!
          \param Button The mouse button that will be removed
          \sa KeyCode.cs
        */
        public void UnsubscribeButton(MouseButton _button) {
            MouseButtonStates.Remove(_button);
        }
    }
}
