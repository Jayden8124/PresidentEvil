using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
            if (gameTime.TotalGameTime.TotalSeconds > 3)
    {
        // หากชนกับ Player ให้เล่น Attack อย่างต่อเนื่อง
        if (GameObject.CheckAABBCollision(Singleton.Instance.player.Rectangle, this.Rectangle))
        {
            AnimationManager.Play(Animations["Attack"]);
        }
        else
        {
            float distanceX = Math.Abs(Position.X - Singleton.Instance.player.Position.X);
            if (distanceX <= 150)
            {
                float runSpeed = 2f;
                if (Singleton.Instance.player.Position.X < Position.X)
                {
                    Position = new Vector2(Position.X - runSpeed, Position.Y);
                    AnimationManager.FacingRight = false;
                }
                else
                {
                    Position = new Vector2(Position.X + runSpeed, Position.Y);
                    AnimationManager.FacingRight = true;
                }
                AnimationManager.Play(Animations["Run"]);
            }
            else
            {
                float walkSpeed = 1f;
                if (Singleton.Instance.player.Position.X < Position.X)
                {
                    Position = new Vector2(Position.X - walkSpeed, Position.Y);
                    AnimationManager.FacingRight = false;
                }
                else
                {
                    Position = new Vector2(Position.X + walkSpeed, Position.Y);
                    AnimationManager.FacingRight = true;
                }
                AnimationManager.Play(Animations["Walk"]);
            }
        }
    }
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