using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PresidentEvil;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    // SpriteFont _font;
    List<GameObject> _gameObjects;
    Texture2D _background;
    public int _numOjects;
    private Camera _camera;
    Player player;

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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // _font = Content.Load<SpriteFont>("Name");
        _background = Content.Load<Texture2D>("map");

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();
        _numOjects = _gameObjects.Count;

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

        // Make camera follow the player
        if (player != null)
        {
            _camera.Follow(player);
        }

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin(transformMatrix: _camera.Transform);

        _spriteBatch.Draw(_background, new Rectangle(0, Singleton.SCREENHEIGHT - _background.Height, _background.Width * 3, _background.Height), Color.White);

        for (int i = 0; i < _numOjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        _spriteBatch.End();
        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    protected void Reset()
    {
        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
        Texture2D knightSheet = Content.Load<Texture2D>("player");
        _gameObjects.Clear();

        // Idle Animation (4 frames)
        var idleFrames = new List<Rectangle>
        {
            new Rectangle(5,   0, 43, 64),
            new Rectangle(72,  0, 43, 64),
            new Rectangle(139, 0, 43, 64),
            new Rectangle(206, 0, 43, 64)
        };

        // Walk Animation (8 frames)
        var walkFrames = new List<Rectangle>
        {
            new Rectangle(8,   80, 40, 64),
            new Rectangle(74,  81, 43, 63),
            new Rectangle(143, 81, 48, 63),
            new Rectangle(217, 80, 47, 64),
            new Rectangle(290, 80, 46, 64),
            new Rectangle(342, 81, 43, 63),
            new Rectangle(411, 80, 43, 64),
            new Rectangle(480, 80, 44, 64)
        };

        // Run Animation (7 frames)
        var runFrames = new List<Rectangle>
        {
            new Rectangle(2,   163, 48, 61),
            new Rectangle(69,  165, 54, 59),
            new Rectangle(142, 162, 49, 62),
            new Rectangle(210, 161, 51, 63),
            new Rectangle(280, 164, 48, 60),
            new Rectangle(347, 165, 54, 59),
            new Rectangle(420, 162, 48, 62)
        };

        // Jump Animation (6 frames)
        var jumpFrames = new List<Rectangle>
        {
            new Rectangle(2,   247, 49, 57),
            new Rectangle(81,  240, 49, 64),
            new Rectangle(153, 240, 46, 64),
            new Rectangle(229, 239, 69, 65),
            new Rectangle(305, 246, 64, 58),
            new Rectangle(385, 247, 61, 57)
        };

        // Attack1 Animation (5 frames)
        var attack1Frames = new List<Rectangle>
        {
            new Rectangle(4,   320, 43, 64),
            new Rectangle(70,  320, 59, 64),
            new Rectangle(152, 320, 64, 64),
            new Rectangle(251, 320, 33, 64),
            new Rectangle(319, 310, 85, 74)
        };

        // Attack2 Animation (4 frames)
        var attack2Frames = new List<Rectangle>
        {
            new Rectangle(6,   405, 42, 75),
            new Rectangle(101, 409, 43, 71),
            new Rectangle(198, 412, 88, 68),
            new Rectangle(299, 416, 66, 64)
        };

        // Attack3 Animation (4 frames)
        var attack3Frames = new List<Rectangle>
        {
            new Rectangle(9,   496, 48, 64),
            new Rectangle(113, 496, 49, 64),
            new Rectangle(217, 498, 85, 62),
            new Rectangle(315, 501, 80, 59)
        };

        // Dead Animation (6 frames)
        var deadFrames = new List<Rectangle>
        {
            new Rectangle(5,   583, 43, 58),
            new Rectangle(78,  596, 47, 45),
            new Rectangle(160, 607, 45, 34),
            new Rectangle(239, 608, 50, 33),
            new Rectangle(316, 608, 54, 33),
            new Rectangle(392, 608, 54, 33)
        };

        // Defend Animation (5 frames)
        var defendFrames = new List<Rectangle>
        {
            new Rectangle(7,   660, 43, 60),
            new Rectangle(83,  659, 43, 61),
            new Rectangle(159, 659, 43, 61),
            new Rectangle(235, 659, 43, 61),
            new Rectangle(311, 660, 43, 60)
        };

        // Create Animation objects (adjust frame speeds and looping as desired)
        var idleAnimation = new Animation(knightSheet, idleFrames, 0.15f, true);
        var walkAnimation = new Animation(knightSheet, walkFrames, 0.10f, true);
        var runAnimation = new Animation(knightSheet, runFrames, 0.10f, true);
        var jumpAnimation = new Animation(knightSheet, jumpFrames, 0.12f, false);
        var attack1Animation = new Animation(knightSheet, attack1Frames, 0.10f, false);
        var attack2Animation = new Animation(knightSheet, attack2Frames, 0.10f, false);
        var attack3Animation = new Animation(knightSheet, attack3Frames, 0.10f, false);
        var deadAnimation = new Animation(knightSheet, deadFrames, 0.20f, false);
        var defendAnimation = new Animation(knightSheet, defendFrames, 0.15f, true);

        // Store animations in a dictionary
        var animations = new Dictionary<string, Animation>
        {
            { "Idle",    idleAnimation },
            { "Walk",    walkAnimation },
            { "Run",     runAnimation },
            { "Jump",    jumpAnimation },
            { "Attack1", attack1Animation },
            { "Attack2", attack2Animation },
            { "Attack3", attack3Animation },
            { "Dead",    deadAnimation },
            { "Defend",  defendAnimation }
        };
        player = new Player(animations)
        {
            Name = "Player",
            Viewport = new Rectangle(5, 0, 43, 64),
            Position = new Vector2(100, Singleton.SCREENHEIGHT - _background.Height - 64),
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
        _gameObjects.Add(player);

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }
}
