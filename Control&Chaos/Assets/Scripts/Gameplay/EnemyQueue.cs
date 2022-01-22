using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyQueue 
{
    private Queue<IAbility> abilities;

    public void EnqueueAbility(IAbility ability)
    {
        abilities.Enqueue(ability);
    }

    public IAbility DequeueAbility()
    {
        return abilities.Dequeue();
    }
    public void Reset()
    {
        abilities.Clear();
    }

}
