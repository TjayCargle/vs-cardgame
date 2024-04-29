using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardData
{
    /// <summary>
    /// The Player class designed to handle the management of a player, their cards, and moving things to their proper locations.
    /// </summary>
    public class Player
    {
        public string Name { get; set; }
        public int Turn { get; set; }
        public int Points { get; set; }
        public int Energy { get; set; }
        public List<Card> Deck { get; private set; } = new();
        public List<Card> Hand { get; private set; } = new();
        public List<Card> PlayedThisTurn { get; private set; } = new();
        public List<Card> Discard { get; private set; } = new();

        public Player(string name, List<Card> deck)
        {
            Name = name;
            Deck = deck;
        }

        public void Setup()
        {
            Turn = 0;
            Points = 0;
            Deck.AddRange(Hand);
            Deck.AddRange(PlayedThisTurn);
            Deck.AddRange(Discard);

            Hand.Clear();
            PlayedThisTurn.Clear();
            Discard.Clear();
            ShuffleDeck();
            Draw(3);
        }

        public void ShuffleDeck()
        {
            Random random = new Random();
            for (int oldIndex = Deck.Count - 1; oldIndex > 0; oldIndex--)
            {
                int newIndex = random.Next(oldIndex + 1);
                var card = Deck[oldIndex];
                Deck[oldIndex] = Deck[newIndex];
                Deck[newIndex] = card;
            }
        }

        public void Draw(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var card = Deck[i];
                Deck.Remove(card);
                Hand.Add(card);
            }
        }

        public int Play(string? cardName)
        {
            if (cardName == null)
            {
                return -1;
            }

            Card? currentCard = Hand.Find((card) => card.Name.ToLower() == cardName.ToLower());

            if (currentCard != null)
            {
                if (currentCard.Cost <= Energy)
                {
                    Hand.Remove(currentCard);
                    PlayedThisTurn.Add(currentCard);
                    Energy -= currentCard.Cost;
                    Points += currentCard.Points;
                   
                    GameManager.WriteToGame($"{Name} played {cardName}!");

                    if (!string.IsNullOrEmpty(currentCard.AdditionalText))
                    {
                       GameManager.WriteToGame($"{cardName} yelled {currentCard.AdditionalText}",20, true);
                    }

                    return 0;
                }
                else
                {
                    return -2;
                }
            }

            return -1;
        }

        public void EndTurn()
        {
            Discard.AddRange(PlayedThisTurn);
            PlayedThisTurn.Clear();
        }

        //Should be more generic and search deck, play, and di
        public int FindAndMoveCardToHand(string cardName)
        {
            Card? currentCard = PlayedThisTurn.Find((card) => card.Name.ToLower() == cardName.ToLower());
            if (currentCard != null)
            {
                PlayedThisTurn.Remove(currentCard);
                Hand.Add(currentCard);
            }
            if (currentCard == null)
            {
                currentCard = Deck.Find((card) => card.Name.ToLower() == cardName.ToLower());
                if (currentCard != null)
                {
                    Deck.Remove(currentCard);
                    Hand.Add(currentCard);
                }
            }
            if (currentCard == null)
            {
                currentCard = Discard.Find((card) => card.Name.ToLower() == cardName.ToLower());
                if (currentCard != null)
                {
                    Discard.Remove(currentCard);
                    Hand.Add(currentCard);
                }
            }

            if (currentCard == null)
            {
                return -1;
            }
            return 0;
        }
    }
}
