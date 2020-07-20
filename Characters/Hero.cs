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
        private float gravity;
        private VirtualButton inputFlashlight;
        private VirtualButton inputInteract;
        private VirtualButton inputRun;
        private VirtualIntegerAxis inputXAxis;
        private List<Entity> interactiveEntitiesList;
        private Vector2 lightLeftOffset;
        private Vector2 lightRightOffset;
        private Mover mover;
        private float runSpeed;
        private Vector2 velocity;
        private float walkSpeed;

        /// <summary>
        ///     This method is called each frame.
        /// </summary>
        public void Update()
        {
            // Light toggle
            if (flashLight.Enabled && inputFlashlight.IsPressed)
                flashLight.SetEnabled(false);
            else if (!flashLight.Enabled && inputFlashlight.IsPressed)
                flashLight.SetEnabled(true);

            IdleAnimation();
            Move();
            Interact(interactiveEntitiesList);

            if (!animator.IsAnimationActive(animation))
                animator.Play(animation);
        }

        /// <summary>
        ///     Loops through a list of entities tagged as Layers.Tag.Interactive. The list is built when OnAddedToEntity() is
        ///     called.
        /// </summary>
        /// <param name="list">The list of entities that the player can interact with.</param>
        private void Interact(List<Entity> list)
        {
            foreach (var entity in list)
                if (inputInteract.IsPressed && Entity.GetComponent<Collider>()
                    .Overlaps(entity.GetComponent<Collider>()))
                {
                    if (entity.Tag == (int) Layers.Tag.Active)
                        entity.SetTag((int) Layers.Tag.Inactive);
                    else
                        entity.SetTag((int) Layers.Tag.Active);
                }
        }

        private void IdleAnimation()
        {
            switch (animator.CurrentAnimationName)
            {
                case "john.idle" when animator.CurrentFrame == 0 && Random.Chance(10):
                    animation = "john.idle.alternate";
                    break;
                case "john.idle.alternate" when animator.CurrentFrame == 71:
                    animation = "john.idle";
                    break;
            }

            // Recover from walk or run without interrupting the current idle animation
            if (animator.CurrentAnimationName != "john.idle" && animator.CurrentAnimationName != "john.idle.alternate")
                animation = "john.idle";
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
            gravity = 50;
            runSpeed = 400;
            walkSpeed = 100;

            SetupInput();
            SetupFlashLight();

            interactiveEntitiesList = BuildInteractiveEntitiesList();
        }

        private List<Entity> BuildInteractiveEntitiesList()
        {
            return Entity.Scene.FindEntitiesWithTag((int) Layers.Tag.Interactive);
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
            inputInteract.Nodes.Add(new VirtualButton.KeyboardKey(Keys.I));

            inputRun = new VirtualButton();
            inputRun.Nodes.Add(new VirtualButton.KeyboardKey(Keys.LeftShift));

            inputXAxis = new VirtualIntegerAxis();
            inputXAxis.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            inputXAxis.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            inputXAxis.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left,
                Keys.Right));
        }

        private void Move()
        {
            var deltaMovement = Vector2.Zero;
            var moveDir = new Vector2(inputXAxis.Value, 0);

            velocity.Y += gravity * Time.DeltaTime;
            deltaMovement.Y = velocity.Y;

            // In case of collision with the ground
            if (collider.CollidesWithAny(ref deltaMovement, out var collisionResult) && collisionResult.Normal.Y < 0)
            {
                velocity = Vector2.Zero;

                if (moveDir.X < 0)
                {
                    animator.FlipY = true;
                    flashLight.Transform.SetRotationDegrees(180);
                    flashLight.SetLocalOffset(lightRightOffset);
                    animation = "john.walk";
                    velocity.X = -walkSpeed;
                    if (inputRun.IsDown)
                    {
                        animation = "john.footing";
                        velocity.X = -runSpeed;
                    }
                }
                else if (moveDir.X > 0)
                {
                    animator.FlipY = false;
                    flashLight.Transform.SetRotationDegrees(0);
                    flashLight.SetLocalOffset(lightLeftOffset);
                    animation = "john.walk";
                    velocity.X = walkSpeed;
                    if (inputRun.IsDown)
                    {
                        animation = "john.footing";
                        velocity.X = runSpeed;
                    }
                }
            }

            // move the Entity to the new position.deltaMovement is already adjusted to resolve collisions for us.
            deltaMovement += velocity * Time.DeltaTime;
            mover.Move(deltaMovement, out collisionResult);
        }
    }
}