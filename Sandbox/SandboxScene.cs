using FormsPixelEngine.FPE.Rendering.Sprites;
using FormsPixelEngine.FPE.Rendering.Text;
using FormsPixelEngine.FPE.Rendering;
using FormsPixelEngine.FPE.Scene;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FormsPixelEngine.FPE.Input;

namespace FormsPixelEngine
{
    public class SandboxScene : Scene
    {
        Camera camera;
        FPEFont fontlarge;
        FPEFont fontsmall;
        TextureSprite sprite;

        TextureSprite[] spriteSheet;
        float animationTimer;
        float xpos = 50;
        float ypos = 50;

        public override void Init() {
            input.SubscribeKey(KeyCode.W);
            input.SubscribeKey(KeyCode.A);
            input.SubscribeKey(KeyCode.S);
            input.SubscribeKey(KeyCode.D);

            input.SubscribeKey(KeyCode.Left);
            input.SubscribeKey(KeyCode.Right);
            input.SubscribeKey(KeyCode.Up);
            input.SubscribeKey(KeyCode.Down);

            camera = new Camera(0, 0);

            fontlarge = new FPEFont("./res/Font/boxy_bold_font.fpeFont", "./res/Font/boxy_bold_font.png");
            fontsmall = new FPEFont("./res/Font/simple.fpeFont", "./res/Font/simple.png");

            sprite = new TextureSprite("./res/Textures/demo.png");

            spriteSheet = new TextureSprite[16];

            for (uint j = 0; j < 4; j++) {
                for (uint i = 0; i < 4; i++) {
                    spriteSheet[j * 4 + i] = new TextureSprite("./res/Textures/SheetDemo.png", 32, i, j);
                }
            }
        }

        public override void Update() {
            // Update -------------
            if (input.KeyPressed(KeyCode.S)) ypos += 25 * deltaTime;
            if (input.KeyPressed(KeyCode.W)) ypos -= 25 * deltaTime;
            if (input.KeyPressed(KeyCode.D)) xpos += 25 * deltaTime;
            if (input.KeyPressed(KeyCode.A)) xpos -= 25 * deltaTime;

            if (input.KeyPressed(KeyCode.Down)) camera.y += 30 * deltaTime;
            if (input.KeyPressed(KeyCode.Up)) camera.y -= 30 * deltaTime;
            if (input.KeyPressed(KeyCode.Right)) camera.x += 30 * deltaTime;
            if (input.KeyPressed(KeyCode.Left)) camera.x -= 30 * deltaTime;

            animationTimer += deltaTime / 4;
            if (animationTimer > 1) animationTimer = 0;
            uint index = (uint)(animationTimer * 16);

            // Render -------------
            renderer.PushCamera(camera);
            renderer.ClearScreen(new Vector3(0, 120, 0));

            renderer.SetDrawColor(new Vector3(255, 255, 0));
            renderer.DrawRect(100, 100, 30, 30);

            renderer.DrawSprite((int)xpos, (int)ypos, spriteSheet[index]);

            renderer.DrawSprite(10, 10, sprite);

            renderer.DrawText(200, 150, "Hello, World!\nThis is a Text!!!", fontlarge);
            renderer.DrawText(200, 200, "Hello, World!\nThis is a Text!!!", fontsmall);

            renderer.PopCamera();
        }

        public override void Close() {
        }

    }
}
