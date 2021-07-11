﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace LeafBot.PacketGestion
{
    class PacketGestion
    {
        public static readonly List<PacketDatas> methodes = new List<PacketDatas>();


        public static void init()
        {
            Assembly asm = typeof(Frame).GetTypeInfo().Assembly;

            foreach (MethodInfo type in asm.GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(PacketAttribute), false).Length > 0))
            {
                PacketAttribute attribute = type.GetCustomAttributes(typeof(PacketAttribute), true)[0] as PacketAttribute;
                Type type_string = Type.GetType(type.DeclaringType.FullName);

                object instance = Activator.CreateInstance(type_string, null);
                methodes.Add(new PacketDatas(instance, attribute.packet, type));
            }
        }

        static List<string> lastPacket = new List<string>();
        public static bool Gestion(Client.Client client, string packet)
        {
            PacketDatas method = methodes.Find(m => packet.StartsWith(m.name_packet));

            try
            {
                if (method != null)
                {
                    method.information.Invoke(method.instance, new object[3] { client, packet, lastPacket });
                    lastPacket.Clear();
                    return true;
                }
                else
                    lastPacket.Add(packet);

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            return false;
        }


    }
}
