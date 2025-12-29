namespace Game.Events
{
    public class DiceRolledEvent
    {
        public int Result;        // 骰子点数
        public int PlayerId;     // 投掷的玩家ID（可选）
    }
}