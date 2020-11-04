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
        SkillDescription = "Target is dealt weak melee damage";
        SkillType = E_SkillType.MELEE;
        SpCost = 2;
        RecoveryTime = 1f;
        DamageMod = 0.8f;
        AccuracyMod = -0.1f;
        CritMod = 0f;
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        owner.AttackTargetEnemy(DamageMod, AccuracyMod, CritMod, DamageVariant.MELEE);
    }
}
