using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontraktbaseret_udvikling___Projekt.Interfaces;

namespace Kontraktbaseret_udvikling___Projekt.DataModels
{
    public class RockPaperScissorPlayer : IPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Wins { get; set; }
        public int Pick { get; set; }

        public RockPaperScissorPlayer(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
