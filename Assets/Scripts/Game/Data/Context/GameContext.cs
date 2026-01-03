using System.Collections.Generic;
using Core.Systems;
using Game.Systems;

namespace Game.Data.Context
{
    public class GameContext
    {
        public IMapSystem MapSystem { get; }
        public IPlayerSystem PlayerSystem{ get; }
        public IMoveSystem MoveSystem{ get; }
        public ITurnSystem TurnSystem{ get; }
        public IDiceSystem DiceSystem{ get; }

        private readonly List<ISystem> allSystems = new();

        public GameContext()
        {
            var mapSystem = new MapSystem();
            var playerSystem = new PlayerSystem(mapSystem);
            var moveSystem = new MoveSystem(mapSystem, playerSystem);
            var turnSystem = new TurnSystem();
            var diceSystem = new DiceSystem();

            MapSystem = mapSystem;
            PlayerSystem = playerSystem;
            MoveSystem = moveSystem;
            TurnSystem = turnSystem;
            DiceSystem = diceSystem;
            
            allSystems.Add(mapSystem);
            allSystems.Add(playerSystem);
            allSystems.Add(moveSystem);
            allSystems.Add(turnSystem);
            allSystems.Add(diceSystem);
        }

        public void Initialize()
        {
            foreach (var system in allSystems)
            {
                system.Initialize();
            }
        }
        
        public void Enable()
        {
            foreach (var system in allSystems)
            {
                system.Enable();
            }
        }

        public void Cleanup()
        {
            foreach (var system in allSystems)
            {
                system.Cleanup();
            }
        }
    }
}