using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    /// <summary>
    /// 事件总线（类型安全）
    /// </summary>
    public class EventBus : MonoBehaviour
    {
        // 简单单例，可用你自己的 Singleton<T>
        private static EventBus _instance;
        public static EventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject(nameof(EventBus));
                    _instance = go.AddComponent<EventBus>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        // 事件存储：事件类型 -> 回调列表
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();
        // 主线程队列
        private readonly Queue<Action> _mainThreadQueue = new();

        private void Update()
        {
            // 主线程执行队列
            while (_mainThreadQueue.Count > 0)
            {
                _mainThreadQueue.Dequeue()?.Invoke();
            }
        }

        /// <summary>订阅事件，返回 IDisposable 便于取消。</summary>
        public IDisposable Subscribe<T>(Action<T> handler)
        {
            var t = typeof(T);
            if (!_handlers.TryGetValue(t, out var list))
            {
                list = new List<Delegate>();
                _handlers[t] = list;
            }
            list.Add(handler);
            return new Subscription<T>(this, handler);
        }

        /// <summary>订阅一次性事件，触发后自动移除。</summary>
        public IDisposable SubscribeOnce<T>(Action<T> handler)
        {
            Action<T> wrapper = null;
            wrapper = (evt) =>
            {
                handler(evt);
                Unsubscribe(wrapper);
            };
            return Subscribe(wrapper);
        }

        /// <summary>取消订阅。</summary>
        public void Unsubscribe<T>(Action<T> handler)
        {
            var t = typeof(T);
            if (_handlers.TryGetValue(t, out var list))
            {
                list.Remove(handler);
            }
        }

        /// <summary>发布事件（立即在当前线程调用）。</summary>
        public void Publish<T>(T evt)
        {
            var t = typeof(T);
            if (_handlers.TryGetValue(t, out var list))
            {
                // 拷贝一份避免遍历中修改
                var snapshot = list.ToArray();
                for (int i = 0; i < snapshot.Length; i++)
                {
                    (snapshot[i] as Action<T>)?.Invoke(evt);
                }
            }
        }

        /// <summary>发布到下一帧（主线程队列）。</summary>
        public void PublishNextFrame<T>(T evt)
        {
            _mainThreadQueue.Enqueue(() => Publish(evt));
        }

        private class Subscription<T> : IDisposable
        {
            private readonly EventBus bus;
            private readonly Action<T> handler;
            private bool disposed;

            public Subscription(EventBus bus, Action<T> handler)
            {
                this.bus = bus;
                this.handler = handler;
            }

            public void Dispose()
            {
                if (disposed) return;
                bus.Unsubscribe(handler);
                disposed = true;
            }
        }
    }
}