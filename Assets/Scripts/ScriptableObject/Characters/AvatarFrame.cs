using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewAvatarFrame", menuName = "Game/AvatarFrame")]
public class AvatarFrame : ScriptableObject
{
    public List<Sprite> avatarFrames;
}
