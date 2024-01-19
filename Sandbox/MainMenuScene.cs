using FormsPixelEngine.FPE.Rendering.Text;
using FormsPixelEngine.FPE.Scene;
using FormsPixelEngine.FPE.Input;
using FormsPixelEngine.FPE.Sound;
using System;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FormsPixelEngine
{
    internal class MainMenuScene : Scene
    {
        private FPEFont menuFont;
        private AudioClip clip;

        public override void Init() {
            input.SubscribeKey(KeyCode.Space);
            menuFont = new FPEFont("./res/Font/boxy_bold_font.fpeFont", "./res/Font/boxy_bold_font.png");
            clip = new AudioClip("D:/Users/elkor/Documents/Projects/Music/Saves/FL/Slap-S.wav");
            //clip.PlayLoop();
        }

        public override void Update() {
            renderer.ClearScreen(new Vector3(200, 255, 220));
            renderer.DrawText(50, 100, "Press Space To Start Game", menuFont); 

            if (input.KeyPressed(KeyCode.Space))
                sceneManager.SetScene(new SandboxScene());
        }

        public override void Close() {
            input.UnsubscribeKey(KeyCode.Space);
            clip.Stop();
        }
    }
}
