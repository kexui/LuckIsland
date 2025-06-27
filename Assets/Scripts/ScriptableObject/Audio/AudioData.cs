using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewAudioData",menuName ="Game/Audio")]
public class AudioData : ScriptableObject
{
    public AudioClip[] buildingMusic; // ½¨ÖşÒôÀÖ
    public AudioClip[] clickMusic; // ÒôÆµ¼ô¼­
    public AudioClip[] backgroundMusic; // ±³¾°ÒôÀÖ
    public AudioClip[] knockUpMusic;  // »÷·ÉÒôĞ§
    public AudioClip[] knockDownMusic; // »÷ÂäÒôĞ§
    public AudioClip[] diceMusic; // ÷»×ÓÒôĞ§
    public AudioClip[] walkMusic; // ĞĞ×ßÒôĞ§
}
