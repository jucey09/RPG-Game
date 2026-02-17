using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowDownPercent = 0.8f;
    [SerializeField] private float slowDownDomainDuration = 5;

    [Header("Spell Casting Upgrade")]
    [SerializeField] private int spellsToCast = 10;
    [SerializeField] private float spellcastingDomainSlowDowm = 1;
    [SerializeField] private float spellCastingDomainDuration = 8;
    private float spellCastTimer;
    private float spellPerSecond;

    [Header("Domain Details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 2;

    private List<Enemy> trappedTragets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateDomain()
    {
        spellPerSecond = spellsToCast / GetDomainDuration();

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }
    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if(currentTarget == null)
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
        if(upgradeType == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(1,0) : new Vector2(-1,0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }

        if(upgradeType == SkillUpgradeType.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        if(trappedTragets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTragets.Count);
        Transform target = trappedTragets[randomIndex].transform;

        if(target == null)
        {
            trappedTragets.RemoveAt(randomIndex);
            return null;
        }

        return target;
    }

    public float GetDomainDuration()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDownDomainDuration;
        else
            return spellCastingDomainDuration;
    }

    public float GetSlowPercentage()
    {
        if(upgradeType == SkillUpgradeType.Domain_SlowingDown)
            return slowDownPercent;
        else
            return spellcastingDomainSlowDowm;
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
        foreach(var enemy in trappedTragets)
            enemy.StopSlowDown();

        trappedTragets = new List<Enemy>();
    }
}
