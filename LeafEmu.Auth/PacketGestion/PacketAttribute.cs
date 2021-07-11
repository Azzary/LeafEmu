using System;

namespace LeafEmu.Auth.PacketGestion
{
    class PacketAttribute : Attribute
    {
        public string packet;

        public PacketAttribute(string _packet) => packet = _packet;

    }
}
