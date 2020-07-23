using System;
using game.Definitions;
using Microsoft.Xna.Framework;
using Nez;
using Nez.DeferredLighting;

namespace game
{
    public class Flashlight : Entity
    {
        private const double tolerance = 0.1;
        private readonly Entity entity;
        private readonly Vector2 leftOffset = new Vector2(6f, -35f);
        private readonly Vector2 rightOffset = new Vector2(-6f, -35f);
        private SpotLight flashlight;

        public Flashlight(Entity entity)
        {
            this.entity = entity;
        }

        /// <summary>
        ///     determines if the flashlight should be rendered normally or flipped horizontally
        /// </summary>
        /// <value><c>true</c> if flip x; otherwise, <c>false</c>.</value>
        public bool FlipX
        {
            get => Math.Abs(flashlight.Transform.LocalRotationDegrees - 180) < tolerance;
            set => flashlight.Transform.LocalRotationDegrees = value ? 180 : 0;
        }

        public override void OnAddedToScene()
        {
            var flashlightEntity = Scene.CreateEntity("flashlight");
            flashlightEntity.SetParent(entity);

            flashlight = new SpotLight
            {
                Color = Color.White,
                ConeAngle = 90,
                Intensity = 2f,
                LocalOffset = rightOffset,
                RenderLayer = (int) RenderLayer.Light
            };

            flashlightEntity.AddComponent(flashlight);
        }

        /// <summary>
        ///     Toggle on or off the flashlight;
        /// </summary>
        public void Toggle()
        {
            if (flashlight.Enabled)
                flashlight.SetEnabled(false);
            else if (!flashlight.Enabled)
                flashlight.SetEnabled(true);
        }

        /// <summary>
        ///     Offset the flashlight component from its entity.
        /// </summary>
        /// <param name="type">"left" or "right"</param>
        public void SetOffset(string type)
        {
            switch (type)
            {
                case "right":
                    flashlight.SetLocalOffset(rightOffset);
                    break;
                case "left":
                    flashlight.SetLocalOffset(leftOffset);
                    break;
            }
        }
    }
}