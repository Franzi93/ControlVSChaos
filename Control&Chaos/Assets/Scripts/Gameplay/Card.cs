using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    private IAbility controlAbility;
    private IAbility chaosAbility;

    public Card(IAbility _controlAbility, IAbility _chaosAbility)
    {
        controlAbility = _controlAbility;
        chaosAbility = _chaosAbility;
    }

}
