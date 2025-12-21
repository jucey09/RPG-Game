using UnityEngine;

public class Player_Combat : Entity_Combat
{
    private Entity_Combat entity_Combat;

    [Header("Counter attack details")]
    [SerializeField] private float CounterRecovery = 1;
    private void Awake()
    {
        entity_Combat = GetComponent<Entity_Combat>();
    }
    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable == null)
                continue;

            if (counterable.CanBeCountered)
            {
                counterAttackDamage();
                counterable.HandleCounter();
                hasPerformedCounter = true;
            }
        }
        return hasPerformedCounter;
    }
    public float GetCounterRecoveryDurtaion() => CounterRecovery;

    public void counterAttackDamage()
    {
        entity_Combat.PerformAttack();
    }
}
