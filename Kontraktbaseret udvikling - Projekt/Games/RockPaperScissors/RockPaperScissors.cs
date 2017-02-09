using System;
using System.Collections.Generic;
using System.Threading;
using Kontraktbaseret_udvikling___Projekt.DataModels;
using Kontraktbaseret_udvikling___Projekt.Games.RockPaperScissors.DataModels;
using Kontraktbaseret_udvikling___Projekt.Interfaces;

namespace Kontraktbaseret_udvikling___Projekt.Games.RockPaperScissors
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


        /*
        * Command 
        * Signature: 
        *   RockPaperScissors()
        * Ensure:
        *   this.Name           = "Rock, Paper and Scissors"
        *   this.Description    = "Play the classic game of Rock, Paper and Scissors."
        *   this.MinPlayers     = 2
        *   this.MaxPlayers     = 2
        */
        public RockPaperScissors()
        {
            this.Name = "Rock, Paper and Scissors";
            this.Description = "Play the classic game of Rock, Paper and Scissors.";
            this.MinPlayers = 2;
            this.MaxPlayers = 2;
        }

        /*
        * Command 
        * Signature: 
        *   Bool Start()
        * Require:
        *   this.Name           != null
        *   this.Description    != null
        *   this.MinPlayers     != null
        *   this.MaxPlayers     != null
        * Ensure:
        *   this._state         == GameState.Started
        *   Result              == this.IsRunning
        */
        public override bool Start()
        {
            // Logic before execution of the game by GameManager
            // e.g. Welcome message.
            // e.g. Command list.
            this.ChangeState(GameState.Started);
            return base.Start();
        }

        /*
        * Command 
        * Signature: 
        *   Bool Terminate()
        * Require:
        *   this.IsRunning      == true;
        * Ensure:
        *   this._state         == GameState.Terminated
        *   Result              == this.IsRunning
        */
        public override bool Terminate()
        {
            // Logic before termination by GameManager
            // e.g. Save current game session.
            // e.g. Save High scores.
            this.ChangeState(GameState.Terminated);
            return base.Terminate();
        }

        /*
        * Command 
        * Signature: 
        *   void Run()
        * Require:
        *   this._state         == GameState.Started
        * Ensure:
        *   this._state         == GameState.Terminated
        */
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

        /*
        * Command 
        * Signature: 
        *   void AssignPlayers()
        * Require:
        *   this._state         == GameState.AssigningPlayers
        * Ensure:
        *   this._state         == (GameState.AssigningPlayerPicks implies:
                                        this.Players.Count == 2)
        *                           || GameState.Terminated
        */
        private void AssignPlayers()
        {
            Console.WriteLine("Welcome to Rock, Paper, Scissors");

            this.Players = new List<IPlayer>();
            for (int i = 1; i < 3; i++)
            {
                Console.WriteLine("Pick a name for Player " + i);

                var input = this.GetUserInput();

                if (!this.IsRunning)
                    return;

                if (input != null)
                    this.Players.Add(new RockPaperScissorPlayer(i, input.RawInput));

            }
            this.ChangeState(GameState.AssigningPlayerPicks);
        }

        /*
        * Command 
        * Signature: 
        *   void AssignPlayerPicks()
        * Require:
        *   this._state         == GameState.AssigningPlayerPicks
        * Ensure:
        *   this._state         == (GameState.DeterminingWinner implies:
        *                               For all players in this.Players:
        *                                   Player.Pick     != null)
        *                           || GameState.Terminated
        */
        private void AssignPlayerPicks()
        {
            Console.WriteLine("Starting game.");

            foreach (RockPaperScissorPlayer player in this.Players)
            {
                Console.WriteLine("Your turn to pick " + player.Name + "\n\n- Commands: \n - Write 1 for Scissor\n - Write 2 for Paper\n - Write 3 for Rock\n");
                while (true)
                {
                    var pick = this.GetUserInput();

                    if (!this.IsRunning)
                        return;

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


        /*
        * Query 
        * Signature: 
        *   bool IsValidPick(Input input, out int pick)
        * Require:
        *   input               != null
        */
        private bool IsValidPick(Input input, out int pick)
        {
            if (!int.TryParse(input.RawInput, out pick))
                return false;

            if(pick < 1 || pick > 3)
                return false;

            return true;
        }

        /*
        * Command 
        * Signature: 
        *   void DeterminingWinners()
        * Require:
        *   this._state         == GameState.DeterminingWinner
        * Ensure:
        *   this._state         == GameState.AssigningPlayerPicks
        */
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

        /*
        * Command 
        * Signature: 
        *   void ChangeState(GameState state)
        * Require:
        *   state               != null
        * Ensure:
        *   this._state         == state
        */
        private void ChangeState(GameState state)
        {
            Console.Clear();
            this._state = state;
        }
    }
}
