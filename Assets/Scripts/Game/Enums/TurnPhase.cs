namespace Game.Enums
{
    public enum TurnPhase
    {
        Start, // 回合开始
        Wait, // 等待
        RollDice, // 投骰子
        Move, // 移动加事件触发
        Player, // 玩家操作回合
        End //回合结束
    }
}
