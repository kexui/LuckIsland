namespace Game.Events
{
    public class PlayerMovedEvent
    {
        public int PlayerId;
        public int FromTileIndex;
        public int ToTileIndex;
    }
}