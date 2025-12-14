using System;
using UnityEngine;

namespace Core.Logic
{
    [Serializable]
    public abstract class LogicBase : ILogic
    {
        // ========== ILogic 实现 ==========
        
        [SerializeField] protected int id = -1;

        /// <summary>
        /// 获取ID（子类必须实现）
        /// </summary>
        public virtual int GetId() => id;

        public virtual string ToJson()
        {
            try
            {
                return JsonUtility.ToJson(this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{GetType().Name} 序列化失败: {ex.Message}");
                return string.Empty;
            }
        }

        public virtual bool FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning($"{GetType().Name} 反序列化失败: JSON为空");
                return false;
            }
            
            try
            {
                JsonUtility.FromJsonOverwrite(json, this);
                
                // 反序列化后验证
                if (!Validate())
                {
                    Debug.LogWarning($"{GetType().Name} 反序列化后验证失败");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{GetType().Name} 反序列化失败: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 验证数据有效性（子类可重写）
        /// </summary>
        public virtual bool Validate()
        {
            // 默认验证：ID不能为负数
            int id = GetId();
            if (id < 0)
            {
                Debug.LogWarning($"{GetType().Name} ID无效: {id}");
                return false;
            }
            return true;
        }
        
        // ========== 辅助方法 ==========
        
        /// <summary>
        /// 转换为字符串（用于调试）
        /// </summary>
        public override string ToString()
        {
            return $"{GetType().Name}(Id={GetId()})";
        }
    }
}