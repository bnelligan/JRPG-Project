using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterStats))]
public abstract class BaseSkill : MonoBehaviour
{
    public enum E_SkillType
    {
        MELEE,
        RANGED,
        SPECIAL,
        UTILITY
    }
    public string SkillID { get; protected set; }
    public string SkillName { get; protected set; }
    public string SkillDescription { get; protected set; }  // Description should be a coded string with colored words and icons
    public E_SkillType SkillType { get; protected set; }
    public int SpCost { get; protected set; }
    // public int HpCost; // TODO maybe?
    public float RecoveryTime { get; protected set; }
    public float DamageMod { get; protected set; }
    public float AccuracyMod { get; protected set; }
    public float CritMod { get; protected set; }
    protected CharacterStats ownerStats;
    protected Character owner;
    
    
    void Awake()
    {
        owner = GetComponent<Character>();
        ownerStats = GetComponent<CharacterStats>();
        InitSkillInfo();
    }
    
    /// <summary>
    /// Activate the skill after checking conditions
    /// </summary>
    /// <returns>Was the skill activated</returns>
    public virtual bool TryActivate()
    {
        bool canActivate = CanActivate();
        if(canActivate)
        {
            Activate();
        }
        return canActivate;
    }

    /// <summary>
    /// Activate the skill without checking conditions
    /// </summary>
    public virtual void Activate()
    {
        if(SpCost > 0)
        {
            ownerStats.LoseSP((uint)Mathf.Abs(SpCost));
        }
        // Gain SP when cost is negative
        else if(SpCost < 0)
        {
            ownerStats.GainSP((uint)Mathf.Abs(SpCost));
        }
        owner.DelayTurnTimer(RecoveryTime);
    }
    
    /// <summary>
    /// Check conditions for activation
    /// </summary>
    public virtual bool CanActivate()
    {
        return owner.IsActive && ownerStats.SP >= SpCost;
    }

    /// <summary>
    /// Should load skill info from a file or database, but for now
    /// each skill will implement this and populate info
    /// </summary>
    protected abstract void InitSkillInfo();
}

