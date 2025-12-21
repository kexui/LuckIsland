using System.Collections.Generic;
using Core.Systems;
using Game.View.Map;

namespace Game.Systems
{
    public class ViewSystem: SystemBase,IViewSystem
    {
        private IMapSystem mapSystem;  // 依赖 MapSystem
        private Dictionary<int, TileView> tileViewMap;
        private Dictionary<int, LandView> landViewMap;
        
        protected override void OnInitialize()
        {
            tileViewMap = new Dictionary<int, TileView>();
            landViewMap = new Dictionary<int, LandView>();

            CollectViewsFromScene();
        }

        protected override void OnCleanup()
        {
            throw new System.NotImplementedException();
        }

        private void CollectViewsFromScene()
        {
            
        }
    }
}