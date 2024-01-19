using FormsPixelEngine.FPE.Input;
using FormsPixelEngine.FPE.Rendering;
using FormsPixelEngine.FPE.Rendering.Sprites;
using FormsPixelEngine.FPE.Rendering.Text;

namespace FormsPixelEngine.FPE.Scene.UI
{
    public class Menu
    {
        InputSystem input;

        private FPEFont font;

        private TextureSprite buttonDown_Sprite;
        private TextureSprite buttonUp_Sprite;

        private Dictionary<String, Action> buttons;

        private int currentSelectedButton = 0;
        private bool wasPressed = false;

        public Menu(TextureSprite _buttonDownSprite, TextureSprite _buttonUpSprite, FPEFont _font, InputSystem _inputSystem) {
            buttonDown_Sprite = _buttonDownSprite;
            buttonUp_Sprite = _buttonUpSprite;
            font = _font;
            input = _inputSystem;

            input.SubscribeKey(KeyCode.Up);
            input.SubscribeKey(KeyCode.Down);
            input.SubscribeKey(KeyCode.Enter);

            buttons = new Dictionary<String, Action>();
        }

        public void AddButton(string _lable, Action _callback) {
            buttons.Add(_lable, _callback);
        }

        public void Update() {
            bool _wasPressed = wasPressed;
            wasPressed = false;

            if (input.KeyPressed(KeyCode.Down)) {
                if (!_wasPressed) currentSelectedButton++;
                wasPressed = true;
            }

            if (input.KeyPressed(KeyCode.Up)) {
                if (!_wasPressed) currentSelectedButton--;
                wasPressed = true;
            }

            if (currentSelectedButton >= buttons.Count) currentSelectedButton = 0;
            if (currentSelectedButton < 0) currentSelectedButton = buttons.Count - 1;

            if (input.KeyPressed(KeyCode.Enter)) {
                if (!_wasPressed) buttons.Values.ElementAt(currentSelectedButton).Invoke();
                wasPressed = true;
            }
        }

        public void Render(int _x, int _y, int _spacing, Renderer _renderer) {
            for (int i = 0; i < buttons.Count; i++) {
                if (i == currentSelectedButton) {
                    _renderer.DrawSprite(_x, _y + (_spacing * i), buttonDown_Sprite);
                }
                else {
                    _renderer.DrawSprite(_x, _y + (_spacing * i), buttonUp_Sprite);
                }

                string _lable = buttons.Keys.ElementAt(i);
                int _strWidht = font.GetTextWidth(_lable);
                int _strHeight= font.GetTextHeight(_lable);
                _renderer.DrawText(_x - (_strWidht/2) + ((int)buttonDown_Sprite.Width/2), 
                                   _y + (_spacing * i) - (_strHeight/2) + ((int)buttonDown_Sprite.Height/2),
                                   _lable, font);
            }
        }

    }
}
