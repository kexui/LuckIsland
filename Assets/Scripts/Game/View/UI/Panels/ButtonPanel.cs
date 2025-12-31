using System;
using System.Collections.Generic;
using Core.Events;
using Core.Systems;
using Game.Enums;
using Game.Events;
using Game.Managers;
using UnityEngine;

namespace Game.View.Button
{
    [Serializable]
    public class PhaseButtonConfig
    {
        public TurnPhase phase;
        public List<GameObject> buttons;
    }

    public class ButtonPanel: MonoBehaviour
    {
        [Header("按钮引用")]
        public List<PhaseButtonConfig> buttonConfigs;
        private List<GameObject> showButtons =  new List<GameObject>();
        
        //private ITurnPhase currentPhase;

        private void Start()
        {
            foreach (var buttonConfig in buttonConfigs)
            {
                foreach (var button in buttonConfig.buttons)
                {
                    button.gameObject.SetActive(false);
                }
            }
        }

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
            HideAllButton();
            ShowAllButtonByPhase(evt.CurrentPhase);
        }

        private void HideAllButton()
        {
            foreach (var button in showButtons)
            {
                if (button != null)
                {
                    button.gameObject.SetActive(false);
                }
            }
            showButtons.Clear();
        }

        /// <summary>
        /// 显示对应阶段的按钮
        /// </summary>
        /// <param name="phase"></param>
        private void ShowAllButtonByPhase(ITurnPhase phase)
        {
            foreach (var buttonConfig in buttonConfigs)
            {
                if (buttonConfig.phase == phase.Phase)
                {
                    buttonConfig.buttons.ForEach(b => ShowButton(b));
                }
            }
        }

        private void HideButton(GameObject button)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
                showButtons.Remove(button);
            }
        }

        private void ShowButton(GameObject button)
        {
            if (button != null)
            {
                button.gameObject.SetActive(true);
                showButtons.Add(button);
            }
        }
        
        
        
        // ========== 按钮方法 ==========
        public void TryRollDice()
        {
            if (GameManager.Instance?.DiceSystem != null)
            {
                GameManager.Instance.DiceSystem.RequestRollDice(GameManager.Instance.LocalPlayerId);
            }
            else
            {
                Debug.LogError("ButtonPanel:DiceSystem == null");
            }
            HideButton(this.gameObject);
        }

        public void TryEndPhase()
        {
            
        }
    }
    
}