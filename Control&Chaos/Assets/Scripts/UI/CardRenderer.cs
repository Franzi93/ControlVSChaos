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

        public GameObject cardUIPrefab;
        private List<CardUI> cardUIs = new List<CardUI>();


        public void AddCards(Card[] cards)
        {
            foreach (Card card in cards)
            {
                AddCard(card);
            }
        }
        public void AddCard(Card card)
        {
            CardUI c = Instantiate(cardUIPrefab,transform).GetComponent<CardUI>();
            cardUIs.Add(c);
        }
        public void RemoveAllCards()
        {
            foreach (CardUI cardUI in cardUIs)
            {
                Destroy(cardUI);
            }
        }
       

        public void SimpleUpdateUI(Card[] cards)
        {
            RemoveAllCards();
            AddCards(cards);
        }

    }
}
