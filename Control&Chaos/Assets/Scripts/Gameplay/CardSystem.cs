using Dmdrn.UnityDebug;
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
        public event System.Action handIsEmpty;

        private Ability GetRandomAbility(List<Ability> abilities)
        {
            int i = Random.Range(0, abilities.Count);
            return abilities[i];
        }
        private EEnemyType GetRandomEnemyType(List<EEnemyType> enemyTypes)
        {
            if (enemyTypes.Count == 0)
            {
                return EEnemyType.None;
            }
            int i = Random.Range(0, enemyTypes.Count);
            return enemyTypes[i];
        }

        private void Start()
        {
            cardRenderer.onClickCard += PlayCard;

            
        }

        private void CreateCard(List<EEnemyType> enemyTypes)
        {

            //todo enemy types dependent on level
            Card card = new Card(GetRandomAbility(playerAbilities), GetRandomAbility(enemyAbilities), GetRandomEnemyType(enemyTypes));
            cards.Add(card);

            cardRenderer.SimpleUpdateUI(cards.ToArray());
 
        }

        private void RefillHand(List<EEnemyType> enemyTypes)
        {
            for (int i = cards.Count; i < maxCards; i++)
            {
                CreateCard(enemyTypes);
            }
        }
        public void ReshuffleHand(List<EEnemyType> enemyTypes)
        {
            RemoveAllCards();
            RefillHand(enemyTypes);
        }

        public void PlayCard(Card card)
        {
            onPlayedCard(card);

            RemoveCard(card);

            if (cards.Count == 0)
            {
                handIsEmpty();
            }
            //ReshuffleHand();
        }
        private void RemoveCard(Card card)
        {
            cards.Remove(card);
            cardRenderer.SimpleUpdateUI(cards.ToArray());
        }
        private void RemoveAllCards()
        {
            cards.Clear();
            cardRenderer.SimpleUpdateUI(cards.ToArray());
        }

    }
}
