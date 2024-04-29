namespace CardData
{
    /// <summary>
    /// AbilityActivator is a class designed to house and to handle executing card abilities.
    /// When a new ability is implemented the work should be done here to keep all ability effects within one class.
    /// Static class so that everyone can access it and no instances should be made.
    /// </summary>
    internal static class AbilityActivator
    {
        public static int ActivateAbility(Card card)
        {
            if(card == null)
            {
                return -1;
            }

            if (card.Ability != null)
            {
                _ = card.Ability.Effect switch
                {
                    AbilityEffect.PowerEqualsTurn => SetPowerToTurn(card),
                    AbilityEffect.ReturnToOwnerHand => ReturnToOwnerHand(card),
                    _=> ThrowInvalidDataExceptionForAbility(card.Ability),
                };              
            }
            return 0;
        }

        private static int SetPowerToTurn(Card card)
        {
            card.Points = GameManager.GetCurrentTurn();
            GameManager.WriteToGame($"***{card.Name} points are now:{card.Points}***", 10);
            return 0;
        }

        private static int ReturnToOwnerHand(Card card)
        {          
            GameManager.TurnPlayer.FindAndMoveCardToHand(card.Name);
            GameManager.WriteToGame($"***{card.Name} returned to the hand***");
            return 0;
        }

        private static int ThrowInvalidDataExceptionForAbility(Ability ability)
        {
            throw new InvalidDataException($"Unable to determine Ability Effect enum `{ability.Effect}`");
        }
    }
}
