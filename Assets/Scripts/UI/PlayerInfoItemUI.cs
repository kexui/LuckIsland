using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoItemUI : MonoBehaviour
{
    public Image avatarImage;
    public TextMeshProUGUI idText;
    public TextMeshProUGUI luckText;

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
        copperText.text = data.Copper.ToString();
        //landText.text = "����"+data.ownedTiles;
        luckText.text = data.Luck.ToString();
    }
}
