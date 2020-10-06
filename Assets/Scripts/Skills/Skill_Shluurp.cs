using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shluurp : BaseSkill
{
    public float StunChance = 0.6f;
    protected override void InitSkillInfo()
    {
        // Base skill info
        SkillName = "Shluurp";
        SkillID = "SKILL_SHLUURP";
        SkillDescription = "Target is dealt weak crush damage with stun chance";
        SkillType = E_SkillType.MELEE;
        SpCost = 2;
        RecoveryTime = 1f;
        DamageMod = 0.7f;
        AccuracyMod = -0.1f;
        CritMod = 0f;
    }
    public override void Activate()
    {
        base.Activate();
        owner.AttackTargetEnemy(DamageMod, AccuracyMod, CritMod, DamageVariant.MELEE);
    }
}
