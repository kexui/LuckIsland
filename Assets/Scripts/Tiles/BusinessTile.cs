using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BusinessTile : TileBase
{//����
    int baseCopper = 1;//��������
    int purchaseCopper = 5;//�������
    BasePlayerController owner;
    bool isOwned = false;//�Ƿ�ӵ��
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
    public void ChangeLand()
    {//�д�˼��
        LandBase go = FindLinker<LandBase>();
        //if (go != null) ;

    }
}
