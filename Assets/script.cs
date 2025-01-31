using System.Collections;
using System.Collections.Generic;
using UnityEngine;




using UnityEngine;

public class CharacterVampire : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public bool canJump = true;

    [Header("References")]
    public Animator animator;
    private Rigidbody rb;

    private Vector3 movementInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!animator) animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovementInput();
        HandleRotation();
        HandleJumpInput();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movementInput = new Vector3(horizontal, 0f, vertical).normalized;

        if (movementInput.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void HandleRotation()
    {
        if (movementInput.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveCharacter()
    {
        Vector3 move = movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("jump");
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

