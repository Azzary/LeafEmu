using System;
using System.Collections.Generic;

namespace LeafBot
{
    class Program
    {

        static void Main(string[] args)
        {
            List<Client.Client> listClient = new List<Client.Client>();
            Console.Write("number of clients: ");
            int nbClient = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Logger.Log($"Creation in progress...");
            for (int i = 0; i <= nbClient; i++)
            {
                listClient.Add(new Client.Client(i, listClient));
                Turn(i, nbClient);
            }
            Console.Clear();
            Logger.Log("PressKey to start...");
            Console.ReadKey();
            new GestionClient(listClient);
        }


        public static void Turn(int i, int max)
        {
            Console.Write(i + "/" + max);
            Console.SetCursorPosition(Console.CursorLeft - (i.ToString() + "/" + max).Length, Console.CursorTop);
        }

    }
}
