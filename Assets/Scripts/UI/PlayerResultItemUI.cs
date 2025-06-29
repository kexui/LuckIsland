using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerResultItemUI : MonoBehaviour
{
    [SerializeField] private AvatarFrame avatarFrame;
    [SerializeField] private Image rankIcon;
    [SerializeField] private Image playerAvatar;
    [SerializeField] private Image playerFrame;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI landText;

    private void Start()
    {
        rankIcon.gameObject.SetActive(false);
    }
    public void PlayerResult(PlayerData playerData)
    {
        playerFrame.sprite = avatarFrame.avatarFrames[playerData.ID];
        idText.text = "ID:"+playerData.ID.ToString();
        nameText.text = playerData.PlayerName;
        coinText.text = playerData.Copper.ToString();
        landText.text = playerData.ownedTiles.Count.ToString();
    }
}
