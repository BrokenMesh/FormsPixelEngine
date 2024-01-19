using FormsPixelEngine.FPE.From;
using FormsPixelEngine.FPE.Rendering.Text;

namespace FlappyClone
{
    public class FlappyClone : GameWindow
    {
        FPEFont fontlarge;

        // FPS
        uint fpsCounter = 0;
        uint fpsTrackCount = 25;
        float fpsAdder = 0;
        float fps;
        // ---

        public FlappyClone(string _title, uint _windowWidth, uint _windowHeight, uint _pixelWidth, uint _pixelHeight) : base(_title, _windowWidth, _windowHeight, _pixelWidth, _pixelHeight) {
        }

        public override void OnInit() {
            fontlarge = new FPEFont("./res/Font/boxy_bold_font.fpeFont", "./res/Font/boxy_bold_font.png");
            sceneManager.SetScene(new MainMenu());
        }

        public override void OnUpdate() {
            // FPS ------
            //fpsCounter++;
            //fpsAdder += 1 / deltaTime;
            //
            //if (fpsCounter >= fpsTrackCount) {
            //    fps = fpsAdder / fpsTrackCount;
            //    fpsCounter = 0;
            //    fpsAdder = 0;
            //}
            //
            //renderer.DrawText(5, 5, $"FPS {(int)fps}", fontlarge);
        }
        public override void OnClose() {
        }
    }
}
