namespace Core.Grid
{
    [System.Serializable]
    public struct GridPos
    {
        public int X;
        public int Y;
        public int Z;

        public GridPos(int x, int y,int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is not GridPos other)
            {
                return false;
            }
            return other.X == X && other.Y == Y &&  other.Z == Z;
        }

        public override int GetHashCode()
        {
            return (X, Y, Z).GetHashCode();
        }
    }
}
