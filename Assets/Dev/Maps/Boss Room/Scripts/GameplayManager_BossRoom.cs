using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;


public class GameplayManager_BossRoom : MonoBehaviour
{
    public Animator canvasAnim, playerAnim;
    public float delayBeforeFadeIn, delayBeforeMapTransition;
    public PlayableDirector director;

    public PlayerMovement playerMovement;
    public CameraOrbitalBehaviour cameraMovement;

    public UnityEvent OnStartBossFight, OnCloseBossDialog, OnBossDeath, OnFinalTransition;

    Jukebox jukeboxScript;

    public Material sceneMaterial;
    public TimelineAsset outroTimeline;

    public Material blackSkybox;
    public Material newSkyboxMaterial;

    IEnumerator Start()
    {
        sceneMaterial.SetFloat("_Mask_Inside", 0f);
        RenderSettings.skybox = blackSkybox;
        TogglePlayerControl(false);

        jukeboxScript = GameObject.FindFirstObjectByType<Jukebox>();

        yield return new WaitForSeconds(delayBeforeFadeIn);
        canvasAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(delayBeforeMapTransition);
        director.Play();
    }

    public void TogglePlayerControl(bool toggle)
    {        
        playerAnim.SetBool("walking", false);
        playerAnim.SetBool("grounded", true);
        playerMovement.enabled = toggle;
        cameraMovement.enabled = toggle;
    }

    public void StartBossFight()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnStartBossFight?.Invoke();
    }

    public void CloseBossDialog()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        OnCloseBossDialog.Invoke();

        if(jukeboxScript != null) 
            jukeboxScript.FadeOutToClip(2);
    }

    public void HandleBossDeath()
    {
        OnBossDeath?.Invoke();
        RenderSettings.skybox = newSkyboxMaterial;
        jukeboxScript?.FadeOutToClip(1);
        StartCoroutine(WaitAndPlayDirector(5f));
    }

    IEnumerator WaitAndPlayDirector(float delay)
    {
        yield return new WaitForSeconds(delay);

        director.playableAsset = outroTimeline;
        sceneMaterial.SetFloat("_Mask_Inside", 1f);
        director.Play();
    }

    public void FinalScreen()
    {
        canvasAnim.SetTrigger("finalScreen");
    }

    public void FinalTransitionHandler()
    {
        OnFinalTransition.Invoke();
    }

}
