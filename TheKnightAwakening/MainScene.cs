using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheKnightAwakening;

public class MainScene : Game
{
    // Graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Font
    SpriteFont _font;

    // GameObjects
    List<GameObject> _gameObjects;
    public int _numOjects;

    // Camera & Map
    private Camera _camera;
    private Map _map;

    // Textures
    private Texture2D _background;
    // Cutscene
    private CutScene _cutscene;


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
        _cutscene = new CutScene(GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("bg");
        _font = Content.Load<SpriteFont>("game_font");
        _map.LoadContent(Content);
        _cutscene.LoadContent(Content);

        Singleton.Instance.HitblockTiles = _map.GetCollisionRectangles();

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
                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;
                }
                break;
            case Singleton.GameState.Cutscene:
                _cutscene.Update(gameTime);
                break;
            case Singleton.GameState.GamePlaying:
                // Mouse State
                // if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed &&
                //     Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                // {
                //     Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
                // }

                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;

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

                // ตรวจสอบและแก้ไข collision สำหรับ Monster แต่ละตัว for Collision   
                foreach (var obj in _gameObjects)
                {
                    CollisionManager.ResolveCollision(obj, Singleton.Instance.HitblockTiles);
                    CollisionManager.UpdateOnGround(obj, Singleton.Instance.HitblockTiles);
                }

                // หลังจากที่ทำ Collision กับ tile map แล้ว
                foreach (var obj in _gameObjects)
                {
                    if (obj is MonsterType monster)
                    {
                        if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, monster.Rectangle))
                        {
                            CollisionManager.ResolveCharacterCollision(Singleton.Instance.player, monster);

                            if (!(monster is SKLT_WR))
                            {
                                Singleton.Instance.player.TakeDamage(1, obj.Position);
                            }
                        }
                    }
                }

                if (Singleton.Instance.player.Position.Y > 50000)
                {
                    // Reset CheckPoint
                }
                break;

            case Singleton.GameState.GamePaused: //Game Paused
                if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                break;
            case Singleton.GameState.GameOver:
                if (Singleton.Instance.CurrentMouse.LeftButton == ButtonState.Pressed && Singleton.Instance.PreviousMouse.LeftButton == ButtonState.Released)
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.Start;
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                }
                break;
        }

        Singleton.Instance.PreviousMouse = Singleton.Instance.CurrentMouse;
        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.Start:
                {
                    _DrawStart();
                }
                break;
            case Singleton.GameState.Cutscene:
                {
                    _cutscene.Draw(_spriteBatch);
                }
                break;
            case Singleton.GameState.GamePlaying:
                {
                    _DrawPlaying();
                }
                break;
            case Singleton.GameState.GamePaused:
                {
                    _DrawPause();
                }
                break;
            case Singleton.GameState.GameOver:
                {
                    _DrawOver();
                }
                break;
        }

        _graphics.BeginDraw();

        base.Draw(gameTime);
    }

    public void _DrawStart()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Button UI
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Code Button

        _spriteBatch.End();
    }
    public void _DrawPause()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Button UI Pause 
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Code Button (Left Click Press)

        _spriteBatch.End();
    }

    public void _DrawPlaying()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Map & Camera
        _spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);

        _map.Draw(_spriteBatch);

        for (int i = 0; i < _numOjects; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }

        _spriteBatch.End();

        // Layer 3: UI
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Setting Menu Button 
        // ResetUI();

        _spriteBatch.End();
    }
    public void _DrawOver()
    {
        // Layer 1: Background
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT), Color.White);

        _spriteBatch.End();

        // Layer 2: Button To Exit
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Code Button To Exit to Main Menu or Restart

        _spriteBatch.End();
    }

    protected void Reset()
    {
        Singleton.Instance.Score = 0;
        Singleton.Instance.Timer = 0;
        Singleton.Instance.CurrentGameState = Singleton.GameState.Cutscene;

        _gameObjects.Clear();

        ResetPlayer(); // Call PLayer Object
        ResetMonster(); // Call Monster Object
        ResetObject(); // Call Object

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    // public void ResetUI() // Can into the player class
    // {
    //     var healthAnimations = AnimationHealth.LoadAnimations(Content.Load<Texture2D>("health_bar_red"));

    //     // สมมุติว่า HP max คือ 100
    //     int health = Singleton.Instance.player.Health;
    //     Animation hpAnimation;

    //     if (health == 4)
    //         hpAnimation = healthAnimations["HP100"];
    //     else if (health == 3)
    //         hpAnimation = healthAnimations["HP75"];
    //     else if (health == 2)
    //         hpAnimation = healthAnimations["HP50"];
    //     else if (health == 1)
    //         hpAnimation = healthAnimations["HP25"];
    //     else
    //         hpAnimation = healthAnimations["HP0"];

    //     AnimationManager hpAnimationManager = new AnimationManager(hpAnimation);
    //     hpAnimationManager.Position = new Vector2(57, 41);
    //     hpAnimationManager.Scale = 2f;
    //     hpAnimationManager.Draw(_spriteBatch);

    //     var ultAnimations = AnimationUltimateCharge.LoadAnimations(Content.Load<Texture2D>("ult_charge"));
    //     Animation _ultAnimation;

    //     int ult = Singleton.Instance.player.Ultimate;

    //     if (ult >= 4)
    //         _ultAnimation = ultAnimations["ULT1"];
    //     else if (ult == 3)
    //         _ultAnimation = ultAnimations["ULT2"];
    //     else if (ult == 2)
    //         _ultAnimation = ultAnimations["ULT3"];
    //     else
    //         _ultAnimation = ultAnimations["ULT4"];

    //     AnimationManager ultAnimationManager = new AnimationManager(_ultAnimation);
    //     ultAnimationManager.Position = new Vector2(287, 35);
    //     ultAnimationManager.Scale = 1.5f;
    //     ultAnimationManager.Draw(_spriteBatch);
    // }

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
            Position = new Vector2(12000, 100),
            Left = Keys.Left,
            Right = Keys.Right,
            Up = Keys.Up,
            Down = Keys.Down,
            Fire = Keys.Space,
            Defend = Keys.LeftControl,
            Attack2 = Keys.F,
            Attack3 = Keys.G,
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
            { AnimationMonster.AnimationMonsterType.SL, Content.Load<Texture2D>("blue_slime") },
            { AnimationMonster.AnimationMonsterType.MDS, Content.Load<Texture2D>("medusa") }
        };

        // Monster Instance
        var _animationMonster = new AnimationMonster();
        _animationMonster.LoadAllAnimations(monsterTextures);

        MonsterType monster = new SKLT_WR(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_WR))
        {
            Name = "SKLT1",
            Viewport = new Rectangle(0, 0, 36, 65),
            Position = new Vector2(12200, 100)
        };

        MonsterType monster1 = new SL(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SL))
        {
            Name = "SL1",
            Viewport = new Rectangle(0, 0, 36, 35),
            Position = new Vector2(12300, 100)
        };

        MonsterType monster2 = new SKLT_SM(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_SM))
        {
            Name = "SKLT2",
            Viewport = new Rectangle(0, 0, 36, 90),
            Position = new Vector2(12400, 100)
        };

        MonsterType monster3 = new SKLT_AC(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.SKLT_AC))
        {
            Name = "SKLT3",
            Viewport = new Rectangle(0, 0, 36, 65),
            Position = new Vector2(12500, 100),
            Bullet = new Bullet(Content.Load<Texture2D>("skeleton_archer"))
            {
                Name = "BulletEnemy",
                Viewport = new Rectangle(384, 65, 45, 3),
                Velocity = new Vector2(0, 600f)
            }
        };

        MonsterType boss = new MDS(_animationMonster.GetAnimations(AnimationMonster.AnimationMonsterType.MDS))
        {
            Name = "MDS1",
            Viewport = new Rectangle(0, 0, 65, 85),
            Position = new Vector2(12000, 100)
        };

        // _gameObjects.Add(monster);
        // _gameObjects.Add(monster1);
        // _gameObjects.Add(monster2);
        _gameObjects.Add(monster3);
        // _gameObjects.Add(boss);
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
            Position = new Vector2(300, 800)
        };

        // Add GameObjects
        _gameObjects.Add(chest);
    }
}