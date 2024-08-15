using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class VoidSceneGameplayController : MonoBehaviour
{

    [SerializeField] Animator canvasAnim;
    public float delayToCloseScreen, delayToShowCube;
    Jukebox jukebox;

    [SerializeField] GameObject bodyCube, cubeModel;
    [SerializeField] VisualEffect cubeParticle;
    [SerializeField] Transform player;
    [SerializeField] Vector3 cubeOffsetToPlayer;

    IEnumerator Start()
    {
        jukebox = GameObject.FindFirstObjectByType<Jukebox>();
        yield return new WaitForSeconds(delayToCloseScreen);

        canvasAnim.SetTrigger("closeCanvas");

        yield return new WaitForSeconds(delayToShowCube);

        bodyCube.transform.position = player.position + cubeOffsetToPlayer;
        bodyCube.SetActive(true);
    }

    public void GrabCubeHandle()
    {
        bodyCube.GetComponent<AudioSource>().Play();
        cubeParticle.Play();
        Destroy(cubeModel);
        canvasAnim.SetTrigger("canvasOutro");
    }

    public void LoadNextScene()
    {
        jukebox.FadeOutToClip(1);
        SceneManager.LoadSceneAsync(1);
    }
}
