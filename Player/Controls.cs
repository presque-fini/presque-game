using Microsoft.Xna.Framework.Input;
using Nez;

namespace game
{
    /// <summary>
    ///     This class holds all the possible inputs.
    /// </summary>
    public class Controls
    {
        /// <summary>
        ///     Define the controls.
        /// </summary>
        public Controls()
        {
            Flashlight = new VirtualButton();
            Flashlight.Nodes.Add(new VirtualButton.KeyboardKey(Keys.T));

            Interact = new VirtualButton();
            Interact.Nodes.Add(new VirtualButton.KeyboardKey(Keys.I));

            Run = new VirtualButton();
            Run.Nodes.Add(new VirtualButton.KeyboardKey(Keys.LeftShift));

            XAxis = new VirtualIntegerAxis();
            XAxis.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            XAxis.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            XAxis.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left,
                Keys.Right));
        }

        /// <summary>
        ///     Turn on or off the flashlight.
        /// </summary>
        public VirtualButton Flashlight { get; }

        /// <summary>
        ///     Interact with objects (open a door, grab an object...).
        /// </summary>
        public VirtualButton Interact { get; }

        /// <summary>
        ///     Run modifier.
        /// </summary>
        public VirtualButton Run { get; }

        /// <summary>
        ///     Left/right axis.
        /// </summary>
        public VirtualIntegerAxis XAxis { get; }
    }
}