using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.DeferredLighting;
using Nez.Sprites;

namespace game
{
    public class Hero : Component, IUpdatable
    {
        private readonly int lightLayer;
        private readonly int renderLayer;
        private string animation;
        private SpriteAnimator animator;
        private BoxCollider collider;
        private SpotLight flashLight;
        private VirtualButton inputFlashlight;
        private VirtualButton inputJump;
        private VirtualIntegerAxis inputXAxis;
        private Vector2 lightLeftOffset;
        private Vector2 lightRightOffset;
        private Mover mover;
        private Vector2 velocity;

        public Hero(int renderLayer, int lightLayer)
        {
            this.renderLayer = renderLayer;
            this.lightLayer = lightLayer;
        }

        public float Gravity { get; set; } = 500;
        public float JumpHeight { get; set; } = 0.2f;
        public float MoveSpeed { get; set; } = 100;

        public override void OnAddedToEntity()
        {
            lightRightOffset = new Vector2(-6f, -35f);
            lightLeftOffset = new Vector2(6f, -35f);

            SpriteAtlas heroAtlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");

            collider = Entity.AddComponent<BoxCollider>();
            mover = Entity.AddComponent<Mover>();
            animator = Entity.AddComponent<SpriteAnimator>().AddAnimationsFromAtlas(heroAtlas);
            animator.RenderLayer = renderLayer;

            animation = "john.idle";

            SetupInput();
            SetupFlashLight();
        }

        private void SetupFlashLight()
        {
            flashLight = new SpotLight(Color.White);
            flashLight.SetConeAngle(90);
            flashLight.SetIntensity(2f);
            flashLight.SetLocalOffset(lightRightOffset);
            flashLight.SetRenderLayer(lightLayer);
            Entity.AddComponent(flashLight);
        }

        private void SetupInput()
        {
            inputFlashlight = new VirtualButton();
            inputFlashlight.Nodes.Add(new VirtualButton.KeyboardKey(Keys.T));

            inputJump = new VirtualButton();
            inputJump.Nodes.Add(new VirtualButton.KeyboardKey(Keys.LeftShift));

            inputXAxis = new VirtualIntegerAxis();
            inputXAxis.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            inputXAxis.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            inputXAxis.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
        }

        void IUpdatable.Update()
        {
            CollisionResult collisionResult;
            Vector2 deltaMovement = new Vector2(0);
            Vector2 moveDir = new Vector2(inputXAxis.Value, 0);

            velocity.Y += 50 * Time.DeltaTime;
            deltaMovement.Y = velocity.Y;
            MoveSpeed = 100;
            animator.Speed = 1;

            // Light toggle
            if (flashLight.Enabled && inputFlashlight.IsPressed)
            {
                flashLight.SetEnabled(false);
            }
            else if (!flashLight.Enabled && inputFlashlight.IsPressed)
            {
                flashLight.SetEnabled(true);
            }

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
                    animator.FlipY = true;
                    flashLight.Transform.SetRotationDegrees(180);
                    flashLight.SetLocalOffset(lightRightOffset);
                    animation = "john.walk";
                    if (inputJump.IsDown)
                    {
                        animation = "john.footing";
                        MoveSpeed = 400;
                        animator.Speed = 1.5f;
                    }
                    velocity.X = -MoveSpeed;
                }
                else if (moveDir.X > 0)
                {
                    animator.FlipY = false;
                    flashLight.Transform.SetRotationDegrees(0);
                    flashLight.SetLocalOffset(lightLeftOffset);
                    animation = "john.walk";
                    if (inputJump.IsDown)
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