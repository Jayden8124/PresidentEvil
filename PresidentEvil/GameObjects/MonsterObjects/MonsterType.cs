using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PresidentEvil
{
    public class MonsterType : GameObject
    {
        // Animation
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;

        // Monster Status
        protected int HealthPoint;
        protected int Damage;
        protected bool facingLeft = true;
        protected bool isAttacking = false;
        protected bool isDead = false;
        public float DistanceMoved;

         // Property ใหม่ สำหรับเช็ค OnGround
    public bool OnGround { get; set; } = false;

        public MonsterType(Dictionary<string, Animation> animations)
        {
            Animations = animations;
            AnimationManager = new AnimationManager(Animations["Idle"]);
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
    }
}

