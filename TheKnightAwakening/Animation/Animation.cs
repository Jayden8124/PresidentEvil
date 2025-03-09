using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Animation
    {
        public Texture2D Texture { get; private set; }
        
        // List of custom frames defined by their exact coordinates and size
        public List<Rectangle> Frames { get; private set; }
        
        // How long (in seconds) to display each frame
        public float FrameSpeed { get; set; }
        
        // Whether the animation should loop
        public bool IsLooping { get; set; }
        
        // Number of frames in this animation
        public int FrameCount => Frames.Count;

        public Animation(Texture2D texture, List<Rectangle> frames, float frameSpeed, bool isLooping)
        {
            Texture = texture;
            Frames = frames;
            FrameSpeed = frameSpeed;
            IsLooping = isLooping;
        }
    }
}
