using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PresidentEvil
{
    public class GameObject : ICloneable
    {
        protected Texture2D _texture;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;
        public Vector2 Velocity;
        public string Name; // Check Collision
        public bool IsActive; // Check is show in screen
        public Rectangle Viewport; // View of the object

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height); }
        }

        public GameObject()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }
       public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Reset()
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}