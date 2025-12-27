namespace Core.Grid
{
    public class GridDirs
    {
        public static readonly GridPos Front = new GridPos(0, 0, 1);
        public static readonly GridPos Back = new GridPos(0, 0, -1);
        public static readonly GridPos Left = new GridPos(-1, 0, 0);
        public static readonly GridPos Right = new GridPos(1, 0, 0);

        public static readonly GridPos[] Dir = { Front, Back, Left, Right };
    }
}