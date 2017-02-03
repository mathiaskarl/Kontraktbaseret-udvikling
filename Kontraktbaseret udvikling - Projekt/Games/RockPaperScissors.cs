using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___Projekt.DataModels;
using Kontraktbaseret_udvikling___Projekt.Interfaces;

namespace Kontraktbaseret_udvikling___Projekt.Games
{
    public class RockPaperScissors : Game
    {
        public enum GameState
        {
            Started,
            Terminated,
            AwaitingInput,
            AssigningPlayers,
            AssigningPlayerPicks,
            DeterminingWinner
        }

        private GameState _state;


        public RockPaperScissors()
        {
            this.Name = "Rock, Paper and Scissors";
            this.Description = "Play the classic game of Rock, Paper and Scissors.";
            this.MinPlayers = 2;
            this.MaxPlayers = 2;
        }

        public override bool Start()
        {
            // Logic before execution of the game by GameManager
            // e.g. Welcome message.
            // e.g. Command list.
            this.ChangeState(GameState.Started);
            return base.Start();
        }

        public override bool Terminate()
        {
            // Logic before termination by GameManager
            // e.g. Save current game session.
            // e.g. Save High scores.
            this.ChangeState(GameState.Terminated);
            return base.Terminate();
        }

        public override void Run()
        {
            while (this.IsRunning)
            {
                switch (this._state)
                {
                    case GameState.Started:
                        this.ChangeState(GameState.AssigningPlayers);
                        break;

                    case GameState.AssigningPlayers:
                        this.AssignPlayers();
                        break;

                    case GameState.AssigningPlayerPicks:
                        this.AssignPlayerPicks();
                        break;

                    case GameState.DeterminingWinner:
                        this.DeterminingWinners();
                        break;
                }
            }
        }

        private void AssignPlayers()
        {
            Console.WriteLine("Welcome to Rock, Paper, Scissors");

            this.Players = new List<IPlayer>();
            for (int i = 1; i < 3; i++)
            {
                if (!this.IsRunning)
                    break;

                Console.WriteLine("Pick a name for Player " + i);

                var input = this.GetUserInput();
                if (input != null)
                    this.Players.Add(new RockPaperScissorPlayer(i, input.RawInput));

            }
            this.ChangeState(GameState.AssigningPlayerPicks);
        }

        private void AssignPlayerPicks()
        {
            Console.WriteLine("Starting game.");

            foreach (RockPaperScissorPlayer player in this.Players)
            {
                if (!this.IsRunning)
                    break;

                Console.WriteLine("Your turn to pick " + player.Name + "\n\n- Commands: \n - Write 1 for Scissor\n - Write 2 for Paper\n - Write 3 for Rock\n");
                while (true)
                {
                    var pick = this.GetUserInput();
                    int pickInt;

                    if (IsValidPick(pick, out pickInt))
                    {
                        player.Pick = pickInt;
                        Console.Clear();
                        break;
                    }

                    Console.WriteLine("The entered value is invalid\n\n- Commands: \n - Write 1 for Scissor\n - Write 2 for Paper\n - Write 3 for Rock\n");
                }
            }
            this.ChangeState(GameState.DeterminingWinner);
        }

        private bool IsValidPick(Input input, out int pick)
        {
            if (!int.TryParse(input.RawInput, out pick))
                return false;

            if(pick < 1 || pick > 3)
                return false;

            return true;
        }

        private void DeterminingWinners()
        {
            IPlayer winner;
            if ((((RockPaperScissorPlayer)this.Players[0]).Pick) % 3 + 1 == ((RockPaperScissorPlayer)this.Players[1]).Pick)
                winner = this.Players[0];

            else if ((((RockPaperScissorPlayer)this.Players[1]).Pick) % 3 + 1 == ((RockPaperScissorPlayer)this.Players[0]).Pick)
                winner = this.Players[1];
            else
                winner = null;

            if (winner == null)
                Console.WriteLine("The round is a draw");
            else
                Console.WriteLine(winner.Name + " has won the round.");


            Console.WriteLine("Next round starts in 3 seconds.");
            Thread.Sleep(3000);
            this.ChangeState(GameState.AssigningPlayerPicks);
        }

        private void ChangeState(GameState state)
        {
            Console.Clear();
            this._state = state;
        }
    }
}
