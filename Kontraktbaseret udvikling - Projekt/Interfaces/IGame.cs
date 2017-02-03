using System;
using System.Collections.Generic;
using Kontraktbaseret_udvikling___Projekt.DataModels;

namespace Kontraktbaseret_udvikling___Projekt.Interfaces
{
    public interface IGame
    {

        string Name                     { get; set; }
        string Description              { get; set; }
        int MinPlayers                  { get; set; }
        int MaxPlayers                  { get; set; }
        List<IPlayer> Players           { get; set; }

        bool Start();
        bool Terminate();
        void Run();

        bool IsRunning                  { get; set; }
        Func<Input> UserInputEvent      { get; set; }
        Input GetUserInput();
    }
}
