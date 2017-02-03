using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontraktbaseret_udvikling___Projekt.DataModels
{
    public class Input
    {
        public string RawInput  = null;
        public string Command   = null;
        public string Value     = null;

        public Input() { }

        public Input(string input)
        {
            this.RawInput = input;
            this.HandleInput();
        }

        public bool HasCommand()
        {
            return this.Command != null;
        }

        public bool HasValue()
        {
            return this.Value != null;
        }

        private void HandleInput()
        {
            var values = this.RawInput.Split(' ');

            if (!values.Any())
                return;
            this.AssignInputValues(values);
        }

        private void AssignInputValues(string[] values)
        {
            if (!values[0].StartsWith("/"))
                return;

            this.Command = values[0].ToLower();
            this.Value = string.Join(" ", values.Skip(1));
            this.Value = string.Join("", this.Value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
