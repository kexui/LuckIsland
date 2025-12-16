using System.Collections.Generic;
using Core.Grid;
using Game.View.Map;
using UnityEngine;

namespace Game.Data.Map
{
    [System.Serializable]
    public class LandData
    {
        public int LandId;
        public BuildingData building;
        public int ownerPlayerId;
        public GridPos Pos;

    }
}
