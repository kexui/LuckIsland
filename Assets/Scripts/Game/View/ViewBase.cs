using UnityEngine;

namespace Game.View
{
    public abstract class ViewBase : MonoBehaviour , IView
    {
        public abstract int GetId();

        public virtual void Initialize()
        {
            
        }

        public virtual void UpdataView()
        {
            
        }

        public virtual void Refresh()
        {
            
        }

        public virtual void Show()
        {
            
        }

        public virtual void Hide()
        {
            
        }

        public virtual void Cleanup()
        {
            
        }
    }
}