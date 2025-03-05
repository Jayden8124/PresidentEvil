using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class MonsterType : GameObject
    {
        // Animation
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;

        // Monster Status
        protected bool facingLeft = true;
        protected bool isAttacking = false;
        protected bool isDead = false;
        protected int Health = 100;
        public float DistanceMoved;

        public MonsterType(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Walk"]);
            IsActive = true;
        }

        public MonsterType(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            float gravityValue = 0.5f;  // กำหนดค่าแรงโน้มถ่วงสำหรับ Monster

            if (!OnGround)
            {
                Velocity.Y += gravityValue;
                Position.Y += Velocity.Y;
            }
            if (Health <= 0)
            {
                Health = 0;
                // isDead = true;
                AnimationManager.Play(Animations["Dead"]);
            }
            AnimationManager.Update(gameTime);
            base.Update(gameTime, _gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw Monster Animation
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public void TakeDamage(int damage, Vector2 enemyPosition)
        {
            // // เช็คว่าหันหน้าถูกด้านหรือไม่
            // bool isFacingEnemy = (enemyPosition.X > Position.X && facingLeft) ||
            //                      (enemyPosition.X < Position.X && !facingLeft);

            // ถ้ากันแต่หันผิดด้าน -> โดนดาเมจ
            Health -= damage;
            Console.WriteLine("Monster Health: " + Health);

        }
    }
}

