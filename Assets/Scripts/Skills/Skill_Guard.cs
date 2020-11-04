using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Guard : BaseSkill
{
    public uint ArmorGain = 2;
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
    public override void ActivateSkill()
    {
        ownerStats.GainArmor(ArmorGain);
        base.ActivateSkill();
    }
    public override bool CanActivate()
    {
        return base.CanActivate() && (ownerStats.Armor < ownerStats.Max_HP);
    }
}
