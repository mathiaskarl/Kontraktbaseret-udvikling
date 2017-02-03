using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___Projekt.DataModels;

namespace Kontraktbaseret_udvikling___Projekt
{
    public class IOHandler
    {
        private readonly GameManager _gameManager;

        public IOHandler(GameManager gm)
        {
            this._gameManager = gm;
        }

        public Input Input(string input)
        {
            return new Input(input);
        }

        public Input HandleInput()
        {
            var input = this.Input(Console.ReadLine());

            if (!input.HasCommand())
                return input;

            if (!ExecuteProtocol(input))
                return input;
            return null;
        }

        public bool ExecuteProtocol(Input input)
        {
            switch (input.Command)
            {
                case "/start":
                    this._gameManager.StartGame(input);
                    break;

                case "/exit":
                    this._gameManager.TerminateGame();
                    break;

                case "/games":
                    this._gameManager.DisplayGames();
                    break;

                case "/desc":
                    this._gameManager.DisplayGameInfo(input);
                    break;

                case "/help":
                    this._gameManager.DisplayInfo();
                    break;

                default:
                    return false;
            }
            return true;
        }
    }
}
