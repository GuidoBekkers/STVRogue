namespace STVrogue.GameLogic
{
    /// <summary>
    /// A parent class representing all game entities in STV Rogue.
    /// </summary>
    public class GameEntity
    {
        /// <summary>
        /// Every entity is identified by a unique ID.
        /// </summary>
        string id;

        public GameEntity(string uniqueId)
        {
            this.id = uniqueId;
        }
        public string Id => id;
    }
}
