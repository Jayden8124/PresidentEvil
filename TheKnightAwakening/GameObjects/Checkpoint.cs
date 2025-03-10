using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheKnightAwakening
{
    public class Checkpoint : GameObject
    {
        public bool Activated { get; private set; } = false;
        public Texture2D Texture { get; private set; }

        public Checkpoint(Texture2D texture, Vector2 position, Rectangle viewport)
        {
            Texture = texture;
            Position = position;
            Viewport = viewport;
        }

        // เมื่อ Checkpoint ถูกเปิดใช้งาน
        public void Activate()
        {
            Activated = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // ถ้า Activated ให้เปลี่ยนสี (เช่น สี Yellow) เพื่อบ่งบอกว่าได้ถูกเปิดใช้งานแล้ว
            Color color = Activated ? Color.Yellow : Color.White;
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height), color);
        }
    }
}
