using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class SKLT_WR : MonsterType
    {
        public SKLT_WR(Texture2D texture) : base(texture)
        {

        }

        public SKLT_WR(Dictionary<string, Animation> animations) : base(animations)
        {
            
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (gameTime.TotalGameTime.TotalSeconds > 3)
        {
            Position = new Vector2(Position.X - 1f, Position.Y);
            AnimationManager.Play(Animations["Walk"]);
        }
        AnimationManager.FacingRight = false;

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
            DistanceMoved = 0;
            base.Reset();
        }
    }
}