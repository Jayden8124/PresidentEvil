using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class SKLT_AC : MonsterType
    {
        public SKLT_AC(Texture2D texture) : base(texture)
        {

        }

         public SKLT_AC(Dictionary<string, Animation> animations) : base(animations)
        {
            
        }
        
         public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}