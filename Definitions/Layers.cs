namespace game.Definitions
{
    /// <summary>
    ///     These layers are used for collision information.
    /// </summary>
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

    /// <summary>
    ///     These tags are used to build an list of interactive objects. The list is built once at the start of the game.
    /// </summary>
    public enum Tag
    {
        None,
        Interactive,
        Active,
        Inactive
    }
}