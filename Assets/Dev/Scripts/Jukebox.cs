using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioClips;
    AudioSource audioSource;

    public float fadeDuration;

    static Jukebox instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        //make this persistent between scenes.
        DontDestroyOnLoad(gameObject);

        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    public void FadeOutToClip(int clipIndex)
    {
        StartCoroutine(SwapClipsWithFade(clipIndex));
    }

    IEnumerator SwapClipsWithFade(int index)
    {
        float startTime = Time.time;
        float endTime = startTime + fadeDuration;

        while (Time.time < endTime) 
        {
            audioSource.volume = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;

        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
