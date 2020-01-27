using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Guard : BaseSkill
{
    protected override void InitSkillInfo()
    {
        // Base skill info
        SkillName = "Guard";
        SkillID = "SKILL_GUARD";
        SkillDescription = "Gain 1 temporary armor";
        SkillType = E_SkillType.UTILITY;
        SpCost = 1;
        RecoveryTime = 0.75f; // Reduced recovery time
        DamageMod = 0f;
        AccuracyMod = 0f;
        CritMod = 0f;
    }
    public override void Activate()
    {
        owner.GainArmor();
        base.Activate();
    }
    public override bool CanActivate()
    {
        return base.CanActivate() && (owner.Armor < owner.HP_Max);
    }
}
