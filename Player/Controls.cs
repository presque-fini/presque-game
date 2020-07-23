using Microsoft.Xna.Framework.Input;
using Nez;

namespace game
{
    public class Controls
    {
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

        public VirtualButton Flashlight { get; }
        public VirtualButton Interact { get; }
        public VirtualButton Run { get; }
        public VirtualIntegerAxis XAxis { get; }
    }
}