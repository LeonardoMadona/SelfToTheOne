using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerCamera, playerModel;
    public float walkSpeed, jumpForce, gravityForce, attackCooldown, dashForce, dashCooldown, walkDashCooldown;

    [Range(0f, 1f)] public float rotationSmooth;

    bool walking, grounded;
    float nextAttackTime = 0f, nextDashTime = 0f, nextWalkTime = 0f;



    Vector2 walkInput;
    Rigidbody rb;
    Vector3 forwardVector;

    [SerializeField] AudioClip[] swordSwingSounds;
    AudioSource audioSrc;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        forwardVector = new Vector3(transform.position.x - playerCamera.position.x, 0f, transform.position.z - playerCamera.position.z).normalized;
        walkInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        walking = walkInput.sqrMagnitude > 0.01;

        CheckForGrounded();

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (Time.timeSinceLevelLoad > nextAttackTime)
            {
                Attack();
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Dash();
        }

        HandleAnimation();
        ApplyWalkingSpeed();
        LookTowardsMovement();
    }

    void Dash()
    {        
        if(Time.time < nextDashTime)
        {
            return;
        }

        rb.AddForce((walkInput.y * forwardVector + walkInput.x * playerCamera.right) * dashForce,ForceMode.VelocityChange);

        nextDashTime = Time.time + dashCooldown;
        nextWalkTime = Time.time + walkDashCooldown;
    }


    void ApplyWalkingSpeed()
    {     
        if (walking && Time.time >= nextWalkTime) 
        {      
            rb.velocity = walkSpeed * playerCamera.right * walkInput.x + Vector3.up * rb.velocity.y + walkSpeed * forwardVector * walkInput.y;
        }
    }

    void Attack()
    {
        nextAttackTime = Time.timeSinceLevelLoad + attackCooldown;
        anim.SetTrigger("attack");

        audioSrc.clip = swordSwingSounds[Random.Range(0, swordSwingSounds.Length)];
        audioSrc.Play();
    }

    void Jump()
    {
        grounded = false;
        anim.SetTrigger("jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    void CheckForGrounded()
    {
        Vector3 rayStart = transform.position + (Vector3.up * .05f);
        Vector3 rayEnd = rayStart - Vector3.up * (Mathf.Max(-rb.velocity.y * Time.fixedDeltaTime, .05f) + .05f);
        Debug.DrawLine(rayStart, rayEnd, Color.red);

        grounded = Physics.Raycast(rayStart, -Vector3.up, Mathf.Max(-rb.velocity.y * Time.fixedDeltaTime, .05f) + .05f) && rb.velocity.y <= .05f;
    }

    void HandleAnimation()
    {
        anim.SetBool("walking", walking);
        anim.SetBool("grounded", grounded);
    }

    void LookTowardsMovement()
    {
        Vector3 projectedSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);        

        if (projectedSpeed.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedSpeed);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, rotationSmooth);
        }
    }
}
