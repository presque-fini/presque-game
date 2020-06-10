using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace game
{
    public class Hero : Component, IUpdatable
    {
        public float Gravity { get; set; } = 500;
        public float JumpHeight { get; set; } = 0.2f;
        public float MoveSpeed { get; set; } = 100;

        private SpriteAnimator animator;
        private BoxCollider collider;
        private Mover mover;
        private Vector2 velocity;
        private VirtualIntegerAxis xAxisInput;
        private VirtualButton runInput;
        private readonly int renderLayer;
        private string animation;

        public Hero(int renderLayer)
        {
            this.renderLayer = renderLayer;
        }

        public override void OnAddedToEntity()
        {
            SpriteAtlas heroAtlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");

            collider = Entity.AddComponent<BoxCollider>();
            mover = Entity.AddComponent<Mover>();
            animator = Entity.AddComponent<SpriteAnimator>().AddAnimationsFromAtlas(heroAtlas);
            animator.RenderLayer = renderLayer;

            animation = "john.idle";

            SetupInput();
        }

        private void SetupInput()
        {
            xAxisInput = new VirtualIntegerAxis();
            xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

            runInput = new VirtualButton();
            runInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.LeftShift));
        }

        void IUpdatable.Update()
        {
            CollisionResult collisionResult;
            Vector2 deltaMovement = new Vector2(0);
            Vector2 moveDir = new Vector2(xAxisInput.Value, 0);

            velocity.Y += 50 * Time.DeltaTime;
            deltaMovement.Y = velocity.Y;
            MoveSpeed = 100;
            animator.Speed = 1;

            // Alternate animation setup
            if (animator.CurrentAnimationName == "john.idle" && animator.CurrentFrame == 0 && Random.Chance(10))
            {
                animation = "john.idle.alternate";
            }
            if (animator.CurrentAnimationName == "john.idle.alternate" && animator.CurrentFrame == 71)
            { animation = "john.idle"; }

            // Recover from walk or run
            if (animator.CurrentAnimationName != "john.idle" && animator.CurrentAnimationName != "john.idle.alternate")
            {
                animation = "john.idle";
            }

            if (collider.CollidesWithAny(ref deltaMovement, out collisionResult) && collisionResult.Normal.Y < 0)
            {
                // reset velocity to prevent movement without user input
                velocity = Vector2.Zero;
                if (moveDir.X < 0)
                {
                    animator.FlipX = true;
                    animation = "john.walk";
                    if (runInput.IsDown)
                    {
                        animation = "john.footing";
                        MoveSpeed = 400;
                        animator.Speed = 1.5f;
                    }
                    velocity.X = -MoveSpeed;
                }
                else if (moveDir.X > 0)
                {
                    animator.FlipX = false;
                    animation = "john.walk";
                    if (runInput.IsDown)
                    {
                        animation = "john.footing";
                        MoveSpeed = 400;
                        animator.Speed = 1.5f;
                    }
                    velocity.X = MoveSpeed;
                }
            }

            // move the Entity to the new position. deltaMovement is already adjusted to resolve collisions for us.
            deltaMovement += velocity * Time.DeltaTime;
            mover.Move(deltaMovement, out collisionResult);
            if (!animator.IsAnimationActive(animation))
            {
                animator.Play(animation);
            }
        }
    }
}