using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardData
{
    /// <summary>
    /// The GameManager is the main handler for in game actions and establishing the flow of the game.
    /// Static class so that everyone can access it and no instances should be made.
    /// </summary>
    internal class GameManager
    {
        private static Player Player1 = new Player("Player 1", Database.GetStarterDeck());
        private static Player Player2 = new Player("Player 2", Database.GetStarterDeck());
        private static bool isRunning = true;

        public static Player TurnPlayer = Player2;

        public static void WriteToGame(string output, int milisecondsToWait = 1, bool forceCaps = false)
        {
            if (forceCaps)
            {
                output = output.ToUpper();
            }
            foreach (char c in output)
            {
                Console.Write(c);
                Thread.Sleep(milisecondsToWait);
            }
            Console.WriteLine();
        }
        public static int GetCurrentTurn()
        {
            return TurnPlayer.Turn;
        }
        public static void Run()
        {
            WriteToGame("The game has begun");
            GetCommands();
            Init();
            while (isRunning)
            {
                var input = Console.ReadLine();

                _ = input?.ToLower() switch
                {
                    "end" => NextTurn(),
                    "turn" => GetTurnInfo(),
                    "help" => GetCommands(),
                    "hand" => CheckHand(),
                    _ => CheckInput(input),
                };
            }
        }
        public static int NextTurn()
        {
            ActivateEffectsAtEndOfTurn();
            if (TurnPlayer == Player2 && Player2.Turn >= 6)
            {
                EndGame();
                return 1;
            }
            else
            {
                WriteToGame("--------------------------------------------");
                TurnPlayer = TurnPlayer == Player1 ? Player2 : Player1;
                TurnPlayer.Turn++;
                TurnPlayer.Energy = TurnPlayer.Turn;
                if (TurnPlayer.Turn == 6)
                {
                    WriteToGame("turn 6 - this is the last turn!", 20, true);
                }
                TurnPlayer.Draw(1);
                GetTurnInfo();
                ActivateEffectsInHand();
                CheckHand();
                WriteToGame("Awaiting input...");
            }
            return 0;
        }
        private static void Init()
        {
            Player1.Setup();
            Player2.Setup();

            //Next turn will switch players so we initally set TurnPlayer to Player 2 since Next Turn will switch to Player 1
            TurnPlayer = Player2;
            NextTurn();
        }

        private static int GetTurnInfo()
        {
            WriteToGame($"It is {TurnPlayer.Name}'s turn.");
            WriteToGame($"Turn: {TurnPlayer.Turn}");
            WriteToGame($"points: {TurnPlayer.Points}");
            WriteToGame($"Energy: {TurnPlayer.Energy}");
            return 0;
        }
        private static int GetCommands()
        {
            WriteToGame("The list of commands are:");
            WriteToGame("'[card name]' - plays the card if it exists in players hand");
            WriteToGame("'end' - ends the current player's turn and begins the next player's turn.");
            WriteToGame("'hand' - check the cards in your hand");
            WriteToGame("'turn' - check current player, turn, current energy, and points.");

            return 0;
        }
        private static int CheckHand()
        {
            WriteToGame("--------");

            WriteToGame($"The current cards in {TurnPlayer.Name}'s hand are:");
            var Hand = TurnPlayer.Hand;
            foreach (var card in Hand)
            {
                WriteToGame(card.GetInfo());
            }
            return 0;
        }
        private static int EndGame()
        {
            WriteToGame("Game Over!");
            WriteToGame($"{Player1.Name} has {Player1.Points}");
            WriteToGame($"{Player2.Name} has {Player2.Points}");
            if (Player1.Points == Player2.Points)
            {
                WriteToGame("Draw!", 10, true);
            }
            else
            {
                WriteToGame($"{(Player1.Points > Player2.Points ? Player1.Name : Player2.Name)} is the winner!", 20, true);
            }
            var didAnswer = false;
            while (didAnswer == false)
            {
                WriteToGame("Would you like to play again? yes/no");
                var input = Console.ReadLine();
                if (input == "yes" || input == "y")
                {
                    didAnswer = true;
                    Init();
                }
                else if (input == "no" || input == "n")
                {
                    didAnswer = true;
                    isRunning = false;
                }
            }
            return 0;
        }
        private static int CheckInput(string? input)
        {
            switch (TurnPlayer.Play(input))
            {
                case 0:
                    WriteToGame($"{TurnPlayer.Name} points are now: {TurnPlayer.Points}");
                    WriteToGame($"{TurnPlayer.Name} remaining energy is: {TurnPlayer.Energy}");
                    CheckHand();
                    break;
                case -1:
                    WriteToGame("Inavlid input. Please type `help` for a list of inputs");
                    break;
                case -2:
                    WriteToGame($"Not enough energy to play {input}");
                    break;
            };
            return 0;
        }
        private static void ActivateEffectsInHand()
        {
            var cardsWithHandEffects = TurnPlayer.Hand.FindAll((card) => card.Ability != null && card.Ability.Timing == AbilityTiming.InHand);
            foreach (var card in cardsWithHandEffects)
            {
                AbilityActivator.ActivateAbility(card);
            }
        }
        private static void ActivateEffectsAtEndOfTurn()
        {
            var endOfTurnCards = TurnPlayer.PlayedThisTurn.FindAll((card) => card.Ability != null && card.Ability.Timing == AbilityTiming.EndOfTurn);
            foreach (var card in endOfTurnCards)
            {
                AbilityActivator.ActivateAbility(card);
            }
            TurnPlayer.EndTurn();
        }
     
    }
}
