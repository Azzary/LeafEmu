using System;

namespace LeafEmu.World.PacketGestion
{
    class PacketAttribute : Attribute
    {
        public string packet;

        public PacketAttribute(string _packet) => packet = _packet;

    }
}
