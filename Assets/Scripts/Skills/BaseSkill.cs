using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class BaseSkill : MonoBehaviour
{

    public int SpCost;
    // public int HpCost;
    public float RecoveryTime = 1f;
    protected Character owner;
    

    void Awake()
    {
        owner = GetComponent<Character>();
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
        owner.DrainSP(SpCost);
        owner.IncreaseTurnTimer(RecoveryTime);
    }
    
    /// <summary>
    /// Check conditions for activation
    /// </summary>
    public virtual bool CanActivate()
    {
        return owner.IsActive && owner.SP_Current >= SpCost;
    }
}
