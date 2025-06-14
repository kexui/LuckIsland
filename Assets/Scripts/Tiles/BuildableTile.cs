using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BuildableTile : TileBase
{//����
    int baseCopper = 1;//��������
    int purchaseCopper = 5;//�������
    int UpgradeCopper = 3;//��������
    BasePlayerController owner;
    BasePlayerController currentPlayer;
    bool isOwned = false;//�Ƿ�ӵ��

    BuildableLand neighborLand;

    private void Awake()
    {
        
    }
    private void Start()
    {
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
    public override void TriggerEvent(BasePlayerController pc)
    {
        currentPlayer = pc;
        if (isOwned)
        {//����ӵ��
            if (owner == pc)
            {//�Լ�ӵ��
                BuildPromptUI.Instance.Show(transform.position, UpgradeBusiness);
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
                }
            }
        }
        else
        {//û��ӵ��
            BuildPromptUI.Instance.Show(transform.position, SpawnBusiness);
        }
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
            neighborLand.UpgradeBusiness();
        }
        else
        {
            print("��û���㹻��ͭ�ң��޷�����");
            return;
        }
    }
}
