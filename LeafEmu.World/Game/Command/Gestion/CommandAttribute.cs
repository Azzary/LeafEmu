using System;

namespace LeafEmu.World.Game.Command.Gestion
{
    class CommandAttribute : Attribute
    {
        public string Command;
        public short Role;
        public short MinimalLen;
        public string CommandInfo;

        public CommandAttribute(short _Role, string _Command, short _MinimalLen, string _CommandInfo = "")
        {
            Command = _Command;
            Role = _Role;
            MinimalLen = _MinimalLen;
            CommandInfo = _CommandInfo;
        }

    }
}
