using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoItemUI : MonoBehaviour
{
    public Image avatarImage;
    public TextMeshProUGUI idText;
    public TextMeshProUGUI copperText;
    public TextMeshProUGUI landText;

    private PlayerData data;

    public void SetData(PlayerData playerData)
    { 
        data = playerData;

    }
    public void RefreshUI()
    { 
        idText.text = "ID:"+data.playerName;
        copperText.text = "Copper" + data.Copper;
        landText.text = "ฒ๚ตุ"+data.ownedTiles;
    }
}
