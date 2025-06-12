using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BuildableTile : TileBase
{//����
    int baseCopper = 1;//��������
    int purchaseCopper = 5;//�������
    BasePlayerController owner;
    bool isOwned = false;//�Ƿ�ӵ��

    private void Awake()
    {
        BuildableLand land = FindLinker<BuildableLand>(transform);
        if (land==null)
        {
            Debug.Log("FindLinkerΪ��");
        }
        else
        {
            linkLand = land;
        }
    }
    public override void TriggerEvent(BasePlayerController pc)
    {
        if (isOwned)
        {//����ӵ��
            if (owner == pc)
            {//�Լ�ӵ��
                print("������Ĳ���");
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
            if (pc.playerData.SubtractCopper(purchaseCopper))
            {//PC��ҹ�
                isOwned = true;
                owner = pc;
                print("�㹺�����������");
            }
            else
            {
                print("��û���㹻��ͭ�ң��޷�����");
            }
        }
    }
    


}
