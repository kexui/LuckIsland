using System.Collections.Generic;
using Core.Systems;
using Game.View.Building;
using Game.View.Map;
using Unity.VisualScripting;
using UnityEngine;
namespace Game.Systems
{
    public class ViewSystem: SystemBase,IViewSystem
    {
        private IMapSystem mapSystem;  // 依赖 MapSystem
        private Dictionary<int, TileView> tileViewMap;
        private Dictionary<int, LandView> landViewMap;
        private Dictionary<int, BuildingView> buildingViewMap;

        public ViewSystem(IMapSystem mapSystem)
        {
            tileViewMap = new Dictionary<int, TileView>();
            landViewMap = new Dictionary<int, LandView>();
            this.mapSystem = mapSystem;
        }

        protected override void OnInitialize()
        {
            CollectViewsFromScene();
        }

        protected override void OnCleanup()
        {
            throw new System.NotImplementedException();
        }

        private void CollectViewsFromScene()
        {
            var allTileViews = Object.FindObjectsOfType<TileView>();
            foreach (var tileView in allTileViews)
            {
                tileViewMap.Add(tileView.GetId(), tileView);
            }
            
            var allLandViews = Object.FindObjectsOfType<LandView>();
            foreach (var landView in allLandViews)
            {
                landViewMap.Add(landView.GetId(), landView);
            }
            
            var allBuildingViews = Object.FindObjectsOfType<BuildingView>();
            foreach (var buildingView in allBuildingViews)
            {
                buildingViewMap.Add(buildingView.GetId(), buildingView);
            }
        }
    }
}