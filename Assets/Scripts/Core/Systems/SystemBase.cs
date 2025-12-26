using System;
using UnityEngine;
using Core.Events;

namespace Core.Systems
{
    public abstract class SystemBase : ISystem
    {
        // ========== 状态 ==========
        public bool IsInitialized { get; private set; } = false;
        public bool IsEnabled { get; private set; }  = false;
        
        // ========== 生命周期 ==========
        public void Initialize()
        {
            if (IsInitialized)
            {
                Debug.LogWarning($"{GetType().Name} 已经初始化过了");
                return;
            }
            OnInitialize();
            IsInitialized = true;
        }
        protected abstract void OnInitialize();
        
        public virtual void Enable()
        {
            if (!IsInitialized)
            {
                Debug.LogWarning($"{GetType().Name} 未初始化，无法启用");
                return;
            }

            if (IsEnabled)
            {
                return;
            }

            IsEnabled = true;
            OnEnable();
        }
        protected virtual void OnEnable() { }

        public virtual void Disable()
        {
            if (!IsEnabled)
            {
                return;
            }

            IsEnabled = false;
            OnDisable();
        }
        protected virtual void OnDisable() { }

        public virtual void Cleanup()
        {
            if (!IsInitialized) return;
            
            Disable();
            ClearSubscriptions(); // 自动清理订阅
            OnCleanup();
            IsInitialized = false;
            
            Debug.Log($"{GetType().Name} 清理完成");
        }
        protected abstract void OnCleanup();
        
        // ========== 事件订阅管理 ==========
        
        /// <summary>
        /// 订阅的事件列表（用于自动清理）
        /// </summary>
        private System.Collections.Generic.List<IDisposable> _subscriptions = new();
        
        /// <summary>
        /// 安全订阅事件（自动管理生命周期）
        /// </summary>
        protected IDisposable SubscribeEvent<T>(Action<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
    
            if (EventBus.Instance == null)
            {
                Debug.LogWarning($"EventBus未初始化，无法订阅事件 {typeof(T).Name}");
                return null;
            }
    
            var subscription = EventBus.Instance.Subscribe(handler);
            if (subscription != null)
            {
                _subscriptions.Add(subscription);
            }
            return subscription;
        }
        
        /// <summary>
        /// 订阅一次性事件（触发后自动移除）
        /// </summary>
        protected IDisposable SubscribeEventOnce<T>(Action<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (EventBus.Instance == null)
            {
                Debug.LogWarning($"EventBus未初始化，无法订阅事件 {typeof(T).Name}");
                return null;
            }

            var subscription = EventBus.Instance.SubscribeOnce(handler);
            if (subscription != null)
            {
                _subscriptions.Add(subscription);
            }
            return subscription;
        }
        
        /// <summary>
        /// 清理所有订阅
        /// </summary>
        protected virtual void ClearSubscriptions()
        {
            if (_subscriptions == null || _subscriptions.Count == 0)
            {
                return;
            }
            
            int count = _subscriptions.Count;
            foreach (var subscription in _subscriptions)
            {
                try
                {
                    subscription?.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{GetType().Name} 清理订阅时出错: {ex.Message}");
                }
            }
            
            _subscriptions.Clear();
            Debug.Log($"{GetType().Name} 清理了 {count} 个事件订阅");
        }
        
        // ========== 辅助方法（protected，只给子类用）==========
        
        //检查必需依赖
        protected void RequireDependency<T>(T dependency, string dependencyName = null) where T : class
        {
            if (dependency == null)
            {
                string name = dependencyName ?? typeof(T).Name;
                throw new ArgumentNullException(
                    name,
                    $"{GetType().Name} 缺少必需依赖: {name}"
                );
            }
        }
        
        //检查依赖是否已初始化
        protected void RequireInitialized<T>(T system, string systemName = null) where T : ISystem
        {
            if (system == null)
            {
                string name = systemName ?? typeof(T).Name;
                throw new ArgumentNullException(name);
            }
            
            if (!system.IsInitialized)
            {
                string name = systemName ?? typeof(T).Name;
                throw new InvalidOperationException(
                    $"{GetType().Name} 的依赖 {name} 未初始化"
                );
            }
        }
        
        //验证System是否可用
        protected void ValidateSystem()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} 未初始化");
            }
            
            if (!IsEnabled)
            {
                throw new InvalidOperationException($"{GetType().Name} 未启用");
            }
        }
    }
}