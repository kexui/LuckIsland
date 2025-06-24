using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{//定义可交互接口

    void Interact(BasePlayerController player);
}