using Core.Events;
using Game.Logic.Map;
using Game.Managers;
using Game.Utils;
using UnityEngine;

namespace Game.View.Player
{
    public class PlayerView : ViewBase
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject characterModel;
        private int playerId;
        private PlayerLogic playerLogic;

        // 动画参数常量
        private const string SPEED = "Speed";
        private const string IS_MOVING = "IsMoving";
        private const string TRIGGER_KNOCKBACK = "TriggerKnockback";

        public override int GetId()
        {
            return playerId;
        }

        public PlayerView()
        {
            
        }

        public void Initialize(int id, PlayerLogic logic)
        {
            playerId = id;
            playerLogic = logic;
            
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            SetWait();
            InitializePosition();
            SubscribeToEvents();
        }

        private void InitializePosition()
        {
            if (playerLogic == null)
            {
                Debug.LogWarning($"PlayerView {playerId}: PlayerLogic为空，无法初始化位置");
                return;
            }
            
            var tile = GameManager.Instance.Context.MapSystem.GetTile(playerLogic.GetCurrentTileIndex());
            SetPosition(tile);
        }

        private void SetPosition(TileLogic tileLogic)
        {
            if (playerLogic != null && GameManager.Instance.Context.MapSystem != null)
            {
                if (tileLogic != null)
                {
                    Vector3 pos = GridHelper.GetPositionByGridPos(tileLogic.Pos, GridManager.Instance.cellSize);
                    transform.position = new Vector3(pos.x, pos.y + GridManager.Instance.cellSize, pos.z);
                }
            }
        }

        private void SubscribeToEvents()
        {
            //EventBus.Instance?.Subscribe<Events.PlayerMovedEvent>(OnPlayerMoved);
            //EventBus.Instance?.Subscribe<Events.PlayerKnockbackEvent>(OnPlayerKnockback);
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
        
        // private void OnPlayerMoved(Events.PlayerMovedEvent evt)
        // {
        //     if (evt.PlayerId == playerId)
        //     {
        //         
        //     }
        // }
        //
        // private void OnPlayerKnockback(Events.PlayerKnockbackEvent evt)
        // {
        //     if (evt.PlayerId == playerId)
        //     {
        //         
        //     }
        // }
        
    }
}