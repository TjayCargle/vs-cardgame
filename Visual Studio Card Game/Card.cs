using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardData
{
    /// <summary>
    /// The Card class is designed to contain all the attributes needed for a card.
    /// Can be extended to have multiple abilities in the future.
    /// </summary>
    public class Card
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Points { get; set; }
        public Ability? Ability { get; private set; }

        public string? AdditionalText { get; private set; }

        public Card(string name, int cost, int power, Ability? ability = null)
        {
            Name = name;
            Cost = cost;
            Points = power;
            Ability = ability;
            AdditionalText = null;
        }

        public Card(string name, int cost, int power, Ability? ability, string additionalText)
        {
            Name = name;
            Cost = cost;
            Points = power;
            Ability = ability;
            AdditionalText = additionalText;
        }

        public string GetInfo()
        {
            return $"{Name} - Cost:{Cost} - Points:{Points} {(Ability != null ? Ability.Description : "")}";
        }     
    }
}
