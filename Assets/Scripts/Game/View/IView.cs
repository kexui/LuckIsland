namespace Game.View
{
    public interface IView
    {
        int GetId();
        void Initialize();
        void UpdataView();
        void Refresh();
        void Show();
        void Hide();
        void Cleanup();
    }
}