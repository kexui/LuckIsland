using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnimator : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator;
    public MenuStage menuStage;
    public MenuManager menuManager;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        animator.SetTrigger("Pressed");
        menuManager.OnClick(menuStage);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hover", false);
    }
}


