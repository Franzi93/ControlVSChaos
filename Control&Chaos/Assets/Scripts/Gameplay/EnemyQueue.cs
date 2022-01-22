using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyQueue 
{
    private Queue<Ability> abilities;

    public void EnqueueAbility(Ability ability)
    {
        abilities.Enqueue(ability);
    }

    public Ability DequeueAbility()
    {
        return abilities.Dequeue();
    }
    public void Reset()
    {
        abilities.Clear();
    }

}
