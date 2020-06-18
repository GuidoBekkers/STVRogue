namespace STVrogue.Utils
{
    /// <summary>
    /// This class handles the generation of all ingame ID's,
    /// ensuring they are unique.
    /// </summary>
    public static class IdFactory
    {
        // The counters for the unique ID's
        // Safe from rollover from 0 to 4 billion. If more unique ID's are needed, switch to ulong
        private static uint _creatureIdCounter = 0;
        private static uint _roomIdCounter = 0;
        private static uint _itemIdCounter = 0;

        /// <summary>
        /// Get the next unique creature ID
        /// </summary>
        /// <returns>A string of the format "c#" with a unique ID</returns>
        public static string GetCreatureId()
        {
            return $"c{_creatureIdCounter++}";
        }

        /// <summary>
        /// Get the next unique room ID
        /// </summary>
        /// <returns>A string of the format "r#" with a unique ID</returns>
        public static string GetRoomId()
        {
            return $"r{_roomIdCounter++}";
        }

        /// <summary>
        /// Get the next unique item ID
        /// </summary>
        /// <returns>A string of the format "i#" with a unique ID</returns>
        public static string GetItemId()
        {
            return $"i{_itemIdCounter++}";
        }

        /// <summary>
        /// Reset the id counters
        /// </summary>
        public static void ResetIdFactory()
        {
            _creatureIdCounter = 0;
            _roomIdCounter = 0;
            _itemIdCounter = 0;
        }
    }
}