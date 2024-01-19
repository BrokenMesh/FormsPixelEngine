using FormsPixelEngine.FPE.Asset;
using FormsPixelEngine.FPE.Input;
using FormsPixelEngine.FPE.Rendering.Sprites;
using FormsPixelEngine.FPE.Rendering.Text;
using FormsPixelEngine.FPE.Scene;
using FormsPixelEngine.FPE.Scene.UI;

namespace FlappyClone
{
    public class MainMenu : Scene
    {
        private FPEFont smallFont;
        private FPEFont largeFont;

        private Menu mainMenu;

        private TextureSprite backgroundTop;
        private TextureSprite backgroundBottom;

        private float background_pan = 0;

        private TextureSprite birdNormal;

        private const float BACKGROUND_SPEED = 10f;

        public override void Init() {
            input.SubscribeKey(KeyCode.Up);
            input.SubscribeKey(KeyCode.Down);
            input.SubscribeKey(KeyCode.Enter);

            largeFont = AssetManager.LoadFont("Font/boxy_bold_font.fpeFont", "Font/boxy_bold_font.png");
            smallFont = AssetManager.LoadFont("Font/Simple.fpeFont", "Font/Simple.png");

            TextureSprite _buttonDown_Sprite = AssetManager.LoadTextureSprite("Textures/UI/Button_Down.png");
            TextureSprite _buttonUp_Sprite = AssetManager.LoadTextureSprite("Textures/UI/Button_Up.png");

            mainMenu = new Menu(_buttonDown_Sprite, _buttonUp_Sprite, largeFont, input);
            mainMenu.AddButton("Start", () => { sceneManager.SetScene(new FlappyGame(false)); });
            mainMenu.AddButton("Hard Mode", () => { sceneManager.SetScene(new FlappyGame(true)); });
            mainMenu.AddButton("Reset", Reset);
            mainMenu.AddButton("Quit", () => { System.Environment.Exit(0); });

            backgroundTop = AssetManager.LoadTextureSprite("Textures/Background/landscape_top.png");
            backgroundBottom = AssetManager.LoadTextureSprite("Textures/Background/landscape_bottom.png");

            birdNormal = AssetManager.LoadTextureSprite("Textures/Bird/Bird_Normal.png");
        }

        public override void Update() {
            // Update
            mainMenu.Update();

            background_pan -= BACKGROUND_SPEED * deltaTime;

            if (background_pan < 0)
                background_pan = backgroundBottom.Width;

            // Render
            renderer.DrawSprite(0, 0, backgroundTop);

            renderer.DrawSprite((int)background_pan, 64, backgroundBottom);
            renderer.DrawSprite((int)background_pan - (int)backgroundBottom.Width, 64, backgroundBottom);

            renderer.DrawText(20, 20, "Flappy Bird", largeFont);
            mainMenu.Render(20, 70, 25, renderer);
        }

        public void Reset() {
            GameData.Load("./flappy.json");
            GameData.current.Score = 0;
            GameData.current.HardScore = 0;
            GameData.Save("./flappy.json");
        }

        public override void Close() {
            input.UnsubscribeKey(KeyCode.Up);
            input.UnsubscribeKey(KeyCode.Down);
            input.UnsubscribeKey(KeyCode.Enter);
        }
    }
}
