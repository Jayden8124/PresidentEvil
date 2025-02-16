using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class Player : GameObject
    {
        public Bullet Bullet;
        public Keys Left,Right,Up,Down,Fire;
    
        public Player(Texture2D texture) : base(texture)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if(Singleton.Instance.CurrentKey.IsKeyDown(Left))
            {
                Position.X -= 5;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Right))
            {
                Position.X += 5;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Up))
            {
                Position.Y -= 5;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Down))
            {
                Position.Y += 5;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Fire) && Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey) 
            {
                var newBullet = Bullet.Clone() as Bullet;
                // newBullet.Position = new Vector2();
                newBullet.Reset();
                _gameObjects.Add(newBullet);
            }

            // newX = MathHelper.Clamp(Position.X, 0, Singleton.SCREENWIDTH - Viewport.Width);
            base.Update(gameTime, _gameObjects);
        }
    }
}
