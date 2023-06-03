using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float comboResetTimer = 1.5f;

    private float lastClickTime;
    private int numClicks;
    private bool isAttacking;
    private Vector2 moveInput;

    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    void Update()
    {
        Movement();
        AttackComboSystem();
    }

    private void Movement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);

        if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    private void AttackComboSystem()
    {
        if (lastClickTime > 0)
            lastClickTime -= Time.deltaTime;
        else if (lastClickTime <= 0)
            numClicks = 0;

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                lastClickTime = comboResetTimer;
                numClicks++;
                StartCoroutine(AttackingRoutine());
            }
        }
    }

    private IEnumerator AttackingRoutine()
    {
        if (numClicks == 1)
        {
            animator.SetTrigger("attack1");
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }
        else if (numClicks == 2)
        {
            animator.SetTrigger("attack2");
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }
        else if (numClicks >= 3)
        {
            numClicks = 0;
            animator.SetTrigger("attack3");
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }
    }
}
