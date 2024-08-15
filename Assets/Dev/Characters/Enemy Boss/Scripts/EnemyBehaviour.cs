using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{

    public GameObject[] lasers;
    public Rigidbody[] projectiles;

    [SerializeField] Collider laserSpawnArea;
    [SerializeField] float timeBetweenLasers, timeBetweenProjectiles, projectileSpeed, playerImpulseForce;
    [SerializeField] Transform playerLookTarget;
    [SerializeField] Transform eyeModel;
    [SerializeField] LayerMask laserSearchMask;
    [SerializeField] Slider healthBar;
    [SerializeField] AudioClip hitSound, shootSound;

    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject invulnerableVfx;

    public UnityEvent OnBossDeath;

    bool vulnerable = true;
    public int baseHealth = 20;
    int currentHealth;
    AudioSource audioSrc;
    Rigidbody playerRb;

    enum fightPhase
    {
        first, 
        second
    } 

    fightPhase currentPhase = fightPhase.first;


    public void BeginFight()
    {
        StartCoroutine(FightBehaviourFirstPhase());
        currentHealth = baseHealth;
        audioSrc = GetComponent<AudioSource>();
        playerRb = playerTransform.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        //Always look at player
        eyeModel.LookAt(playerLookTarget.position);
    }

    public IEnumerator FightBehaviourFirstPhase()
    {
        while (currentPhase == fightPhase.first)
        {
            StartCoroutine(UseLasers());

            yield return new WaitForSeconds(timeBetweenLasers * (lasers.Length - 1) + 5f);
        }        
    }

    public IEnumerator FightBehaviourSecondPhase()
    {
        invulnerableVfx.SetActive(true);

        vulnerable = false;
        SendPlayerFlying();

        yield return new WaitForSeconds(3f);

        vulnerable = true;
        invulnerableVfx.SetActive(false);

        while (currentHealth > 0)
        {
            StartCoroutine(Projectiles());

            float targetTime = Time.time + timeBetweenProjectiles * (projectiles.Length - 1) + 4f;

            while (Time.time < targetTime)
            {
                if (currentHealth <= 0)
                {
                    break;
                }

                yield return null;
            }

            if (currentHealth <= 0)
                break;

            StartCoroutine (UseLasers());

            targetTime = Time.time + timeBetweenLasers * (lasers.Length - 1) + 2f;

            while (Time.time < targetTime)
            {
                if(currentHealth <= 0)
                { 
                    break; 
                }

                yield return null;
            }

            if (currentHealth <= 0)
                break;

            invulnerableVfx.SetActive(true);

            vulnerable = false;
            SendPlayerFlying();

            yield return new WaitForSeconds(3f);

            vulnerable = true;
            invulnerableVfx.SetActive(false);
        }
    }

    void SendPlayerFlying()
    {
        playerRb.AddForce(((playerTransform.position - transform.position).normalized + Vector3.up).normalized * playerImpulseForce, ForceMode.VelocityChange);
    }

    public void TakeHit()
    {
        Debug.Log("Enemy Hit");

        if (!vulnerable)
        {
            //som de tomar hit fraco
            return;
        }

        currentHealth -= 1;

        audioSrc.clip = hitSound;
        audioSrc?.Play();

        healthBar.value = (float)currentHealth / baseHealth;

        if(currentHealth <= baseHealth / 2 && currentPhase == fightPhase.first)
        {
            currentPhase = fightPhase.second;
            StartCoroutine(FightBehaviourSecondPhase());
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("Boss died");
        OnBossDeath.Invoke();
    }

    public IEnumerator UseLasers()
    {
        Debug.Log("Spawning Lasers");

        bool spawnedOnPlayer = false;

        foreach (var l in lasers)
        {
            Vector3 laserSpawnPosition = Vector3.zero;
            bool foundSpawn = false;    

            while (!foundSpawn)
            {
                Vector3 randomPos;
                randomPos.x = Random.Range(laserSpawnArea.bounds.min.x, laserSpawnArea.bounds.max.x);
                randomPos.y = Random.Range(laserSpawnArea.bounds.min.y, laserSpawnArea.bounds.max.y);
                randomPos.z = Random.Range(laserSpawnArea.bounds.min.z, laserSpawnArea.bounds.max.z);

                RaycastHit hit;

                if (Physics.Raycast(randomPos, Vector3.down, out hit, 20f, laserSearchMask))
                {
                    if (hit.normal.y > .5f)
                    {
                        foundSpawn = true;
                        laserSpawnPosition = hit.point;
                    }
                }
            }

            if (!spawnedOnPlayer)
            {
                spawnedOnPlayer = true;
                laserSpawnPosition.x = playerTransform.position.x;
                laserSpawnPosition.z = playerTransform.position.z;
            }

            l.transform.position = laserSpawnPosition;
            l.SetActive(true);

            yield return new WaitForSeconds(timeBetweenLasers);
        }        
    }

    public IEnumerator Projectiles()
    {
        Debug.Log("Spawning Projectiles");

        Vector2 randomDir = Random.insideUnitCircle;
        Vector3 currentDir = new Vector3(randomDir.x, 0f, randomDir.y);

        foreach (var p in projectiles)
        {
            p.transform.position = eyeModel.position - Vector3.up * 1f;
            p.gameObject.SetActive(true);
            p.velocity = currentDir.normalized * projectileSpeed;

            audioSrc.clip = shootSound;
            audioSrc.Play();

            currentDir = Quaternion.Euler(0f, (360f / projectiles.Length) * 2f, 0f) * currentDir;

            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
    }
}
