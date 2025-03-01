using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PresidentEvil;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    SpriteFont _font;
    List<GameObject> _gameObjects;
    public int _numOjects;
    private Camera _camera;
    private Map _map;

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
        _map = new Map(GraphicsDevice); // Initialize map

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("game_font");
        _map.LoadContent(Content);

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

        _map.Draw(_spriteBatch);

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

        _gameObjects.Clear();

        // Load Texture Player
        Texture2D knightSheet = Content.Load<Texture2D>("player");

        // Load Texture Monster
        var monsterTextures = new Dictionary<AnimationMonster.AnimationMonsterType, Texture2D>
        {
            { AnimationMonster.AnimationMonsterType.SKLT_WR, Content.Load<Texture2D>("skeleton_warrior") },
            { AnimationMonster.AnimationMonsterType.SKLT_SM, Content.Load<Texture2D>("skeleton_spearman") },
            { AnimationMonster.AnimationMonsterType.SKLT_AC, Content.Load<Texture2D>("skeleton_archer") },
            { AnimationMonster.AnimationMonsterType.SL, Content.Load<Texture2D>("blue_slime") }
        };

        // Player Instance
        var _animationsPlayer = AnimationPlayer.LoadAnimations(knightSheet);

        // Monster Instance
        var _animationMonster = new AnimationMonster();
        _animationMonster.LoadAllAnimations(monsterTextures);

        player = new Player(_animationsPlayer)
        {
            Name = "Player",
            Viewport = new Rectangle(5, 0, 43, 64),
            Position = new Vector2(100, Singleton.SCREENHEIGHT - 300),
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

        // Add GameObjects
        _gameObjects.Add(player);
        _gameObjects.Add(monster);
        _gameObjects.Add(monster1);

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }
}