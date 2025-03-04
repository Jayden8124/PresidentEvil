using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class Player : GameObject
    {
        // Bullet Object of Player
        public Bullet Bullet;

        // Properties Status
        public int HealthPoint;

        // Movement
        public Keys Left, Right, Up, Down, Fire, Defend;

        // Animation
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;

        // Properties
        private bool isJumping = false;
        private float jumpVelocity = -10f;
        private float gravity = 0.5f;
        private bool facingRight = true;
        private bool isAttacking = false;
        private bool isDefending = false;
        private float attackTimer = 0f;
        private bool isDead = false;

        // Properties on ground
        public Player(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Idle"]);
            IsActive = true;
        }

        public override void Reset()
        {
            HealthPoint = 100;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (isDead)
            {
                AnimationManager.Update(gameTime);
                return;
            }

            var velocity = Vector2.Zero;
            bool isRunning = Singleton.Instance.CurrentKey.IsKeyDown(Keys.LeftShift);
            float speed = isRunning ? 8f : 3f; // Normal = 3, Run = 8
            isDefending = false;
            isJumping = false;

            // isDefending = Singleton.Instance.CurrentMouse.RightButton == ButtonState.Pressed;
            if (Singleton.Instance.CurrentKey.IsKeyDown(Defend))
            {
                AnimationManager.Play(Animations["Defend"]);
                isDefending = true;
            }
            if (!isDefending)
            {
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

                // กด Up เพื่อกระโดด เมื่ออยู่บนพื้น
                if (Singleton.Instance.CurrentKey.IsKeyDown(Up) && OnGround)
                {
                    isJumping = true;
                    OnGround = false;
                    Velocity.Y = jumpVelocity;
                    AnimationManager.Play(Animations["Jump"]);
                }

                // ถ้าอยู่ในสถานะกระโดดหรือกำลังตก (ไม่ติดพื้น) ให้อัพเดทแรงโน้มถ่วง
                if (!OnGround)
                {
                    Velocity.Y += gravity;
                    Position.Y += Velocity.Y;

                    // // ถ้าตำแหน่งเกินพื้นที่พื้น (สมมุติว่า Singleton.SCREENHEIGHT - 110 คือระดับพื้น)
                    // if (Position.Y >= Singleton.SCREENHEIGHT - 110)
                    // {
                    //     Position.Y = Singleton.SCREENHEIGHT - 110;
                    //     OnGround = true;
                    //     isJumping = false;
                    //     VerticalVelocity = 0f;
                    // }
                }

                // Fire bullet, but only if not attacking
                if (!isAttacking && Singleton.Instance.CurrentKey.IsKeyDown(Fire) && 
                !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    // var newBullet = Bullet.Clone() as Bullet;
                    // newBullet.Position = Position;
                    // newBullet.Reset();
                    // _gameObjects.Add(newBullet);

                    // Play Attack Animation and Prevent Further Attacks Until It Finishes
                    AnimationManager.Play(Animations["Attack1"]);
                    isAttacking = true;
                    attackTimer = 0f;
                }
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

            // อัพเดทการเคลื่อนที่ในแนว X (ไม่เกี่ยวกับแรงโน้มถ่วง)
            Position += new Vector2(velocity.X, 0);

            // เลือก Animation สำหรับการเดินหรือวิ่งถ้าไม่อยู่ในสถานะกระโดด
            if (OnGround && !isDefending && !isAttacking && !isJumping)
            {
                if (velocity.X != 0)
                {
                    AnimationManager.Play(isRunning ? Animations["Run"] : Animations["Walk"]);
                }
                else
                {
                    AnimationManager.Play(Animations["Idle"]);
                }
            }

            // ปรับการหันหน้าให้ถูกต้อง (ตัวอย่างนี้สามารถปรับปรุงเพิ่มเติมได้)
            AnimationManager.FacingRight = facingRight;

            AnimationManager.Update(gameTime);
            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
            base.Update(gameTime, _gameObjects);
        }

        public void TakeDamage(int damage)
        {
            HealthPoint -= isDefending ? 0 : damage;
            if (HealthPoint <= 0)
            {
                HealthPoint = 0;
                isDead = true;
                AnimationManager.Play(Animations["Dead"]);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Position = Position;
            AnimationManager.Draw(spriteBatch);
        }
    }
}