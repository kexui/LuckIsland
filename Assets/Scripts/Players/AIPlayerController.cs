using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerController : BasePlayerController
{
    public override IEnumerator RollDice()
    {
        hasDiceRolled = true;
        Roll();
        yield return new WaitForSeconds(0.1f);
    }
    public override IEnumerator TriggerTileEvent()
    { //����Tile�¼�
        TileManager.Instance.TriggerEvent(playerData.CurrentTileIndex, this);
        yield return new WaitForSeconds(0.1f);//���ɶ����¼�����ʱ�����
    }
}
