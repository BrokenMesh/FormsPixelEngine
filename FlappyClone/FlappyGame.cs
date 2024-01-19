using FormsPixelEngine.FPE.Asset;
using FormsPixelEngine.FPE.Input;
using FormsPixelEngine.FPE.Rendering.Sprites;
using FormsPixelEngine.FPE.Rendering.Text;
using FormsPixelEngine.FPE.Scene;
using FormsPixelEngine.FPE.Scene.UI;
using System.Numerics;

namespace FlappyClone
{
    public class FlappyGame : Scene {

        private bool hardMode = false;

        private FPEFont smallFont;
        private FPEFont largeFont;

        private Menu gameMenu;
        private Vector2 menuPosition = Vector2.Zero;
        private bool showMenu = false;
        private bool menuWasPressed = false;

        private Menu respawnMenu;
        private bool showRespawnMenu = false;

        private bool hasStarted = false;

        private int score = 0;
        private int highScore = 0;
        private int targetPipe = 0;

        private TextureSprite backgroundTop;
        private TextureSprite backgroundBottom;

        private TextureSprite birdNormal;
        private TextureSprite birdUp;
        private TextureSprite birdDown;

        private TextureSprite pipeTop;
        private TextureSprite pipeBottom;

        private Random random = new Random();

        private const float JUMP_FORCE = 3.7f;
        private const float JUMP_DELAY = 0.2f;
        private const float MIN_SPEED_DIV = 1.2f;
        private const float GRAVITY = 11f;
        private const float GRAVITY_MUL = 1.3f;
        private const float PIPE_SPEED = 50f;
        private const float PIPE_SPEED_HARDMODE = 220f;
        private const float PIPE_SPACING = 85;
        private const float PIPE_SPACING_HARDMODE = 58;
        private const float PIPE_RANDSPAN = 100;
        private const float PIPE_SPAWNDELAY = 3.5f;
        private const float PIPE_SPAWNDELAY_HARDMODE = 1.4f;
        private const float BACKGROUND_SPEED = 20f;
        private const float BACKGROUND_SPEED_HARDMODE = 50f;

        private float birdX = 0;
        private float birdSpeed = 0;
        private float birdJumpTimer = JUMP_DELAY;
        private bool hasCollided = false;

        private List<Vector2> pipePositions = new List<Vector2>();
        private float pipeSpawnTimer = 0;

        private float pipeSpeed = PIPE_SPEED;
        private float pipeSpacing = PIPE_SPACING;
        private float pipeSpawnDelay = PIPE_SPAWNDELAY;

        private float backgroundSpeed = BACKGROUND_SPEED;
        private float background_pan = 0;

        private bool hasJumped;

        public FlappyGame(bool _isHardMode) : base() {
            hardMode = _isHardMode;
        }

        public override void Init() {

            // ---- Save ----
            GameData.Load("./flappy.json");

            highScore = GameData.current.Score;

            if (hardMode) {
                highScore = GameData.current.HardScore;
            }

            // ---- Game ----
            if (hardMode) {
                pipeSpeed = PIPE_SPEED_HARDMODE;
                pipeSpacing = PIPE_SPACING_HARDMODE;
                pipeSpawnDelay = PIPE_SPAWNDELAY_HARDMODE;
                backgroundSpeed = BACKGROUND_SPEED_HARDMODE;
            }

            input.SubscribeKey(KeyCode.Space);

            backgroundTop = AssetManager.LoadTextureSprite("Textures/Background/landscape_top.png");
            backgroundBottom = AssetManager.LoadTextureSprite("Textures/Background/landscape_bottom.png");

            birdNormal = AssetManager.LoadTextureSprite("Textures/Bird/Bird_Normal.png");
            birdUp = AssetManager.LoadTextureSprite("Textures/Bird/Bird_Up.png");
            birdDown = AssetManager.LoadTextureSprite("Textures/Bird/Bird_Down.png");

            pipeTop = AssetManager.LoadTextureSprite("Textures/Pipe/Pipe_Top.png");
            pipeBottom = AssetManager.LoadTextureSprite("Textures/Pipe/Pipe_Bottom.png");


            birdX = renderer.pixelHeight / 3;

            // ---- Menu ----
            input.SubscribeKey(KeyCode.Escape);
            input.SubscribeKey(KeyCode.Up);
            input.SubscribeKey(KeyCode.Down);
            input.SubscribeKey(KeyCode.Enter);

            largeFont = AssetManager.LoadFont("Font/boxy_bold_font.fpeFont", "Font/boxy_bold_font.png");
            smallFont = AssetManager.LoadFont("Font/Simple.fpeFont", "Font/Simple.png");

            TextureSprite _buttonDown_Sprite = AssetManager.LoadTextureSprite("Textures/UI/Button_Down.png");
            TextureSprite _buttonUp_Sprite = AssetManager.LoadTextureSprite("Textures/UI/Button_Up.png");

            gameMenu = new Menu(_buttonDown_Sprite, _buttonUp_Sprite, largeFont, input);
            gameMenu.AddButton("Resume", () => { showMenu = false; });
            gameMenu.AddButton("Quit", () => { sceneManager.SetScene(new MainMenu()); });

            menuPosition = new Vector2(renderer.pixelWidth/2 - _buttonUp_Sprite.Width/2, 70);

            // ---- Respawn Menu ----

            respawnMenu = new Menu(_buttonDown_Sprite, _buttonUp_Sprite, largeFont, input);
            respawnMenu.AddButton("Restart", () => { sceneManager.SetScene(new FlappyGame(hardMode)); });
            respawnMenu.AddButton("Quit", () => { sceneManager.SetScene(new MainMenu()); });
        }

        public override void Update() {
            // ---- Update ----
            UpdateMenu();
            UpdateGame();

            // ---- Render ----
            Render();
        }

        private void UpdateMenu() {
            if (showMenu) {
                gameMenu.Update();
            }

            if (showRespawnMenu) {
                respawnMenu.Update();
            }

            bool _menuWasPressed = menuWasPressed;
            menuWasPressed = false;

            if (input.KeyPressed(KeyCode.Escape)) {
                if (!_menuWasPressed)
                    showMenu = !showMenu;
                menuWasPressed = true;
            }

            if (!hasStarted) {
                if (input.KeyPressed(KeyCode.Space))
                    hasStarted = true;
            }

        }

        private void UpdateGame() {
            if (showMenu || showRespawnMenu || !hasStarted) return;

            // Bird
            float _gMul = birdSpeed > 0 ? GRAVITY_MUL : 1;

            birdSpeed += GRAVITY * deltaTime * _gMul;

            birdJumpTimer += deltaTime;

            bool _hasJumped = hasJumped;
            hasJumped = false;

            if (input.KeyPressed(KeyCode.Space)) {
                if (!_hasJumped && birdJumpTimer > JUMP_DELAY) {
                    birdSpeed = -JUMP_FORCE;
                    birdJumpTimer = 0;
                }

                hasJumped = true;
            }

            birdX += birdSpeed;

            // Background
            background_pan -= backgroundSpeed * deltaTime;

            if (background_pan < 0)
                background_pan = backgroundBottom.Width;

            // Pipe
            pipeSpawnTimer += deltaTime;

            if (pipeSpawnTimer > pipeSpawnDelay) {
                CreateNewPipe();
                pipeSpawnTimer = 0;
            }

            Sprite _sprite = GetBirdSprite();
            Vector4 _player = new Vector4(40f +5, (int)birdX +10, _sprite.Width -10, _sprite.Height -20);

            hasCollided = false;

            for (int i = pipePositions.Count - 1; i >= 0; i--) {
                pipePositions[i] = pipePositions[i] - new Vector2(pipeSpeed * deltaTime, 0);

                float _pipeX = pipePositions[i].X - 25;
                float _pipeY = pipePositions[i].Y;

                bool _hasCollided = Collide(_player, new Vector4(_pipeX, _pipeY + pipeSpacing / 2, 50, 200)) ||
                    Collide(_player, new Vector4(_pipeX, _pipeY - pipeSpacing / 2 - 200, 50, 200));

                if (_hasCollided)
                    hasCollided = true;

                if (i == targetPipe && pipePositions[i].X < 40) {
                    targetPipe++;
                    score++;
                }

                if (pipePositions[i].X < -100) {
                    pipePositions.RemoveAt(i);
                    targetPipe--;
                }
            }

            if (_player.Y < 0 || _player.Y + _player.W > renderer.pixelHeight) {
                hasCollided = true;
            }

            if (hasCollided) {
                showRespawnMenu = true;

                if (!hardMode) {
                    GameData.current.Score = Math.Max(GameData.current.Score, score);
                } else {
                    GameData.current.HardScore = Math.Max(GameData.current.HardScore, score);
                }

                GameData.Save("./flappy.json");
            }
        }

        private void Render() {
            // Game
            renderer.DrawSprite(0, 0, backgroundTop);

            renderer.DrawSprite((int)background_pan, 64, backgroundBottom);
            renderer.DrawSprite((int)background_pan - (int)backgroundBottom.Width, 64, backgroundBottom);

            renderer.DrawSprite(40, (int)birdX, GetBirdSprite());
            
            // Pipes
            renderer.SetDrawColor(new Vector3(200, 200, 200));
            foreach (Vector2 _p in pipePositions) {
                float _pipeX = _p.X - 25;
                float _pipeY = _p.Y;

                renderer.DrawSprite((int)_pipeX, (int)(_pipeY + pipeSpacing / 2), pipeBottom);
                renderer.DrawSprite((int)_pipeX, (int)(_pipeY - pipeSpacing / 2 - 200), pipeTop);

                //renderer.DrawRect((int)_pipeX, (int)(_pipeY + pipeSpacing / 2), 50, 200);
                //renderer.DrawRect((int)_pipeX, (int)(_pipeY - pipeSpacing / 2 - 200), 50, 200);
            }

            // Menu
            renderer.DrawText(
                (int)renderer.pixelWidth - 10 - largeFont.GetTextWidth(score.ToString()), 5, 
                score.ToString(), largeFont);

            renderer.DrawText(
                (int)renderer.pixelWidth - 10 - largeFont.GetTextWidth(highScore.ToString()), largeFont.GetTextHeight(highScore.ToString()) + 10,
                highScore.ToString(), largeFont);

            if (showMenu) {
                gameMenu.Render((int)menuPosition.X, (int)menuPosition.Y, 25, renderer);
            }

            if(showRespawnMenu){
                respawnMenu.Render((int)menuPosition.X, (int)menuPosition.Y, 25, renderer);
            }

            if (!hasStarted) {
                string _startLable = "Press Space to Start";
                renderer.DrawText((int)(renderer.pixelWidth/2 - largeFont.GetTextWidth(_startLable)/2), (int)(renderer.pixelHeight * 0.7f), _startLable, largeFont);
            }
        }

        private bool Collide(Vector4 _o1, Vector4 _o2) {
            return
                _o1.X <= _o2.X + _o2.Z &&
                _o1.X + _o1.Z >= _o2.X &&
                _o1.Y <= _o2.Y + _o2.W &&
                _o1.Y + _o1.W >= _o2.Y;
        }

        private void CreateNewPipe() {
            float _randY = (float)((PIPE_RANDSPAN * random.NextDouble()) + (renderer.pixelHeight - PIPE_RANDSPAN) / 2);
            pipePositions.Add(new Vector2(renderer.pixelWidth+100, _randY));
        }

        private Sprite GetBirdSprite() {
            if (birdSpeed > MIN_SPEED_DIV) return birdDown;
            if (birdSpeed < -MIN_SPEED_DIV) return birdUp;

            return birdNormal;
        }

        public override void Close() {
            input.UnsubscribeKey(KeyCode.Space);
            input.UnsubscribeKey(KeyCode.Up);
            input.UnsubscribeKey(KeyCode.Down);
            input.UnsubscribeKey(KeyCode.Enter);
        }
    }

}
