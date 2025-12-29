using Core.Events;
using Core.Systems;
using Game.Enums;
using Game.Events;
using Game.Systems.Turn;
using UnityEngine;

namespace Game.View.Button
{
    public class ButtonPanel: MonoBehaviour
    {
        [Header("按钮引用")]
        [SerializeField] private UnityEngine.UI.Button endTurnButton;
        [SerializeField] private UnityEngine.UI.Button rollDiceButton;
        //[SerializeField] private UnityEngine.UI.Button buildButton;
        //[SerializeField] private UnityEngine.UI.Button useCardButton;
        //[SerializeField] private UnityEngine.UI.Button buyLandButton;
        
        private ITurnPhase currentPhase;
        
        private void OnEnable()
        {
            EventBus.Instance.Subscribe<TurnPhaseChangedEvent>(OnPhaseChanged);
        }
        private void OnDisable()
        {
            EventBus.Instance.Unsubscribe<TurnPhaseChangedEvent>(OnPhaseChanged);
        }

        private void OnPhaseChanged(TurnPhaseChangedEvent evt)
        {
            currentPhase = evt.CurrentPhase;
            UpdateButtonForCurrentPhase();
        }

        private void UpdateButtonForCurrentPhase()
        {
            HideAllButton();
            if (endTurnButton == null)
            {
                return;
            }

            switch (currentPhase)
            {
                case StartPhase:
                    ;
                    break;
                case WaitPhase:
                    ;
                    break;
                case RollDicePhase:
                    ShowButton(rollDiceButton);
                    break;
                case MovePhase:
                    ;
                    break;
                case PlayerPhase:
                    ShowButton(endTurnButton);
                    break;
            }
        }

        private void HideAllButton()
        {
            SetButtonActive(rollDiceButton, false);
            SetButtonActive(endTurnButton, true);
        }

        private void ShowButton(UnityEngine.UI.Button button)
        {
            SetButtonActive(button, true);
        }

        private void SetButtonActive(UnityEngine.UI.Button button,bool active)
        {
            if (button != null)
            {
                button.gameObject.SetActive(active);
            }
        }
    }
    
}