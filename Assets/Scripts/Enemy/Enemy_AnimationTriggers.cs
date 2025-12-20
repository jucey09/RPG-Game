using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
    private Enemy enemy;
    private Enemy_VFX enemyvfx;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyvfx  = GetComponentInParent<Enemy_VFX>();
    }
    private void EnableCounterWindow()
    {
        enemyvfx.EnableAttackAlert(true);
        enemy.EnableCounterWindow(true);
    }

    private void DisableCounterWindow()
    { 
        enemyvfx.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);
    }
}
