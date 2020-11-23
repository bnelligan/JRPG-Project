public class Skill_Rest : BaseSkill
{
    protected override void InitSkillInfo()
    {
        // Base skill info
        SkillName = "Rest";
        SkillID = "SKILL_REST";
        SkillDescription = "Take a break to recover some SP";
        SkillType = E_SkillVariant.UTILITY;
        SpCost = -1; // Negative SP cost means we recover SP
        RecoveryTime = 0.75f;
        DamageMod = 0f;
        AccuracyMod = 0f;
        CritMod = 0f;
    }
}
