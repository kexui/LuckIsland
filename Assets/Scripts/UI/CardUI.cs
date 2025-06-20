using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Image Faces;
    public Image Frames;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI effectText;

    public void SetData(CardDataBase data)
    { 
        //cardName.text = data.cardName;
    }
}

