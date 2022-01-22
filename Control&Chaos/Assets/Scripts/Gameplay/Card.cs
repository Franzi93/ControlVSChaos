using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    private Ability controlAbility;
    private Ability chaosAbility;

    public Card(Ability _controlAbility, Ability _chaosAbility)
    {
        controlAbility = _controlAbility;
        chaosAbility = _chaosAbility;
    }

}
