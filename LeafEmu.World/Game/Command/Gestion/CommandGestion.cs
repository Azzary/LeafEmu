using LeafEmu.World.PacketGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LeafEmu.World.Game.Command.Gestion
{
    class CommandGestion
    {
        public static readonly List<CommandDatas> metodos = new List<CommandDatas>();


        public static void init()
        {
            Assembly asm = typeof(Frame).GetTypeInfo().Assembly;

            foreach (MethodInfo type in asm.GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0))
            {
                CommandAttribute attribute = type.GetCustomAttributes(typeof(CommandAttribute), true)[0] as CommandAttribute;
                Type type_string = Type.GetType(type.DeclaringType.FullName);

                object instance = Activator.CreateInstance(type_string, null);
                metodos.Add(new CommandDatas(instance, attribute.Command, type, attribute.Role, attribute.MinimalLen, attribute.CommandInfo));
            }
        }


        public static bool Gestion(Network.listenClient PrmClient, string command)
        {
            string[] CommandSplit = command.Split(' ');
            CommandDatas method = metodos.Find(m => CommandSplit[0].Substring(1) == m.name_command);
            try
            {
                if (method != null && PrmClient.account.Role >= method.RoleNeeded && CommandSplit.Length - 1 >= method.MinimalLen)
                {
                    Network.listenClient target = CommandSplit.Length - 2 >= method.MinimalLen ? PrmClient.CharacterInWorld.Find(x => x.account.character.speudo == CommandSplit[method.MinimalLen + 1]) : PrmClient;
                    if (target == null)
                    {
                        PrmClient.sendDebugMsg($"client {CommandSplit[method.MinimalLen + 1]} is not connected");
                        return false;
                    }
                    method.information.Invoke(method.instance, new object[2] { target, command });
                    return true;
                }

            }
            catch (Exception ex)
            {
                Logger.Logger.Log(ex);
            }
            return false;
        }


    }
}
