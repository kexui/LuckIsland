using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewAudioVolum", menuName="Game/AudioVolum")]
public class AudioVolum : ScriptableObject
{
    [Header("���������С")]
    [Range(0,10)]
    [SerializeField] private float masterVolume;
    [Header("SFX�ܿ�")]
    [SerializeField] private float sfxVolume;

    [Header("BGM")]
    [Range(0,10)]
    [SerializeField]private float bgmVolume;
    [Header("SFX")]
    [SerializeField] private float bgmVolum;
    [SerializeField] private float clickvolume;
    [SerializeField] private float knockupVolume;
    [SerializeField] private float knockdownVolume;
    [SerializeField] private float walkVolume;
    [SerializeField] private float diceVolume;

    //����
    public float MasterVolum => masterVolume;
    public float SFXVolume => sfxVolume;
    public float BGMVolum => bgmVolum;
    public float BuildVolume => bgmVolum;
    public float ClickVolume => clickvolume;
    public float KnockupVolume => knockupVolume;
    public float KnockdownVolume => knockdownVolume;
    public float WalkVolume => walkVolume;
    public float DiceVolume => diceVolume;
}
