using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeOverTime : MonoBehaviour
{

    public TMP_Text text;
    [TextArea]
    public string[] textsToType;
    public bool typeFirstTextOnStart;

    [SerializeField] float initialDelay, delayBetweenCharacters;

    [SerializeField] bool playSoundsWhenTyping;
    [SerializeField] AudioClip[] typeSounds;
    AudioSource audioSrc;

    private void Awake()
    {
        if (typeFirstTextOnStart)
        {
            TypeTextOverTime(0);
        }

        if(playSoundsWhenTyping)
        {
            audioSrc = gameObject.AddComponent<AudioSource>();
            audioSrc.volume = .5f;
            audioSrc.playOnAwake = false;
        }
    }

    public void TypeTextOverTime(int textIndex)
    {
        Debug.Log("called type");
        StartCoroutine(DelayedTyping(textsToType[textIndex]));
    }

    public void ClearText()
    {
        text.text = "";
    }

    IEnumerator DelayedTyping(string s)
    {
        text.text = string.Empty;

        if(initialDelay > 0f)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        for(int i = 0; i < s.Length; i++) 
        {
            text.text += s[i];   

            if(playSoundsWhenTyping)
            {
                audioSrc.clip = typeSounds[Random.Range(0, typeSounds.Length)];
                audioSrc.Play();
            }

            yield return new WaitForSeconds(delayBetweenCharacters);

            if (s[i] == ',' || s[i] == '.')
            {
                //extra pause for commas and periods.
                yield return new WaitForSeconds(2 * delayBetweenCharacters);
            }
        }
    }


}
