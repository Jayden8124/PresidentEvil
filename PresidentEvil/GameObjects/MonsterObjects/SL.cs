using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class SL : MonsterType
    {
        public SL(Texture2D texture) : base(texture)
        {

        }

        public SL(Dictionary<string, Animation> animations) : base(animations)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
        //     if (gameTime.TotalGameTime.TotalSeconds == 3)
        // {
        //     Position = new Vector2(Position.X + 1f, Position.Y);
        //     AnimationManager.Play(Animations["Walk"]);
        // } else if (gameTime.TotalGameTime.TotalSeconds >= 10){
        //     Position = new Vector2(Position.X - 1f, Position.Y);
        //     AnimationManager.Play(Animations["Attack"]);
        // }
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