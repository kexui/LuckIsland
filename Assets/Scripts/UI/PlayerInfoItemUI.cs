using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoItemUI : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private Image frame;
    [SerializeField] private AvatarFrame avatarFrame;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI luckText;

    public TextMeshProUGUI copperText;
    public TextMeshProUGUI landText;

    private PlayerData data;

    public void SetData(PlayerData playerData)
    { 
        data = playerData;
        frame.sprite = avatarFrame.avatarFrames[playerData.ID];
        avatar.color = PlayerColorData.GetColor(data.ID);
        RefreshUI();
    }
    public void RefreshUI()
    {//Ë¢ÐÂUI
        nameText.text = data.PlayerName;
        copperText.text = data.Copper.ToString();
        //landText.text = "²úµØ"+data.ownedTiles;
        luckText.text = data.Luck.ToString();
    }
}
