using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class Player : GameObject
    {
        public Bullet Bullet;
        public Keys Left, Right, Up, Down, Fire;

        // Animation
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;
        private bool isJumping = false;
        private float jumpVelocity = -10f;
        private float gravity = 0.5f;
        private float velocityY = 0;
        private bool facingRight = true;
        private bool isAttacking = false;
        private float attackTimer = 0f;

        public Player(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Idle"]);
            IsActive = true;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            var velocity = Vector2.Zero;
            bool isRunning = Singleton.Instance.CurrentKey.IsKeyDown(Keys.LeftShift);

            float speed = isRunning ? 8f : 3f; // Normal speed = 3, Running speed = 8

            if (Singleton.Instance.CurrentKey.IsKeyDown(Left))
            {
                velocity.X -= speed;
                facingRight = false;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Right))
            {
                velocity.X += speed;
                facingRight = true;
            }

            // Jump logic
            if (Singleton.Instance.CurrentKey.IsKeyDown(Up) && !isJumping)
            {
                isJumping = true;
                velocityY = jumpVelocity;
                AnimationManager.Play(Animations["Jump"]);
            }

            if (isJumping)
            {
                velocityY += gravity;
                Position.Y = MathHelper.Clamp(Position.Y + velocityY, 0, Singleton.SCREENHEIGHT - 110);

                isJumping = Position.Y < Singleton.SCREENHEIGHT - 110; // Reset jumping flag when landing
            }


            // Fire bullet, but only if not attacking
            if (!isAttacking && Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
                Singleton.Instance.PreviousKey.IsKeyUp(Fire))
            {
                var newBullet = Bullet.Clone() as Bullet;
                newBullet.Position = Position;
                newBullet.Reset();
                _gameObjects.Add(newBullet);

                // Play Attack Animation and Prevent Further Attacks Until It Finishes
                AnimationManager.Play(Animations["Attack1"]);
                isAttacking = true;
                attackTimer = 0f;
            }

            // Ensure Attack Animation Plays Fully Before Allowing Another Attack
            if (isAttacking)
            {
                attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (attackTimer >= Animations["Attack1"].FrameSpeed * Animations["Attack1"].FrameCount)
                {
                    isAttacking = false;
                }
            }

            // Prevent movement animations from overriding attack animation
            if (!isAttacking && !isJumping)
            {
                if (velocity != Vector2.Zero)
                {
                    AnimationManager.Play(isRunning ? Animations["Run"] : Animations["Walk"]);
                }
                else
                {
                    AnimationManager.Play(Animations["Idle"]);
                }
            }

            // Flip animation if changing direction
            AnimationManager.FacingRight = facingRight;

            // Update position
            Position += velocity;

            // Clamp player's position
            // Position.X = MathHelper.Clamp(Position.X, 0, Singleton.SCREENWIDTH - 32);

            // Update animation
            AnimationManager.Update(gameTime);

            base.Update(gameTime, _gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
        }
    }
}
