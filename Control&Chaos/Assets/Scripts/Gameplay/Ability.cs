using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    
}
public interface IAbility
{
    void Use();
}
public class DirectionAbility : IAbility
{
    public void Use()
    {
        throw new System.NotImplementedException();
    }
}

public class AttackAbility : IAbility
{
    public void Use()
    {
        throw new System.NotImplementedException();
    }
}
