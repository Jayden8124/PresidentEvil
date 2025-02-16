using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class Bullet : GameObject
    {
        public float DistanceMoved;
        public Bullet(Texture2D texture) : base(texture)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            DistanceMoved = 0;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            DistanceMoved += Math.Abs(Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond);
            Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (DistanceMoved > 2000)
            {
                IsActive = false;
            }
            base.Update(gameTime, _gameObjects);
        }
    }
}
