using System.Collections.Generic;
using game.Definitions;
using Microsoft.Xna.Framework;
using Nez;

namespace game
{
    public class Hero : Component, IUpdatable
    {
        private readonly float gravity = 50;
        private readonly float runSpeed = 400;
        private readonly int sinScale = 20;
        private readonly int timeScale = 2;
        private readonly float walkSpeed = 100;
        private SpriteAnimation animator;
        private BoxCollider collider;
        private Controls controls;
        private Flashlight flashlight;
        private List<Entity> interactiveEntitiesList;
        private Mover mover;
        private Vector2 velocity;

        /// <summary>
        ///     This method is called each frame.
        /// </summary>
        public void Update()
        {
            if (controls.Flashlight.IsPressed)
                flashlight.Toggle();

            animator.Idle();
            Move();
            Interact(interactiveEntitiesList);

            //flashlight.LocalOffset += new Vector2(0, (float) (Math.Sin(Time.TotalTime * timeScale)/sinScale));
            //flashlight.Transform.LocalRotation = (float) Math.Sin(Time.TotalTime)/ sinScale;

            if (!animator.IsAnimationActive(animator.Animation))
                animator.Play(animator.Animation);
        }

        /// <summary>
        ///     Loops through a list of entities tagged as Layers.Tag.Interactive. The list is built when OnAddedToEntity() is
        ///     called.
        /// </summary>
        /// <param name="list">The list of entities that the player can interact with.</param>
        private void Interact(List<Entity> list)
        {
            foreach (var entity in list)
                if (controls.Interact.IsPressed && Entity.GetComponent<Collider>()
                    .Overlaps(entity.GetComponent<Collider>()))
                {
                    if (entity.Tag == (int) Tag.Active)
                        entity.SetTag((int) Tag.Inactive);
                    else
                        entity.SetTag((int) Tag.Active);
                }
        }

        public override void OnAddedToEntity()
        {
            var heroAtlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");

            interactiveEntitiesList = BuildInteractiveEntitiesList();

            controls = new Controls();
            flashlight = new Flashlight(Entity);
            Entity.Scene.AddEntity(flashlight);
            collider = new BoxCollider
            {
                CollidesWithLayers = (int) PhysicsLayer.Player
            };
            mover = new Mover();
            animator = new SpriteAnimation();
            animator.AddAnimationsFromAtlas(heroAtlas);
            animator.RenderLayer = (int) RenderLayer.Player;

            Entity.AddComponent(collider);
            Entity.AddComponent(mover);
            Entity.AddComponent(animator);
        }

        private List<Entity> BuildInteractiveEntitiesList()
        {
            return Entity.Scene.FindEntitiesWithTag((int) Tag.Interactive);
        }

        private void Move()
        {
            var deltaMovement = Vector2.Zero;
            var moveDir = new Vector2(controls.XAxis.Value, 0);

            velocity.Y += gravity * Time.DeltaTime;
            deltaMovement.Y = velocity.Y;

            // In case of collision with the ground
            if (collider.CollidesWithAny(ref deltaMovement, out var collisionResult) && collisionResult.Normal.Y < 0)
            {
                velocity = Vector2.Zero;

                if (moveDir.X < 0)
                {
                    animator.FlipX = true;
                    flashlight.FlipX = true;
                    flashlight.SetOffset("right");
                    animator.Animation = "john.walk";
                    velocity.X = -walkSpeed;
                    if (controls.Run.IsDown)
                    {
                        animator.Animation = "john.footing";
                        velocity.X = -runSpeed;
                    }
                }
                else if (moveDir.X > 0)
                {
                    animator.FlipX = false;
                    flashlight.FlipX = false;
                    flashlight.SetOffset("left");
                    animator.Animation = "john.walk";
                    velocity.X = walkSpeed;
                    if (controls.Run.IsDown)
                    {
                        animator.Animation = "john.footing";
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