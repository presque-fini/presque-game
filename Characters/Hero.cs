using System.Collections.Generic;
using game.Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.DeferredLighting;
using Nez.Sprites;

namespace game.Characters
{
    public class Hero : Component, IUpdatable
    {
        private string animation;
        private SpriteAnimator animator;
        private BoxCollider collider;
        private SpotLight flashLight;

        private VirtualButton inputFlashlight;
        private VirtualButton inputInteract;
        private VirtualButton inputRun;
        private VirtualIntegerAxis inputXAxis;
        private List<Entity> interactiveEntitiesList;
        private Vector2 lightLeftOffset;
        private Vector2 lightRightOffset;
        private Mover mover;
        private Vector2 velocity;

        public float Gravity { get; set; } = 50;
        public float JumpHeight { get; set; } = 0.2f;
        public float MoveSpeed { get; set; } = 100;

        void IUpdatable.Update()
        {
            var deltaMovement = Vector2.Zero;
            var moveDir = new Vector2(inputXAxis.Value, 0);

            velocity.Y += Gravity * Time.DeltaTime;
            deltaMovement.Y = velocity.Y;
            MoveSpeed = 100;
            animator.Speed = 1;

            // Light toggle
            if (flashLight.Enabled && inputFlashlight.IsPressed)
                flashLight.SetEnabled(false);
            else if (!flashLight.Enabled && inputFlashlight.IsPressed)
                flashLight.SetEnabled(true);

            // Interact with entities
            foreach (var interactiveEntity in interactiveEntitiesList)
                if (inputInteract.IsPressed && Entity.GetComponent<Collider>()
                    .Overlaps(interactiveEntity.GetComponent<Collider>()))
                {
                    if (interactiveEntity.Tag == (int) Layers.Tag.Active)
                        interactiveEntity.SetTag((int) Layers.Tag.Inactive);
                    else
                        interactiveEntity.SetTag((int) Layers.Tag.Active);
                }

            // Alternate animation setup
            if (animator.CurrentAnimationName == "john.idle" && animator.CurrentFrame == 0 && Random.Chance(10))
                animation = "john.idle.alternate";

            if (animator.CurrentAnimationName == "john.idle.alternate" && animator.CurrentFrame == 71)
                animation = "john.idle";

            // Recover from walk or run
            if (animator.CurrentAnimationName != "john.idle" && animator.CurrentAnimationName != "john.idle.alternate")
                animation = "john.idle";

            if (collider.CollidesWithAny(ref deltaMovement, out var collisionResult) && collisionResult.Normal.Y < 0)
            {
                // reset velocity to prevent movement without user input
                velocity = Vector2.Zero;
                if (moveDir.X < 0)
                {
                    animator.FlipY = true;
                    flashLight.Transform.SetRotationDegrees(180);
                    flashLight.SetLocalOffset(lightRightOffset);
                    animation = "john.walk";
                    if (inputRun.IsDown)
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
                    if (inputRun.IsDown)
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
                animator.Play(animation);
        }

        public override void OnAddedToEntity()
        {
            lightRightOffset = new Vector2(-6f, -35f);
            lightLeftOffset = new Vector2(6f, -35f);

            var heroAtlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");

            collider = new BoxCollider {CollidesWithLayers = (int) Layers.PhysicsLayer.Player};
            mover = new Mover();
            animator = new SpriteAnimator();
            animator.AddAnimationsFromAtlas(heroAtlas);
            animator.RenderLayer = (int) Layers.RenderLayer.Player;

            Entity.AddComponent(collider);
            Entity.AddComponent(mover);
            Entity.AddComponent(animator);

            animation = "john.idle";

            SetupInput();
            SetupFlashLight();

            interactiveEntitiesList = Entity.Scene.FindEntitiesWithTag((int) Layers.Tag.Interactive);
        }

        private void SetupFlashLight()
        {
            flashLight = new SpotLight(Color.White);
            flashLight.SetConeAngle(90);
            flashLight.SetIntensity(2f);
            flashLight.SetLocalOffset(lightRightOffset);
            flashLight.SetRenderLayer((int) Layers.RenderLayer.Light);
            Entity.AddComponent(flashLight);
        }

        private void SetupInput()
        {
            inputFlashlight = new VirtualButton();
            inputFlashlight.Nodes.Add(new VirtualButton.KeyboardKey(Keys.T));

            inputInteract = new VirtualButton();
            inputInteract.Nodes.Add(new VirtualButton.KeyboardKey(Keys.A));

            inputRun = new VirtualButton();
            inputRun.Nodes.Add(new VirtualButton.KeyboardKey(Keys.LeftShift));

            inputXAxis = new VirtualIntegerAxis();
            inputXAxis.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            inputXAxis.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            inputXAxis.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left,
                Keys.Right));
        }
    }
}