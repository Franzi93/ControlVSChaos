using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData 
{
    
   
}
public class Ability : ScriptableObject
{
    public virtual void Use() { }
}


public class AttackAbility : Ability
{
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
