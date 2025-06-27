using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]private AudioData audioData; // 音频数据
    [SerializeField]private AudioVolum audioVolum;  //声音大小

    private AudioSource audioSource; // 音频源

    private Coroutine walkSoundCoroutine;
    private Coroutine rollDiceSoundCoroutine;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        audioSource.Play();
    }


    public void PlayerBuildSound(int index =0)
    {//播放建筑音乐
        PlayerSFX(audioData.buildingMusic[index], audioVolum.BuildVolume);
    }
    public void PlayerRandomClickSound()
    {//播放点击音效
        int index = UnityEngine.Random.Range(0, audioData.clickMusic.Length);
        PlayerClickSound(index);
    }
    public void PlayerClickSound(int index=0)
    {//播放随机点击音效
        PlayerSFX(audioData.clickMusic[index], audioVolum.ClickVolume);
    }
    public void PlayerKnockUpSound(int index=0)
    {//播放击飞音效
        PlayerSFX(audioData.knockUpMusic[index], audioVolum.KnockupVolume);
    }
    public void PlayerKnockDownSound(int index = 0)
    {//播放击落音效
        PlayerSFX(audioData.knockDownMusic[index], audioVolum.KnockdownVolume);
    }

    public void StartPlayerRollDiceSound()
    {
        if (rollDiceSoundCoroutine!=null)
            StopCoroutine(rollDiceSoundCoroutine);
        rollDiceSoundCoroutine = StartCoroutine(LoopPlayerRollDiceSound());
    }
    public void StopPlayerRollDiceSound()
    {
        if (rollDiceSoundCoroutine!=null)
            StopCoroutine(rollDiceSoundCoroutine);
        rollDiceSoundCoroutine = null;
    }
    public IEnumerator LoopPlayerRollDiceSound()
    {//循环播放骰子音效
        while (true)
        {
            PlayerDiceSound(0);
            yield return new WaitForSeconds(audioData.diceMusic[0].length+0.1f);
        }
    }
    public void PlayerDiceSound(int index)
    {//播放骰子音效
        PlayerSFX(audioData.diceMusic[index], audioVolum.DiceVolume);
    }

    public void StartPlayerWalkSound()
    {
        if (walkSoundCoroutine!=null)
        {
            StopCoroutine(walkSoundCoroutine);
        }
        walkSoundCoroutine = StartCoroutine(LoopPlayerWalkSound());
    }
    public void StopPlayerWalkSound()
    {
        if (walkSoundCoroutine!=null)
            StopCoroutine(walkSoundCoroutine);
        walkSoundCoroutine = null;
    }
    public IEnumerator LoopPlayerWalkSound()
    {//循环播放行走音效
        while (true)
        {
            PlayerWalkSound(0);
            yield return new WaitForSeconds(audioData.walkMusic[0].length + 0.1f);
        }
    }
    public void PlayerWalkSound(int index)
    {//播放行走音效
        PlayerSFX(audioData.walkMusic[index], audioVolum.WalkVolume);
    }

    public void PlayerSFX(AudioClip audioClip, float volumeMutipler)
    {
        audioSource.PlayOneShot(audioClip, volumeMutipler *audioVolum.SFXVolume/10f* audioVolum.MasterVolum / 10f);
    }
    public void PlayerSound(AudioClip audioClip, float volumeMutipler)
    {//播放音效
        audioSource.PlayOneShot(audioClip,volumeMutipler * audioVolum.MasterVolum/10f);
    }

}
