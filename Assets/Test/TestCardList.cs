using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


[Serializable]
[XmlRoot("Cards")]//外层标签 类
public class TestCardList
{
    [XmlElement("Card")]//子项标签 列表
    public List<TestCard> cards;
}
