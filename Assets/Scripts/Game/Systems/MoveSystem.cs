using Core.Systems;
using Game.Enums;
using Game.Events;
using Game.Managers;

namespace Game.Systems
{
    public class MoveSystem: SystemBase , IMoveSystem
    {
        private IMapSystem mapSystem;
        private IPlayerSystem playerSystem;

        public  MoveSystem(IMapSystem mapSystem , IPlayerSystem playerSystem)
        {
            this.mapSystem = mapSystem;
            this.playerSystem = playerSystem;
        }

        // ========== SystemBase ==========
        protected override void OnInitialize()
        {
            
        }

        protected override void OnEnable()
        {
            SubscribeEvent<TurnPhaseChangedEvent>(OnPhaseChanged);
        }

        protected override void OnCleanup()
        {
            
        }
        
        // ========== Events ==========

        private void OnPhaseChanged(TurnPhaseChangedEvent evt)
        {
            if (evt.CurrentPhase.Phase == TurnPhase.Move)
            {
                MovePhase();
            }
        }

        // ========== IMoveSystem 实现 ==========

        public void MovePhase()
        {
            
        }

        private void Move(int playerId,int step)
        {
            
        }

        private bool IsPhase(TurnPhase phase)
        {
            if (GameManager.Instance?.TurnSystem == null)
            {
                return false;
            }

            var currentPhase = GameManager.Instance.TurnSystem.GetCurrentPhase().Phase;
            return currentPhase ==  phase;
        }
    }
}