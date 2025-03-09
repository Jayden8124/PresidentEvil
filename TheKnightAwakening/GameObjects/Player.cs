using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Player : GameObject
    {
        // Bullet Object of Player
        public Bullet Bullet;

        // Animation
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;

        // Properties Status
        public int Ultimate { get; set; }
        // Movement
        public Keys Left, Right, Up, Down, Fire, Defend, Attack2, Attack3;

        // Properties
        private bool isAttacking = false;
        private bool isDefending = false;
        private float attackTimer = 0f;
        private bool isInvincible = false;
        private float invincibleTimer = 0f;
        private float blinkTimer = 0f;
        private bool isVisible = true;

        // Properties on ground
        public Player(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Idle"]);
            IsActive = true;
        }

        public override void Reset()
        {
            Health = 9999;
            Ultimate = 0;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> _gameObjects)
        {
            if (isDead)
            {
                AnimationManager.Update(gameTime);
                return;
            }
            if (isInvincible)
            {
                invincibleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                blinkTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (blinkTimer <= 0f)
                {
                    isVisible = !isVisible; // สลับการมองเห็น
                    blinkTimer = 0.1f; // กระพริบทุก 0.1 วิ
                }

                if (invincibleTimer <= 0f)
                {
                    isInvincible = false;
                    isVisible = true; // กลับมาแสดงผลปกติ
                }
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
                    AnimationManager.FacingRight = false;
                }
                if (Singleton.Instance.CurrentKey.IsKeyDown(Right))
                {
                    velocity.X += speed;
                    AnimationManager.FacingRight = true;
                }

                // กด Up เพื่อกระโดด เมื่ออยู่บนพื้น
                if (Singleton.Instance.CurrentKey.IsKeyDown(Up) && OnGround &&
                !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                {
                    isJumping = true;
                    OnGround = false;
                    Velocity.Y = -15;
                    AnimationManager.Play(Animations["Jump"]);
                }

                // ถ้าอยู่ในสถานะกระโดดหรือกำลังตก (ไม่ติดพื้น) ให้อัพเดทแรงโน้มถ่วง
                if (!OnGround)
                {
                    Velocity.Y += Gravity;
                    Position.Y += Velocity.Y;
                }

                // Fire bullet, but only if not attacking
                if (!isAttacking)
                {
                    if (Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
                        !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    {
                        AnimationManager.Play(Animations["Attack1"]);
                        isAttacking = true;
                        attackTimer = 0f;
                        Ultimate += 1;
                    }
                    else if (Singleton.Instance.CurrentKey.IsKeyDown(Attack2) &&
                        !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    {
                        AnimationManager.Play(Animations["Attack2"]);
                        isAttacking = true;
                        attackTimer = 0f;
                        Ultimate = 0;
                    }
                    else if (Singleton.Instance.CurrentKey.IsKeyDown(Attack3) &&
                        !Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey))
                    {
                        AnimationManager.Play(Animations["Attack3"]);
                        isAttacking = true;
                        attackTimer = 0f;
                    }
                }
                // Ensure Attack Animation Plays Fully Before Allowing Another Attack
                if (isAttacking)
                {
                    attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    foreach (var obj in _gameObjects)
                    {
                        if (obj is MonsterType monster)
                        {
                            if (GameObject.CheckAABBCollision(Rectangle, monster.Rectangle)) // Check if player is colliding with monster
                            {
                                // Call TakeDamage method on monster if colliding with attack
                                monster.TakeDamage(50, Position); // You can adjust the damage value as needed
                            }
                        }
                    }
                    if (attackTimer >= Animations["Attack1"].FrameSpeed * Animations["Attack1"].FrameCount)
                    {
                        isAttacking = false;
                    }
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
            AnimationManager.FacingRight = AnimationManager.FacingRight;

            AnimationManager.Update(gameTime);
            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
            base.Update(gameTime, _gameObjects);
        }

        public void TakeDamage(int damage, Vector2 enemyPosition)
        {
            if (isInvincible) return;

            // เช็คว่าหันหน้าถูกด้านหรือไม่
            bool isFacingEnemy = (enemyPosition.X > Position.X && AnimationManager.FacingRight) ||
                                 (enemyPosition.X < Position.X && !AnimationManager.FacingRight);

            // ถ้ากันแต่หันผิดด้าน -> โดนดาเมจ
            Health -= (isDefending && !isFacingEnemy) ? damage : (isDefending ? 0 : damage);

            if (Health <= 0)
            {
                Health = 0;
                isDead = true;
                AnimationManager.Play(Animations["Dead"]);
            }
            else
            {
                isInvincible = true;
                invincibleTimer = 1f; // อมตะ 2 วิ
                blinkTimer = 0.1f; // ความเร็วกระพริบ
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                AnimationManager.Position = Position;
                AnimationManager.Draw(spriteBatch);
            }
        }
    }
}