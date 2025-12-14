namespace Core.Logic
{
    public interface ILogic
    {
        /// <summary>
        /// 获取唯一ID
        /// </summary>
        int GetId();
        
        /// <summary>
        /// 克隆（深拷贝）
        /// </summary>
        //ILogic Clone();
        
        /// <summary>
        /// 序列化为JSON（用于网络传输）
        /// </summary>
        string ToJson();
        
        /// <summary>
        /// 从JSON反序列化
        /// </summary>
        bool FromJson(string json);
        
        /// <summary>
        /// 验证数据有效性
        /// </summary>
        /// <returns>是否有效</returns>
        bool Validate();
    }
}