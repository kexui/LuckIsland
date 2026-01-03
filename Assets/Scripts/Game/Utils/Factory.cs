using Game.Data.Player;
using Game.View.Player;
using UnityEngine;

namespace Game.Utils
{
    public class Factory
    {
        private Transform playerRoot;
        private const string PLAYER_PATH = "Prefabs/Characters/";

        public Factory()
        {
            var go = new GameObject("PlayersRoot");
            playerRoot = go.transform;
        }

        /// <summary>
        /// 创建Logic，模型，view
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public PlayerLogic CreatePlayerLogic(PlayerData data)
        {
            GameObject prefab = Resources.Load<GameObject>(PLAYER_PATH + data.characterPrefabName);
            if (prefab == null)
            {
                Debug.LogError($"Factory: 未能找到玩家角色，Name：{data.characterPrefabName}，Path：{PLAYER_PATH}");
                return null;
            }

            PlayerLogic logic = new PlayerLogic(data);

            //todo 思考：需不需要分离view的生成
            GameObject go = GameObject.Instantiate(prefab, playerRoot);
            var view = go.AddComponent<PlayerView>();
            view.Initialize(logic.GetId(),logic);
            
            return logic;
        }
    }
}