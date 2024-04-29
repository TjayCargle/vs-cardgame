using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardData
{
    /// <summary>
    /// The Database class is deisgned to house the cards of the game. 
    ///Currently only houses the starter deck but can be used to house and retrieve all cards.
    ///Should be replaced in the future with a connection to an actual database.
    /// </summary>
    internal static class Database
    {
        //Card is Name, 
        private static List<Card> StarterDeck = new List<Card>()
        {
            new Card("Ghost", 0, 1),
            new Card("Shadow", 0, 1),
            new Card("Worm", 1, 1),
            new Card("Imp", 1, 1),
            new Card("Demon", 2, 2),
            new Card("Rouge", 2, 2),
            new Card("Devil", 3, 0, new Ability(AbilityTiming.InHand,AbilityEffect.PowerEqualsTurn)),
            new Card("Nightmare", 3, 2, new Ability(AbilityTiming.EndOfTurn, AbilityEffect.ReturnToOwnerHand)),
            new Card("Witch", 4, 4),
            new Card("Vampire", 4, 4),
            new Card("Necromancer", 5, 6),
            new Card("Bone Golem", 6, 8, null, "Give me your bones!"),
        };
        public static List<Card> GetStarterDeck()
        {
            var returnedDeck = new List<Card>();
            returnedDeck.AddRange(StarterDeck);
            return returnedDeck;
        }
    }
}
