using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator;
    public UnityEvent onClick; // 事件触发

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
        //事件
        onClick?.Invoke();
    }
}
