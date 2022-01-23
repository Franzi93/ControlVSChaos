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

        public Level currentLevel;
        
        [SerializeField] List<Ability> playerAbilities;
        [SerializeField] List<Ability> enemyAbilities;
        


        public event System.Action<Card> onPlayedCard;
        public event System.Action handIsEmpty;

        private Ability GetRandomAbility(List<Ability> abilities)
        {
            int i = Random.Range(0, abilities.Count);
            return abilities[i];
        }
        private EEnemyType GetRandomEnemyType()
        {
            List<EEnemyType> enemyTypes = currentLevel.GetAllRemainEnemyTypes();
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

        private void CreateRandomCard()
        {
            
            Card card = new Card(GetRandomAbility(playerAbilities), GetRandomAbility(enemyAbilities), GetRandomEnemyType());
            cards.Add(card);

            cardRenderer.SimpleUpdateUI(cards.ToArray());
 
        }

        public void CreateCard(int abilityIndex)
        {
            if (abilityIndex > playerAbilities.Count - 1)
            {
                Debug.LogWarning("You cannot create more cards than player abilities");
                return;
            }
            Card card = new Card(playerAbilities[abilityIndex], GetRandomAbility(enemyAbilities), GetRandomEnemyType());
            cards.Add(card);

            cardRenderer.SimpleUpdateUI(cards.ToArray());

        }
        public void CreateCard(Ability missingAbility)
        {
            Card card = new Card(missingAbility, GetRandomAbility(enemyAbilities), GetRandomEnemyType());
            cards.Add(card);

            cardRenderer.SimpleUpdateUI(cards.ToArray());

        }

        private void RefillHand()
        {
            for (int i = cards.Count; i < maxCards; i++)
            {
                CreateCard( i);
            }
        }
        public void ReshuffleHand()
        {
            RemoveAllCards();
            RefillHand();
        }

      

        public void PlayCard(Card card)
        {
            onPlayedCard(card);
            StartCoroutine(waitForSeconds(1f,()=>{
                
                RemoveCard(card);
                CreateCard(card.GetControlAbility());
                if (cards.Count == 0)
                {
                  //  handIsEmpty();
                }
                //ReshuffleHand();
            }));
        }

        IEnumerator waitForSeconds(float seconds, System.Action callback)
        {
            InputSystem.Lock();
            yield return new WaitForSeconds(seconds);
            callback();
            InputSystem.Unlock();
        }
        

        private void RemoveCard(Card card)
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
