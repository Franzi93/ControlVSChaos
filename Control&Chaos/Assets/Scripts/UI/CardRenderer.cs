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
            cardUI.enemyType.sprite = GetSpriteFromArray(card.GetEnemyType());
            cardUI.enemyAbility.sprite = card.GetChaosAbility().enemySprite;
            cardUI.playerAbility.sprite = card.GetControlAbility().playerSprite;
            
            cardUI.button.onClick.AddListener(()=>onClickCard(card));
        }

        Sprite GetSpriteFromArray(EEnemyType enemyType)
        {
            foreach (var symbol in enemySymbols)
            {
                if (symbol.enemyType == enemyType)
                {
                    return symbol.sprite;
                }
            }

            return null;
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
            RemoveAllCards();
            AddCards(cards);
        }

    }
}
