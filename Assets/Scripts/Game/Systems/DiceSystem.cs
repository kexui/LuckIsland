using Core.Systems;

public class DiceSystem : SystemBase,IDiceSystem
{
    private System.Random random;
    
    // ========== SystemBase ==========
    protected override void OnInitialize()
    {
        
    }

    protected override void OnCleanup()
    {
        
    }

    public int Roll()
    {
        return random.Next(1, 6);
    }
}
