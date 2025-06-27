using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class BuildableTile : TileBase
{//����
    int baseCopper = 1;//��������
    int purchaseCopper = 5;//�������
    int UpgradeCopper = 3;//��������
    BasePlayerController owner;
    BasePlayerController currentPlayer;
    bool isOwned = false;//�Ƿ�ӵ��
    bool isUpgraded = false;//�Ƿ�����

    BuildableLand neighborLand;

    protected override void Start()
    {
        base.Start();
        BuildableLand land = FindNeighbor<BuildableLand>(transform.position);
        if (land==null)
        {
            Debug.Log("BuildableTile��FindLinkerΪ��");
        }
        else
        {
            neighborLand = land;
        }
    }
    public override IEnumerator TriggerEvent(BasePlayerController pc)
    {
        currentPlayer = pc;
        if (isOwned)
        {//����ӵ��
            if (owner == pc)
            {//�Լ�ӵ��
                if (isUpgraded) yield break;
                if (GameManager.Instance.LocalPlayer != pc) yield break;
                WorldSpaceUI.Instance.buildPromptUI.Show(transform.position, UpgradeBusiness);
            }
            else
            {//����ӵ��
                if (pc.playerData.SubtractCopper(baseCopper))
                {//PC��ҹ�
                    owner.playerData.AddCopper(baseCopper);
                    print("�㱻��ȡ��" + baseCopper + "ͭ��");
                }
                else
                {
                    print("��û���㹻��ͭ�ң��޷�֧��");
                    //��
                }
            }
        }
        else
        {//û��ӵ��
            if (GameManager.Instance.LocalPlayer != pc) yield break;
            WorldSpaceUI.Instance.buildPromptUI.Show(transform.position,SpawnBusiness); //��ʾ����UI
        }
        yield return null;
    }
    void SpawnBusiness()
    { //����
        if (currentPlayer.playerData.SubtractCopper(purchaseCopper))
        {//Ǯ��
            owner = currentPlayer;
            isOwned = true;
            print("�㹺����"  + "��������" + purchaseCopper + "ͭ��");
            neighborLand.SpawmBusiness();
        }
        else
        {//Ǯ����
            print("��û���㹻��ͭ�ң��޷�����");
            return;
        }
    }
    void UpgradeBusiness()
    {
        if (currentPlayer.playerData.SubtractCopper(UpgradeCopper))
        {
            print("�������˽���" + "��������" + UpgradeCopper + "ͭ��");
            isUpgraded = true;
            neighborLand.UpgradeBusiness();
        }
        else
        {
            print("��û���㹻��ͭ�ң��޷�����");
            return;
        }
    }
}
