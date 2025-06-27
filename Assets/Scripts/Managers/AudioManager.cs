using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]private AudioData audioData; // ��Ƶ����
    [SerializeField]private AudioVolum audioVolum;  //������С

    private AudioSource audioSource; // ��ƵԴ

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
    {//���Ž�������
        PlayerSFX(audioData.buildingMusic[index], audioVolum.BuildVolume);
    }
    public void PlayerRandomClickSound()
    {//���ŵ����Ч
        int index = UnityEngine.Random.Range(0, audioData.clickMusic.Length);
        PlayerClickSound(index);
    }
    public void PlayerClickSound(int index=0)
    {//������������Ч
        PlayerSFX(audioData.clickMusic[index], audioVolum.ClickVolume);
    }
    public void PlayerKnockUpSound(int index=0)
    {//���Ż�����Ч
        PlayerSFX(audioData.knockUpMusic[index], audioVolum.KnockupVolume);
    }
    public void PlayerKnockDownSound(int index = 0)
    {//���Ż�����Ч
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
    {//ѭ������������Ч
        while (true)
        {
            PlayerDiceSound(0);
            yield return new WaitForSeconds(audioData.diceMusic[0].length+0.1f);
        }
    }
    public void PlayerDiceSound(int index)
    {//����������Ч
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
    {//ѭ������������Ч
        while (true)
        {
            PlayerWalkSound(0);
            yield return new WaitForSeconds(audioData.walkMusic[0].length + 0.1f);
        }
    }
    public void PlayerWalkSound(int index)
    {//����������Ч
        PlayerSFX(audioData.walkMusic[index], audioVolum.WalkVolume);
    }

    public void PlayerSFX(AudioClip audioClip, float volumeMutipler)
    {
        audioSource.PlayOneShot(audioClip, volumeMutipler *audioVolum.SFXVolume/10f* audioVolum.MasterVolum / 10f);
    }
    public void PlayerSound(AudioClip audioClip, float volumeMutipler)
    {//������Ч
        audioSource.PlayOneShot(audioClip,volumeMutipler * audioVolum.MasterVolum/10f);
    }

}
