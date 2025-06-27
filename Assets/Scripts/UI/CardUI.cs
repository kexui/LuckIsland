using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private Image Faces;//卡面
    [SerializeField] private Image Frames;//框
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI effectText;
    public CardDataBase cardData { get; private set; }
    public int Index { get; private set; }
    
    private Animator animator;
    AnimationClip[] clips;
    private float timer;

    public HandPanelUI handCardPanel;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
        //获取动画剪辑
        clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Card_Click")
            {
                timer = clip.length; //获取动画时长
                break;
            }
            //Debug.LogWarning("Card_Click animation not found in animator on " + gameObject.name);
        }
    }

    public void SetData(int index, CardDataBase data,HandPanelUI hand)
    {
        Index = index;
        cardData = data;
        handCardPanel = hand;
        Init();
    }
    private void Init()
    { 
        Faces.sprite = cardData.FaceImage;
        Frames.sprite = cardData.GetFrameByRarity(cardData.Rarity);
        cardName.text = cardData.CardName;
        effectText.text = cardData.EffectText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hover", true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hover", false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (TurnManager.Instance.IsLocalPlayerTurn)
        {//是出牌阶段并且是自己回合
            animator.SetTrigger("Click");
            TimerUtility.Instance.StartTimer(0.15f, () =>
            {
                cardData.UseCard(GameManager.Instance.LocalPlayer);
                handCardPanel.RemoveCard(this);
            });
        }
        else
        {
            Debug.Log("未到你的回合");
        }
    }
}

