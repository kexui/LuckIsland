namespace Core.Grid
{
    [System.Serializable]
    public struct GridPos
    {
        public int X;
        public int Y;

        public GridPos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is not GridPos other)
            {
                return false;
            }
            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }
    }
}
