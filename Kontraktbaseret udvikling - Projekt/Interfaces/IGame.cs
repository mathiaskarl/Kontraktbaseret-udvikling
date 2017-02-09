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

        bool IsRunning                  { get; set; }
        Func<Input> UserInputEvent      { get; set; }

        /*
        * Command 
        * Signature: 
        *   Bool Start()
        * Ensure:
        *   Result              == this.IsRunning
        */
        bool Start();

        /*
         * Command 
         * Signature: 
         *   Bool Terminate()
         * Require:
         *   this.IsRunning      == true;
         * Ensure:
         *   Result              == this.IsRunning
         */
        bool Terminate();

        /*
        * Command 
        * Signature: 
        *   void Run()
        * Require:
        *   this.IsRunning      == true;
        * Ensure:
        *   this.IsRunning      == false;
        */
        void Run();

        /*
        * Query 
        * Signature: 
        *   Input GetUserInput()
        * Ensure:
        *    Result              == Func<Input> || null
        */
        Input GetUserInput();

    }
}
