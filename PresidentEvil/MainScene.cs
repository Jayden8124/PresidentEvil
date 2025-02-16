using System;
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
    Texture2D _background;
    public int _numOjects;


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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);


        // _font = Content.Load<SpriteFont>("Name");
        // _background = Content.Load<Texture2D>("Name");

        // Reset();
    }

    protected override void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();

        _numOjects = _gameObjects.Count;

        for (int i = 0; i < _numOjects; i++)
        {
            if(_gameObjects[i].IsActive)
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

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

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
        // Texture2D sometexture = Content.Load<Texture2D>("Sprite2");
        _gameObjects.Clear();
        _gameObjects.Add(new Player(Texture){
            Name = "Player",
            Viewport = new Rectangle(0, 0, 50, 50),
            Position = new Vector2(100, 100),
            Left = Keys.Left,
            Right = Keys.Right,
            Up = Keys.Up,
            Down = Keys.Down,
            Fire = Keys.Space,
            Bullet = new Bullet(Texture){
                Name = "BulletPlayer",
                Viewport = new Rectangle(0, 0, 10, 10),
                Velocity = new Vector2(-500f, 0)
            }
        });

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }
}
