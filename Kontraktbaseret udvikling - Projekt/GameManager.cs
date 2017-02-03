using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___Projekt.DataModels;
using Kontraktbaseret_udvikling___Projekt.Games;
using Kontraktbaseret_udvikling___Projekt.Interfaces;

namespace Kontraktbaseret_udvikling___Projekt
{
    public class GameManager
    {
        public IGame CurrentGame;

        private readonly IOHandler _ioHandler;
        private readonly List<IGame> _games = new List<IGame>() { new RockPaperScissors() };

        public GameManager()
        {
            this._ioHandler = new IOHandler(this);
            this.DisplayInfo();
            this.InputLoop();
        }

        private void InputLoop()
        {
            while (true)
            {
                this._ioHandler.HandleInput();
            }
        }

        public void StartGame(Input input)
        {
            try
            {
                var gameId = this.GetGameId(input);

                if (this.IsCurrentlyPlaying())
                    throw new Exception("You are already playing a game.");

                var currentGame = this.GetGameAtIndex(gameId);
                currentGame.UserInputEvent = this._ioHandler.HandleInput;

                if (!currentGame.Start())
                    throw new Exception("Could not start game - Internal game error.");

                this.CurrentGame = currentGame;

                Console.WriteLine("Started game: " + currentGame.Name);
                this.CurrentGame.Run();
            }
            catch (NullReferenceException e) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void TerminateGame()
        {
            if (!this.IsCurrentlyPlaying())
            {
                Console.WriteLine("You are not currently playing a game.");
                return;
            }

            if(!this.CurrentGame.Terminate())
            {
                Console.WriteLine("Could not terminate game - Internal game error.");
                return;
            }

            Console.WriteLine(this.CurrentGame.Name + " has been terminated.");
            this.CurrentGame = null;
        }

        public void DisplayInfo()
        {
            Console.WriteLine("Launched GameManager successfully");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Type /games to see the list of current games.");
            Console.WriteLine("Type /desc {Game id} for a description of the game.");
            Console.WriteLine("Type /start {Game id} to start a game.");
            Console.WriteLine("Type /exit to terminate an active game session.");
            Console.WriteLine("Type /help to view this menu again.");
        }

        public void DisplayGames()
        {
            Console.WriteLine("Displaying current available games.");
            Console.WriteLine("---------------------------------------------------------------");
            for (var i = 1; i < this._games.Count() + 1; i++)
            {
                Console.WriteLine(i + ". " + this._games[i - 1].Name + ".");
            }
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Type /desc {Game id} for a description of the game.");
            Console.WriteLine("Type /start {Game id} to start a game.");
        }

        public void DisplayGameInfo(Input input)
        {
            try
            {
                var gameId = this.GetGameId(input);
                var currentGame = this.GetGameAtIndex(gameId);

                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Displaying game: " + currentGame.Name);
                Console.WriteLine(currentGame.Description);
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Minimum players: " + currentGame.MinPlayers);
                Console.WriteLine("Maximum players: " + currentGame.MaxPlayers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private bool IsCurrentlyPlaying()
        {
            return this.CurrentGame != null;
        }

        private bool GameExistsAtIndex(int index)
        {
            return this._games.Count - 1 >= index;
        }

        private IGame GetGameAtIndex(int index)
        {
            return this._games[index];
        }

        private int GetGameId(Input input)
        {
            if (!input.HasValue())
                throw new Exception("You must enter a game id.");

            int gameId;
            if (!int.TryParse(input.Value, out gameId))
                throw new Exception("Invalid game id.");

            if (!this.GameExistsAtIndex(gameId - 1))
                throw new Exception("Game does not exist.");

            return gameId - 1;
        }

    }
}
