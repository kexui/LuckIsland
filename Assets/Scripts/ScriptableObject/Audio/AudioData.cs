using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewAudioData",menuName ="Game/Audio")]
public class AudioData : ScriptableObject
{
    public AudioClip[] buildingMusic; // ��������
    public AudioClip[] clickMusic; // ��Ƶ����
    public AudioClip[] backgroundMusic; // ��������
    public AudioClip[] knockUpMusic;  // ������Ч
    public AudioClip[] knockDownMusic; // ������Ч
    public AudioClip[] diceMusic; // ������Ч
    public AudioClip[] walkMusic; // ������Ч
}
