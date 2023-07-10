using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource SoundFXObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(SoundFXObject, spawnTransform.position, Quaternion.identity);

        //assign audioClip
        audioSource.clip = audioClip;

        //play volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get lenght of the sound FX clip
        float clipLenght = audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLenght);
    }
}
