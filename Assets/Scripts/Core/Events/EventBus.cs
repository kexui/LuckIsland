using System;
using System.Collections.Generic;

namespace Core.Events
{
    /// <summary>
    /// 事件总线（类型安全）
    /// </summary>
    public class EventBus : Singleton<EventBus>
    {
        protected override bool PersistAcrossScenes => false;

        // 事件存储：事件类型 -> 回调列表
        private readonly Dictionary<Type, List<Delegate>> handlers = new();
        // 主线程队列
        private readonly Queue<Action> mainThreadQueue = new();

        private void Update()
        {
            // 主线程执行队列
            while (mainThreadQueue.Count > 0)
            {
                mainThreadQueue.Dequeue()?.Invoke();
            }
        }

        /// <summary>订阅事件，返回 IDisposable 便于取消。</summary>
        public IDisposable Subscribe<T>(Action<T> handler)
        {
            var t = typeof(T);
            if (!handlers.TryGetValue(t, out var list))
            {
                list = new List<Delegate>();
                handlers[t] = list;
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
            if (handlers.TryGetValue(t, out var list))
            {
                list.Remove(handler);
            }
        }

        /// <summary>发布事件（立即在当前线程调用）。</summary>
        public void Publish<T>(T evt)
        {
            var t = typeof(T);
            if (handlers.TryGetValue(t, out var list))
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
            mainThreadQueue.Enqueue(() => Publish(evt));
        }

        private void OnDestroy()
        {
            handlers.Clear();
            mainThreadQueue.Clear();
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