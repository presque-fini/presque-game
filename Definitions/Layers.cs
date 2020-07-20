namespace game.Definitions
{
    internal class Layers
    {
        public enum PhysicsLayer
        {
            None,
            Player,
            Background
        }

        public enum RenderLayer
        {
            None,
            Light,
            Foreground,
            Player,
            Items,
            Background
        }

        public enum Tag
        {
            None,
            Interactive,
            Active,
            Inactive
        }
    }
}