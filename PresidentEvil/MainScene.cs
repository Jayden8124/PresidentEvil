using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PresidentEvil;

public class MainScene : Game
{
    // Graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    // private Texture2D _background;

    // Font
    SpriteFont _font;

    // GameObjects
    List<GameObject> _gameObjects;
    public int _numOjects;

    // Camera & Map
    private Camera _camera;
    private Map _map;

    // Property Main Menu
    private Texture2D _startButton, _exitButton, _pauseButton;
    private Rectangle _startButtonPos, _exitButtonPos, _pauseButtonPos;

    public MainScene()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
        _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
        _graphics.ApplyChanges();

        _gameObjects = new List<GameObject>();

        _camera = new Camera(GraphicsDevice.Viewport); // Initialize camera
        _map = new Map(GraphicsDevice); // Initialize map

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("game_font");
        _map.LoadContent(Content);
        // _background = Content.Load<Texture2D>("bg");

        //Button
        _startButton = new Texture2D(GraphicsDevice, 1, 1);
        _startButton.SetData(new[] { Color.White });
        _startButtonPos = new Rectangle(500, 300, 200, 80);

        _exitButton = new Texture2D(GraphicsDevice, 1, 1);
        _exitButton.SetData(new[] { Color.White });
        _exitButtonPos = new Rectangle(1200, 20, 60, 40);

        _pauseButton = new Texture2D(GraphicsDevice, 1, 1);
        _pauseButton.SetData(new[] { Color.White });
        _pauseButtonPos = new Rectangle(1100, 20, 80, 40);

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        Singleton.Instance.CurrentMouse = Mouse.GetState();

        _numOjects = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Start:
                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                    Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released &&
                    _startButtonPos.Contains(Singleton.Instance.CurrentMouse.Position))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                break;

            case Singleton.GameState.GamePlaying: // Missing Key State
                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                    Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released &&
                    _exitButtonPos.Contains(Singleton.Instance.CurrentMouse.Position))
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
                }

                for (int i = 0; i < _numOjects; i++)
                {
                    if (_gameObjects[i].IsActive)
                    {
                        _gameObjects[i].Update(gameTime, _gameObjects);
                    }
                }

                for (int i = 0; i < _numOjects; i++)
                {
                    if (!_gameObjects[i].IsActive)
                    {
                        _gameObjects.RemoveAt(i);
                        i--;
                        _numOjects--;
                    }
                }

                if (Singleton.Instance.player != null)
                {
                    _camera.Follow(Singleton.Instance.player);
                }

                // // ดึงรายชื่อ tile collision จาก Map
                List<Rectangle> collisionTiles = _map.GetCollisionRectangles();

                // ตรวจสอบและแก้ไข collision สำหรับ Player
                CollisionManager.ResolveCollision(Singleton.Instance.player, collisionTiles);
                CollisionManager.UpdateOnGround(Singleton.Instance.player, collisionTiles);

                // ตรวจสอบและแก้ไข collision สำหรับ Monster แต่ละตัว
                foreach (var obj in _gameObjects)
                {
                    if (obj is MonsterType monster)
                    {
                        CollisionManager.ResolveCollision(monster, collisionTiles);
                        CollisionManager.UpdateOnGround(monster, collisionTiles);
                    }
                }
                break;

            case Singleton.GameState.GamePaused: //Game Paused
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;

                //Pause Code

                break;
            case Singleton.GameState.GameOver:
                break;
        }

        Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        // _DrawUI(); // Draw UI

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Start:
                {
                    GraphicsDevice.Clear(Color.Green);

                    _spriteBatch.Begin();

                    _spriteBatch.Draw(_startButton, _startButtonPos, Color.Gray);
                    _spriteBatch.DrawString(_font, "Start", GetCenteredTextPosition("Start", _font, _startButtonPos), Color.Black);

                    _spriteBatch.End();
                }
                break;
            case Singleton.GameState.GamePlaying:
                {
                    GraphicsDevice.Clear(Color.White);

                    /*--------------UI---------------*/
                    _spriteBatch.Begin();

                    _spriteBatch.Draw(_exitButton, _exitButtonPos, Color.Gray); // Exit
                    _spriteBatch.DrawString(_font, "Exit", GetCenteredTextPosition("Exit", _font, _exitButtonPos), Color.Black);

                    _spriteBatch.Draw(_pauseButton, _pauseButtonPos, Color.Gray); // Pause
                    _spriteBatch.DrawString(_font, "Pause", GetCenteredTextPosition("Pause", _font, _pauseButtonPos), Color.Black);

                    _spriteBatch.End();
                    /*------------------------------*/

                    _spriteBatch.Begin(transformMatrix: _camera.Transform);

                    _map.Draw(_spriteBatch);

                    for (int i = 0; i < _numOjects; i++)
                    {
                        _gameObjects[i].Draw(_spriteBatch);
                    }


                    _spriteBatch.End();
                }
                break;
            case Singleton.GameState.GamePaused:
                break;
            case Singleton.GameState.GameOver:
                break;
        }

        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    protected void Reset()
    {
        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.Start;

        _gameObjects.Clear();

        ResetPlayer(); // Call PLayer Object
        ResetMonster(); // Call Monster Object
        ResetObject(); // Call Object

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    public void _DrawUI()
    {
        // Layer 1: Background
        _spriteBatch.Begin();

        // _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Map & Camera
        _spriteBatch.Begin(transformMatrix: _camera.Transform);

        _map.Draw(_spriteBatch);

        for (int i = 0; i < _numOjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        _spriteBatch.End();

        // Layer 3: UI
        _spriteBatch.Begin();

        _spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score, new Vector2(10, 10), Color.Black);
        _spriteBatch.DrawString(_font, "Time: " + Singleton.Instance.Timer, new Vector2(10, 30), Color.Black);

        _spriteBatch.End();
    }

    public void ResetPlayer()
    {
        // Load Texture Singleton.Instance.player
        Texture2D knightSheet = Content.Load<Texture2D>("player");

        // Player Instance
        var _animationsPlayer = AnimationPlayer.LoadAnimations(knightSheet);

        Singleton.Instance.player = new Player(_animationsPlayer)
        {
            Name = "Player",
            Viewport = new Rectangle(5, 0, 43, 64),
            Position = new Vector2(100, Singleton.SCREENHEIGHT - 550),
            Left = Keys.Left,
            Right = Keys.Right,
            Up = Keys.Up,
            Down = Keys.Down,
            Fire = Keys.Space,
            Bullet = new Bullet(knightSheet)
            {
                Name = "BulletPlayer",
                Viewport = new Rectangle(0, 0, 10, 10),
                Velocity = new Vector2(-500f, 0)
            }
        };

        _gameObjects.Add(Singleton.Instance.player);
    }

    public void ResetMonster()
    {
        // Load Texture Monster
        var monsterTextures = new Dictionary<AnimationMonster.AnimationMonsterType, Texture2D>
        {
            { AnimationMonster.AnimationMonsterType.SKLT_WR, Content.Load<Texture2D>("skeleton_warrior") },
            { AnimationMonster.AnimationMonsterType.SKLT_SM, Content.Load<Texture2D>("skeleton_spearman") },
            { AnimationMonster.AnimationMonsterType.SKLT_AC, Content.Load<Texture2D>("skeleton_archer") },
            { AnimationMonster.AnimationMonsterType.SL, Content.Load<Texture2D>("blue_slime") }
        };

        // Monster Instance
        var _animationMonster = new AnimationMonster();
        _animationMonster.LoadAllAnimations(monsterTextures);

        // สร้าง monster โดยเรียกใช้ animation ที่ต้องการ (เช่น SKLT_WR)
        MonsterType monster = new SKLT_WR(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_WR))
        {
            Name = "SKLT1",
            Viewport = new Rectangle(0, 0, 36, 65),
            Position = new Vector2(500, Singleton.SCREENHEIGHT - 70)
        };

        MonsterType monster1 = new SL(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SL))
        {
            Name = "SL1",
            Viewport = new Rectangle(0, 0, 36, 65),
            Position = new Vector2(700, Singleton.SCREENHEIGHT - 70)
        };

        _gameObjects.Add(monster);
        _gameObjects.Add(monster1);
    }

    public void ResetObject()
    {
        // Load Texture Chest
        Texture2D chestSheet = Content.Load<Texture2D>("chest");

        // Chest Instance
        var _animationsChest = AnimationChest.LoadChestsAnimations(chestSheet);
        var goldChestAnimations = _animationsChest[ChestType.GoldChest];


        Chest chest = new Chest(goldChestAnimations)
        {
            Name = "Chest",
            openKey = Keys.E,
            Position = new Vector2(300, Singleton.SCREENHEIGHT - 300)
        };

        // Add GameObjects
        _gameObjects.Add(chest);
    }

    protected Vector2 GetCenteredTextPosition(string text, SpriteFont font, Rectangle buttonRect)
    {
        Vector2 textSize = font.MeasureString(text);
        return new Vector2(
            buttonRect.X + (buttonRect.Width - textSize.X) / 2,
            buttonRect.Y + (buttonRect.Height - textSize.Y) / 2
        );
    }
}