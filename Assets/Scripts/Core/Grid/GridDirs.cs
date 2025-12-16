namespace Core.Grid
{
    public class GridDirs
    {
        public static readonly GridPos Up = new GridPos(0, 1);
        public static readonly GridPos Down = new GridPos(0, -1);
        public static readonly GridPos Left = new GridPos(-1, 0);
        public static readonly GridPos Right = new GridPos(1, 0);

        public static readonly GridPos[] Four = { Up, Down, Left, Right };
    }
}