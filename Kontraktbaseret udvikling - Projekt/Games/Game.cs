using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___Projekt.DataModels;
using Kontraktbaseret_udvikling___Projekt.Interfaces;

namespace Kontraktbaseret_udvikling___Projekt.Games
{
    public abstract class Game : IGame
    {
        public string Name                  { get; set; }
        public string Description           { get; set; }
        public int MinPlayers               { get; set; }
        public int MaxPlayers               { get; set; }

        public List<IPlayer> Players        { get; set; }

        public bool IsRunning { get; set; }

        public Func<Input> UserInputEvent { get; set; }

        public Input GetUserInput()
        {
            if (this.UserInputEvent == null || !this.IsRunning)
                return null;

            var input = this.UserInputEvent.Invoke();
            if (input != null)
                return input;

            return GetUserInput();
        }

        public virtual bool Start()
        {
            this.IsRunning = true;
            return true;
        }

        public virtual void Run()
        {
            throw new NotImplementedException();
        }

        public virtual bool Terminate()
        {
            this.IsRunning = false;
            return true;
        }

        
    }
}
