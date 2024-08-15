using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class LaserObj : MonoBehaviour
{

    VisualEffect vfx;
    AudioSource audioSrc;
    [SerializeField] AudioClip[] laserAudios;
    public UnityEvent OnHitPlayer;
    bool hasHitPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        StartCoroutine("LaserBehaviour");
    }

    IEnumerator LaserBehaviour()
    {
        hasHitPlayer = false;
        vfx.Play();

        yield return new WaitForSeconds(1.5f);

        audioSrc.clip = laserAudios[Random.Range(0, laserAudios.Length)];
        audioSrc.Play();

        float endCollisionTime = Time.time + .75f;

        while(Time.time < endCollisionTime)
        {
            if(!hasHitPlayer)
                TestPlayerCollision();

            yield return null;
        }

        gameObject.SetActive(false);
    }

    void TestPlayerCollision()
    {
        int layermask = LayerMask.GetMask("Player");

        if(Physics.CheckCapsule(transform.position, transform.position + Vector3.up * 3f, 1f, layermask))
        {
            hasHitPlayer = true;
            OnHitPlayer.Invoke();
        }

    }
}
