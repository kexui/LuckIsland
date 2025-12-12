
namespace Core.Systems
{
    public interface ISystem 
    {
        bool IsInitialized { get; }//是否初始化
        bool IsEnabled { get; }//是否启用
        void Initialize();//初始化
        void Enable();//启用
        void Disable();//禁用
        void Cleanup();//清理
    }
}


