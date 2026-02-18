using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowDownPercent = 0.8f;
    [SerializeField] private float slowDownDomainDuration = 5;

    [Header("Shard Cast Upgrade")]
    [SerializeField] private int shardsToCast = 10;
    [SerializeField] private float shardCastDomainSlow = 1;
    [SerializeField] private float shardCastDomainDuration = 8;
    private float spellCastTimer;
    private float spellPerSecond;

    [Header("Time Echo Cast Upgrade")]
    [SerializeField] private int echoToCast = 8;
    [SerializeField] private float echoCastDomainSlow = 1;
    [SerializeField] private float echoCastDomainDuration = 6;

    [Header("Domain Details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 2;

    private List<Enemy> trappedTragets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateDomain()
    {
        spellPerSecond = GetSpellsToCast() / GetDomainDuration();

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }
    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindTargetInDomain();

        if (currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1 / spellPerSecond;
            currentTarget = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }

        if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        trappedTragets.RemoveAll(target => target == null || target.health.isDead);

        if(trappedTragets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTragets.Count);
        return trappedTragets[randomIndex].transform;
    }

    public float GetDomainDuration()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDownDomainDuration;
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardCastDomainDuration;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoCastDomainDuration;

        return 0;
    }

    public float GetSlowPercentage()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDownPercent;
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardCastDomainSlow;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoCastDomainSlow;

        return 0;
    }

    private int GetSpellsToCast()
    {
        if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
            return shardsToCast;
        else if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
            return echoToCast;

        return 0;
    }

    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_EchoSpam
        && upgradeType != SkillUpgradeType.Domain_ShardSpam;
    }

    public void AddTarget(Enemy target)
    {
        trappedTragets.Add(target);
    }

    public void ClearTargets()
    {
        foreach (var enemy in trappedTragets)
            enemy.StopSlowDown();

        trappedTragets = new List<Enemy>();
    }
}
