using System;
using System.Collections.Generic;
using game.Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.DeferredLighting;
using Nez.Sprites;
using Random = Nez.Random;

namespace game.Characters
{
    public class Hero : Component, IUpdatable
    {
        private string animation = "john.idle";
        private SpriteAnimator animator;
        private BoxCollider collider;
        private SpotLight flashlight;
        private Entity flashlightEntity;
        private readonly float gravity = 50;
        private VirtualButton inputFlashlight;
        private VirtualButton inputInteract;
        private VirtualButton inputRun;
        private VirtualIntegerAxis inputXAxis;
        private List<Entity> interactiveEntitiesList;
        private readonly Vector2 lightLeftOffset = new Vector2(6f, -35f);
        private readonly Vector2 lightRightOffset = new Vector2(-6f, -35f);
        private Mover mover;
        private readonly float runSpeed = 400;
        private Vector2 velocity;
        private readonly float walkSpeed = 100;
        private readonly int sinScale = 20;
        private readonly int timeScale = 2;

        /// <summary>
        ///     This method is called each frame.
        /// </summary>
        public void Update()
        {
            // Light toggle
            if (flashlight.Enabled && inputFlashlight.IsPressed)
                flashlight.SetEnabled(false);
            else if (!flashlight.Enabled && inputFlashlight.IsPressed)
                flashlight.SetEnabled(true);


            IdleAnimation();
            Move();
            Interact(interactiveEntitiesList);

            //flashlight.LocalOffset += new Vector2(0, (float) (Math.Sin(Time.TotalTime * timeScale)/sinScale));
            //flashlight.Transform.LocalRotation = (float) Math.Sin(Time.TotalTime)/ sinScale;

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
            var heroAtlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");

            collider = new BoxCollider
            {
                CollidesWithLayers = (int) Layers.PhysicsLayer.Player
            };
            mover = new Mover();
            animator = new SpriteAnimator();
            animator.AddAnimationsFromAtlas(heroAtlas);
            animator.RenderLayer = (int) Layers.RenderLayer.Player;

            Entity.AddComponent(collider);
            Entity.AddComponent(mover);
            Entity.AddComponent(animator);

            SetupInput();
            AddFlashlight();

            interactiveEntitiesList = BuildInteractiveEntitiesList();
        }

        private List<Entity> BuildInteractiveEntitiesList()
        {
            return Entity.Scene.FindEntitiesWithTag((int) Layers.Tag.Interactive);
        }

        private void AddFlashlight()
        {
            flashlightEntity = Entity.Scene.CreateEntity("flashlight");
            flashlightEntity.SetParent(Entity);

            flashlight = new SpotLight
            {
                Color = Color.White,
                ConeAngle = 90,
                Intensity = 2f,
                LocalOffset = lightRightOffset,
                RenderLayer = (int) Layers.RenderLayer.Light
            };

            flashlightEntity.AddComponent(flashlight);
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
                    animator.FlipX = true;
                    flashlight.Transform.SetRotationDegrees(180);
                    flashlight.SetLocalOffset(lightRightOffset);
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
                    animator.FlipX = false;
                    flashlight.Transform.SetRotationDegrees(0);
                    flashlight.SetLocalOffset(lightLeftOffset);
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