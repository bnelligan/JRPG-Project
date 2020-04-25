using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple melee attack for 80% of melee damage
/// </summary>
public class Skill_Strike : BaseSkill
{
    protected override void InitSkillInfo()
    {
        // Base skill info
        SkillName = "Strike!";
        SkillID = "SKILL_STRIKE";
        SkillDescription = "Deal weak melee damage to target enemy";
        SkillType = E_SkillType.MELEE;
        SpCost = 1;
        RecoveryTime = 1f;
        DamageMod = 1f;
        AccuracyMod = -0.1f;
        CritMod = 0f;
    }
    public override void Activate()
    {
        owner.AttackTargetEnemy(DamageMod, AccuracyMod, CritMod, DamageVariant.MELEE);
        base.Activate();
    }
}
