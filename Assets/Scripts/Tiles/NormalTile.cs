using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : TileBase
{//��ͨ����
    public override IEnumerator TriggerEvent(BasePlayerController pc)
    {
        Debug.Log("���·���");
        yield break;
    }
}
