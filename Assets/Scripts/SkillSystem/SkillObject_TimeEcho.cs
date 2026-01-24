using UnityEngine;
using UnityEngine.XR;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;
    private bool shouldMoveToPlayer;

    private Transform playerTransform;
    private Skill_TimeEcho echoManager;
    private TrailRenderer wispTrail;

    public int maxAttacks { get; private set; }

    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxAttacks = echoManager.GetMaxAttacks();
        playerTransform = echoManager.transform.root;

        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());
        FlipToTarget();

        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxAttacks > 0);
    }

    private void Update()
    {
        if (shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }


    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < 0.5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void HandlePlayerTouch()
    {

    }

    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void performAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if (targetGotHit == false)
            return;

        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0, 0));
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);

        if (echoManager.ShouldBeWisp())
        {
            shouldMoveToPlayer = true;
            anim.gameObject.SetActive(false);
            wispTrail.gameObject.SetActive(true);
            rb.simulated = false;
        }
        else
            Destroy(gameObject);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

    }
}
