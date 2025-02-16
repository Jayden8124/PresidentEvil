using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class MonsterType : GameObject
    {
        public MonsterType(Texture2D texture) : base(texture)
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
            base.Update(gameTime, _gameObjects);
        }
    }
}
