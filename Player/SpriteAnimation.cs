using Nez;
using Nez.Sprites;

namespace game
{
    public class SpriteAnimation : SpriteAnimator
    {
        public SpriteAnimation()
        {
            Animation = "john.idle";
        }

        public string Animation { get; set; }

        public void Idle()
        {
            switch (CurrentAnimationName)
            {
                case "john.idle" when CurrentFrame == 0 && Random.Chance(10):
                    Animation = "john.idle.alternate";
                    break;
                case "john.idle.alternate" when CurrentFrame == 71:
                    Animation = "john.idle";
                    break;
            }

            // Recover from walk or run without interrupting the current idle animation
            if (CurrentAnimationName != "john.idle" && CurrentAnimationName != "john.idle.alternate")
                Animation = "john.idle";
        }
    }
}