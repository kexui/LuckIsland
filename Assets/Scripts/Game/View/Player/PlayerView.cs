using Core.Events;
using Game.Managers;
using UnityEngine;

namespace Game.View.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private int playerId;
        private PlayerLogic playerLogic;
        private GameObject characterModel;

        private const string prefabName = "Prefabs/Characters/"; 
        // 动画参数常量
        private const string SPEED = "Speed";
        private const string IS_MOVING = "IsMoving";
        private const string TRIGGER_KNOCKBACK = "TriggerKnockback";

        public int GetPlayerId() => playerId;
        
        public void Initialize(int id, PlayerLogic logic)
        {
            playerId = id;
            playerLogic = logic;
            
            LoadCharacterModel(logic.GetCharacterPrefabName());
            SubscribeToEvents();
        }
        
        private void LoadCharacterModel(string characterName)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName + characterName);

            if (prefab == null)
            {
                Debug.LogError($"无法加载角色预制体: {prefabName + characterName}");
                return;
            }

            characterModel = Instantiate(prefab, transform);
            characterModel.name = $"Character_{playerId}";
            
            animator = characterModel.GetComponent<Animator>();
            if (animator == null)
            {
                animator = characterModel.GetComponentInChildren<Animator>();
            }

            if (playerLogic != null && GameManager.Instance?.MapSystem != null)
            {
                var tile = GameManager.Instance.MapSystem.GetTile(playerLogic.GetId());
                if (tile != null)
                {
                    //transform.position = tile.Position;
                }
            }

            SetWait();
        }

        private void SubscribeToEvents()
        {
            EventBus.Instance?.Subscribe<Events.PlayerMovedEvent>(OnPlayerMoved);
            EventBus.Instance?.Subscribe<Events.PlayerKnockbackEvent>(OnPlayerKnockback);
        }
        
        private void OnDestroy()
        {
            // 取消订阅（如果需要）
        }
        
        // ========== 动画控制 ==========
        
        public void SetIdle()
        {
            if (animator != null)
            {
                animator.SetFloat(SPEED, 0f);
                animator.SetBool(IS_MOVING, false);
            }
        }
        
        public void SetWalk()
        {
            if (animator != null)
            {
                animator.SetFloat(SPEED, 1f);
                animator.SetBool(IS_MOVING, true);
            }
        }

        public void SetWait()
        {
            
        }

        public void TriggerKnockback()
        {
            if (animator != null)
            {
                animator.SetTrigger(TRIGGER_KNOCKBACK);
            }
        }
        
        // ========== Events ==========
        
        private void OnPlayerMoved(Events.PlayerMovedEvent evt)
        {
            if (evt.PlayerId == playerId)
            {
                
            }
        }
        
        private void OnPlayerKnockback(Events.PlayerKnockbackEvent evt)
        {
            if (evt.PlayerId == playerId)
            {
                
            }
        }
        
    }
}