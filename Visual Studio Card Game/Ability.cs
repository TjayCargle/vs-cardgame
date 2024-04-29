namespace CardData
{
    /// <summary>
    /// The Ability class is designed to give extra effects to cards.
    /// Designed to be flexible with timing of abilities an execution of effects.
    /// Could be extended in the future to have an ability contain multiple effects.
    /// </summary>
    public class Ability
    {
        public AbilityTiming Timing { get; set; }
        public AbilityEffect Effect { get; set; }
        public string Description { get; private set; }

        public Ability(AbilityTiming timing, AbilityEffect effect)
        {
            Timing = timing;
            Effect = effect;
            Description = BuildDescription(timing, effect);
        }

        private string BuildDescription(AbilityTiming timing, AbilityEffect effect)
        {

            var prefix = timing switch
            {
                AbilityTiming.InHand => "While this card is in the hand ",
                AbilityTiming.EndOfTurn => "At the end of the turn ",
                _ => ThrowInvalidDataExceptionDuringDescription(effect)
            };
            var suffix = effect switch
            {
                AbilityEffect.PowerEqualsTurn => "this card's points are equal to the current turn.",
                AbilityEffect.ReturnToOwnerHand => "return this card to its owner's hand.",
                _ => ThrowInvalidDataExceptionDuringDescription(effect),
            };

            var description = prefix + suffix;
            return description;
        }       

        private string ThrowInvalidDataExceptionDuringDescription(AbilityEffect effect)
        {
            throw new InvalidDataException($"Unknown type of AbilityTiming:`{effect}` was provided when building description");
        }
    }
}
