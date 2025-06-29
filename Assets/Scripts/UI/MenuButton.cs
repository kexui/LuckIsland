using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator;
    public UnityEvent onClick; // �¼�����
    public int musiceIndex;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
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
        animator.SetTrigger("Click");
        AudioManager.Instance.PlayerClickSound(musiceIndex);
        //�¼�
        onClick?.Invoke();
    }
}


