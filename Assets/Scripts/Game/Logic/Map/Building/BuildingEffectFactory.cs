using System.Collections.Generic;
using Core.Enums;
using UnityEngine;

namespace Game.Logic.Building
{
    public static class BuildingEffectFactory
    {
        private static readonly Dictionary<BuildingType, IBuildingEffect> effects =
            new Dictionary<BuildingType, IBuildingEffect>
            {
                { BuildingType.Start, new StartBuildingEffect() },
                { BuildingType.Shop, new ShopBuildingEffect() },
                { BuildingType.Property, new PropertyBuildingEffect() },
            };

        public static IBuildingEffect GetEffect(BuildingType type)
        {
            if (effects.TryGetValue(type, out IBuildingEffect effect))
            {
                return effect;
            }

            Debug.LogWarning($"未找到Building类型{type}的效果实现");
            return null;
        }
    }
}