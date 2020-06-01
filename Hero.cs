using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace game
{
    public class Hero : Component, IUpdatable
    {
        public float Gravity { get; set; } = 1000;
        public float JumpHeight { get; set; } = 0.2f;
        public float MoveSpeed { get; set; } = 150;

        private SpriteAnimator _animator;
        private BoxCollider _boxCollider;
        private Mover _mover;
        private Vector2 _velocity;
        private VirtualButton _jumpInput;
        private VirtualIntegerAxis _xAxisInput;

        public override void OnAddedToEntity()
        {
            _boxCollider = Entity.GetComponent<BoxCollider>();
            _mover = Entity.GetComponent<Mover>();
            _animator = Entity.AddComponent<SpriteAnimator>();

            SetupAnimation();
            SetupInput();
        }

        private void SetupAnimation()
        {
            Texture2D heroTexture = Entity.Scene.Content.Load<Texture2D>("Sprites/RX_ANIM_global");
            var heroSprites = Sprite.SpritesFromAtlas(heroTexture, 64, 64);

            _animator.AddAnimation(
                "Idle",
                5f,
                heroSprites[0],
                heroSprites[1],
                heroSprites[2],
                heroSprites[3],
                heroSprites[4],
                heroSprites[5]
            );
            _animator.AddAnimation(
                "Walking",
                5f,
                heroSprites[0 + 6],
                heroSprites[1 + 6],
                heroSprites[2 + 6],
                heroSprites[3 + 6],
                heroSprites[4 + 6],
                heroSprites[5 + 6]
            );
        }

        private void SetupInput()
        {
            _xAxisInput = new VirtualIntegerAxis();
            _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
        }

        void IUpdatable.Update()
        {
            CollisionResult collisionResult;
            Vector2 deltaMovement = new Vector2(0);
            Vector2 moveDir = new Vector2(_xAxisInput.Value, 0);
            string animation = "Idle";

            _velocity.Y += 200 * Time.DeltaTime;
            deltaMovement.Y = _velocity.Y;

            if (_boxCollider.CollidesWithAny(ref deltaMovement, out collisionResult))
            {
                Debug.Log(deltaMovement);
                // only allow horizontal movement and jumping if the player is on the ground
                if (collisionResult.Normal.Y < 0)
                {
                    // reset velocity to prevent movement without user input
                    _velocity = Vector2.Zero;
                    if (moveDir.X < 0)
                    {
                        _velocity.X = -MoveSpeed;
                        _animator.FlipX = true;
                        animation = "Walking";
                    }
                    else if (moveDir.X > 0)
                    {
                        _velocity.X = MoveSpeed;
                        _animator.FlipX = false;
                        animation = "Walking";
                    }
                }
            }

            // move the Entity to the new position. deltaMovement is already adjusted to resolve collisions for us.
            deltaMovement += _velocity * Time.DeltaTime;
            _mover.Move(deltaMovement, out collisionResult);
            if (!_animator.IsAnimationActive(animation))
                _animator.Play(animation);
        }
    }
}