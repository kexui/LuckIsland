using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Xml
{
    public static TestCardList LoadCardsFromXml(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TestCardList));
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            return (TestCardList)serializer.Deserialize(stream) as TestCardList;
        }
    }

}
