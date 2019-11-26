using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Text_Diver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(100, 50);
            //Directory.CreateDirectory("Content");
            //Directory.CreateDirectory("Content\\Levels");
            //File.WriteAllText("Content\\Levels\\TextFile1.txt", "Hello");
            PlayLevel();
        }

        static void PlayLevel()
        {
            int WriteY = -1;
            int PlayerY = -26;
            int PlayerX = 25;
            ConsoleKeyInfo KeyPressed;
            string[] Level;
            Level = File.ReadAllLines("Content\\Levels\\TextFile1.txt");

            foreach (string Line in Level)
            {
                WriteY += 1;
                PlayerY += 1;

                Console.SetCursorPosition(0, WriteY);
                Console.WriteLine(Line);

                if (PlayerY > -1)
                {
                    Console.SetCursorPosition(PlayerX, PlayerY);
                    Console.Write("@");
                }

                if (Console.KeyAvailable)
                {
                    KeyPressed = Console.ReadKey(true);
                    switch (KeyPressed.Key)
                    {
                        case ConsoleKey.I:
                            break;
                        case ConsoleKey.K:
                            break;
                        case ConsoleKey.J:
                            PlayerX += -1;
                            break;
                        case ConsoleKey.L:
                            PlayerX += 1;
                            break;
                        default:
                            break;
                    }
                }
                
                Thread.Sleep(50);
            }
        }
    }
}
