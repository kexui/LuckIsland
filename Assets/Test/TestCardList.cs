using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


[Serializable]
[XmlRoot("Cards")]//����ǩ ��
public class TestCardList
{
    [XmlElement("Card")]//�����ǩ �б�
    public List<TestCard> cards;
}
