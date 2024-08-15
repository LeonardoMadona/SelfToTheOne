using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealthHandle : MonoBehaviour
{
    int health = 3;
    public UnityEvent OnDeath;

    [SerializeField] AudioClip deathSound, hitSound;

    AudioSource audioSrc;

    [SerializeField] Material playerMat;
    Color baseColor;

    bool vulnerable = true;

    public TMP_Text lifeText;

    private void Start()
    {
        lifeText.text = "";

        for (int i = 0; i < health; i++)
        {
            lifeText.text += "♥";
        }
        baseColor = playerMat.GetColor("_BaseColor");
        audioSrc = GetComponent<AudioSource>();
    }

    public void TakeHit()
    {
        if(!vulnerable) 
        { 
            return;        
        }

        Debug.Log("Player hit");

        health -= 1;

        lifeText.text = "";

        for(int i = 0; i < health; i++) 
        {
            lifeText.text += "♥";
        }

        StartCoroutine(DamageFlash());

        if(health <= 0)
        {
            Death();
        }
        else
        {
            audioSrc.clip = hitSound;
            audioSrc.Play();
        }
    }

    IEnumerator DamageFlash()
    {
        playerMat.SetColor("_BaseColor", Color.red);
        yield return new WaitForSeconds(.3f);
        playerMat.SetColor("_BaseColor", baseColor);
    }

    void Death()
    {
        vulnerable = false;
        audioSrc.clip = deathSound;
        audioSrc.Play();
        OnDeath.Invoke();
        FindFirstObjectByType<Jukebox>().FadeOutToClip(0);

        SceneManager.LoadScene(0);
    }
}
