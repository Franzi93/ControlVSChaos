using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duality
{
    public class CardSystem : MonoBehaviour
    {
        public CardRenderer cardRenderer;
        public List<Card> cards = new List<Card>();
        public int maxCards;
        
        [SerializeField] List<Ability> playerAbilities;
        [SerializeField] List<Ability> enemyAbilities;

        public event System.Action<Card> onPlayedCard;

        public Ability GetRandomAbility(List<Ability> abilities)
        {
            int i = Random.Range(0, abilities.Count-1);
            return abilities[i];
        }

        private void Start()
        {
            cardRenderer.onClickCard += PlayCard;
        }

        public void CreateCard()
        {
            //todo enemy types dependent on level
            Card card = new Card(GetRandomAbility(playerAbilities), GetRandomAbility(enemyAbilities), EEnemyType.King);
            cards.Add(card);

            cardRenderer.SimpleUpdateUI(cards.ToArray());
 
        }

        public void CreateCards()
        {
            for (int i = 0; i < maxCards; i++)
            {
                CreateCard();
            }
        }
        public void ReshuffleHand()
        {
            RemoveAllCards();
            CreateCards();
        }

        public void PlayCard(Card card)
        {
            onPlayedCard(card);

            RemoveCard(card);
            ReshuffleHand();
        }
        public void RemoveCard(Card card)
        {
            cards.Remove(card);
            cardRenderer.SimpleUpdateUI(cards.ToArray());
        }
        public void RemoveAllCards()
        {
            cards.Clear();
            cardRenderer.SimpleUpdateUI(cards.ToArray());
        }

    }
}
