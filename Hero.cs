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

        private SpriteAnimator _animator;
        private BoxCollider _boxCollider;
        private Mover _mover;
        private Vector2 _velocity;
        private VirtualIntegerAxis _xAxisInput;
        private VirtualButton _runInput;

        public override void OnAddedToEntity()
        {
            SpriteAtlas heroAtlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");

            _boxCollider = Entity.AddComponent<BoxCollider>();
            _mover = Entity.AddComponent<Mover>();
            _animator = Entity.AddComponent<SpriteAnimator>().AddAnimationsFromAtlas(heroAtlas);

            SetupInput();
        }

        private void SetupInput()
        {
            _xAxisInput = new VirtualIntegerAxis();
            _xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            _xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

            _runInput = new VirtualButton();
            _runInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.LeftShift));
        }

        void IUpdatable.Update()
        {
            CollisionResult collisionResult;
            Vector2 deltaMovement = new Vector2(0);
            Vector2 moveDir = new Vector2(_xAxisInput.Value, 0);
            string animation = "john.idle";

            _velocity.Y += 50 * Time.DeltaTime;
            deltaMovement.Y = _velocity.Y;

            if (_boxCollider.CollidesWithAny(ref deltaMovement, out collisionResult) && collisionResult.Normal.Y < 0)
            {
                // reset velocity to prevent movement without user input
                _velocity = Vector2.Zero;
                MoveSpeed = 100;
                _animator.Speed = 1;
                if (moveDir.X < 0)
                {
                    _animator.FlipX = true;
                    animation = "john.walk";
                    if(_runInput.IsDown)
                    {
                        animation = "john.run";
                        MoveSpeed = 400;
                        _animator.Speed = 1.5f;
                    }
                    _velocity.X = -MoveSpeed;
                }
                else if (moveDir.X > 0)
                {
                    _animator.FlipX = false;
                    animation = "john.walk";
                    if (_runInput.IsDown)
                    {
                        animation = "john.run";
                        MoveSpeed = 400;
                        _animator.Speed = 1.5f;
                    }
                    _velocity.X = MoveSpeed;
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