using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Coin : GameObject
    {
        public bool IsOpen;
        public Keys openKey;

        // Animation
        private AnimationManager AnimationManager;
        private Dictionary<string, Animation> Animations;

        public Coin(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Closed"]);
            IsActive = true;
            IsOpen = false;
        }

        public void Toggle()
        {
            if (!IsOpen)
            {
                IsOpen = true;
                AnimationManager.Play(Animations["Opening"]);
            }
            else
            {
                IsOpen = false;
                AnimationManager.Play(Animations["Closing"]);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (CheckAABBCollision(Rectangle, Singleton.Instance.player.Rectangle))
            {
                if (Singleton.Instance.CurrentKey.IsKeyDown(openKey) && !IsOpen)
                {
                    Toggle();
                }
            }

            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }
    }
}