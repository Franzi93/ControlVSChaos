using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Duality
{
    public class CardRenderer : MonoBehaviour
    {
        [System.Serializable]
        public class EnemySymbol
        {
            public Image image;
            public EEnemyType enemyType;
        }

        public EnemySymbol[] enemySymbols;


    }
}
