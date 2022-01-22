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
            public Sprite sprite;
            public EEnemyType enemyType;
        }

        public EnemySymbol[] enemySymbols;

        public GameObject cardUIPrefab;
        private List<CardUI> cardUIs = new List<CardUI>();

        public event System.Action<Card> onClickCard;


        public void AddCards(Card[] cards)
        {
            foreach (Card card in cards)
            {
                AddCard(card);
            }
        }

        public void AddCard(Card card)
        {
            CardUI cardUI = Instantiate(cardUIPrefab,transform).GetComponent<CardUI>();
            cardUIs.Add(cardUI);

            cardUI.button.onClick.AddListener(() => onClickCard(card));
        }



        public void RemoveAllCards()
        {
            for (int i = 0; i< cardUIs.Count;i++)
            {
                if (cardUIs[i])
                {
                    Destroy(cardUIs[i].gameObject);
                }
            }
            cardUIs.Clear();
        }
       

        public void SimpleUpdateUI(Card[] cards)
        {
            Debug.Log("Redraw Cards");
            RemoveAllCards();
            AddCards(cards);
        }

    }
}
