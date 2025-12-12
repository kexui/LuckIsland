using Core.SystemLogic;
using Core.Systems;

public class MapSystem : SystemBace
{
    private MapLogic mapLogic;
    public MapSystem()
    {
        // 无依赖，直接创建
    }

    protected override void OnInitialize()
    {
        mapLogic = new MapLogic();
        mapLogic.Initialize();
    }

    public void Cleanup()
    {
        //mapLogic?.Cleanup();
        //mapLogic = null;
    }

    protected override void OnCleanup()
    {
        throw new System.NotImplementedException();
    }

    // ========== 公共接口 ==========
    public MapLogic GetMapLogic()
    {
        ValidateSystem();
        return mapLogic;
    }
    public TileLogic GetTile(int index)
    {
        ValidateSystem();
        return mapLogic.GetTile(index);
    }
}
