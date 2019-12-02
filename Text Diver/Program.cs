using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

//Text Diver by Joshua Feiger.

namespace Text_Diver
{
    class Program
    {
        static void Main(string[] args)
        {
            //Confirm the window size is correct. Try to set the window size,
            //and if that gives an error then prompt the user to fix the issue that caused the problem.
            bool IsCorrectWindowSize = false;
            while (!IsCorrectWindowSize)
            {
                try
                {
                    Console.SetWindowSize(100, 50);
                    IsCorrectWindowSize = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Your screen's resolution is too low for the current font size to fit the game window. Please adjust your font size.");
                    Console.WriteLine("To adjust font size, right-click the title bar and click \"Properties\". Switch to the \"Fonts\" tab and change the size option to a lower number.");
                    Console.Title = "Right-click me!";
                    DisplayContinueScreen("Press any key to try again.");
                    Console.Clear();
                }
            }

            //Set the title bar's text to "Console Diver"
            Console.Title = "Console Diver";
            DisplayTitleScreen();
            PlayLevel();
        }

        static void DisplayTitleScreen()
        {
            Console.WriteLine(
                "OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                 ###                           #                                                 O" + "\n" +
                "O                #      ###   ###   ###  ###    #    ####                                         O" + "\n" +
                "O                #     #   #  #  # ##   #   #   #   #   #                                         O" + "\n" +
                "O                #     #   #  #  #   ## #   #   #   #                                             O" + "\n" +
                "O                 ###   ###   #  # ###   ###   ###   ###                                          O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                       #######  ######## #      # ######   #######                               O" + "\n" +
                "O                       #      #    #     #      # #        #      #                              O" + "\n" +
                "O                       #      #    #     #      # ####     #######                               O" + "\n" +
                "O                       #      #    #      #    #  #        #      #                              O" + "\n" +
                "O                       #      #    #       #  #   #        #      #                              O" + "\n" +
                "O                       #######  ########    ##    ######## #      #                              O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                     Controls:                                                                   O" + "\n" +
                "O                     J: steer left                                                               O" + "\n" +
                "O                     L: steer right                                                              O" + "\n" +
                "O                     K: slide cancel                                                             O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                     Press a key to begin!                                                       O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "O                                                                                                 O" + "\n" +
                "OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            Console.SetCursorPosition(0, 0);
            Console.SetCursorPosition(43, 35);
            Console.ReadKey(true);
        }

        /// <summary>
        /// Play a level.
        /// </summary>
        static void PlayLevel()
        {
            //Initialize several variables.

            //WriteY is the area of the level being written to the screen-- it's always the line at the bottom of the screen.
            int WriteY = 0;
            //PlayerY is the Y position of the player-- it's usually always going to be 26 rows above the WriteY row.
            //PlayerX is the column the player is on.
            int PlayerY = -26;
            int PlayerX = 50;

            //The FramesSince(key)Pressed variables are used to determine when the slide state should end.
            int FramesSinceJPressed = 100;
            int FramesSinceLPressed = 100;

            //"Level" is created as a string array to load the level in in its original state, but the slightly more versatile "LevelList" is what will be used by the program.
            string[] Level;
            Level = File.ReadAllLines("Content\\Levels\\TextFile1.txt");
            List<string> LevelList = Level.ToList();

            //A few variables for inputs are included. 
            //KeyPressed is used to determine what key has most recently been reacted to by the console, 
            //while InputBuffer stores some input that might otherwise get ignored accidentally.
            ConsoleKeyInfo KeyPressed;
            List<ConsoleKeyInfo> InputBuffer = new List<ConsoleKeyInfo>();

            //Clear the console and initialize one more variable, Line-- the line that will be written to the screen.
            Console.Clear();
            string Line = LevelList[WriteY];
            while (!(WriteY >= LevelList.Count()))
            {
                Line = LevelList[WriteY];
                WriteY += 1;
                PlayerY += 1;

                FramesSinceJPressed += 1;
                FramesSinceLPressed += 1;

                Console.SetCursorPosition(0, WriteY - 1);
                Console.WriteLine(Line);

                if (PlayerY > -1)
                {
                    Console.SetCursorPosition(PlayerX, PlayerY);
                    Console.Write("@");
                }

                PlayerX = SlideCode(2, 10, FramesSinceJPressed, FramesSinceLPressed, PlayerX, PlayerY, LevelList);

                if (Console.KeyAvailable || InputBuffer.ToArray().Length > 0)
                {
                    if (InputBuffer.ToArray().Length > 0)
                    {
                        KeyPressed = InputBuffer[0];
                        InputBuffer.RemoveAt(0);
                    }
                    else
                    {
                        KeyPressed = Console.ReadKey(true);
                    }

                    switch (KeyPressed.Key)
                    {
                        case ConsoleKey.I:
                            break;
                        case ConsoleKey.K:
                            FramesSinceJPressed = 0;
                            FramesSinceLPressed = 0;
                            break;
                        case ConsoleKey.J:
                            PlayerX = ChangePlayerX(PlayerX, PlayerY, -1, LevelList);
                            FramesSinceJPressed = 0;
                            break;
                        case ConsoleKey.L:
                            PlayerX = ChangePlayerX(PlayerX, PlayerY, 1, LevelList);
                            FramesSinceLPressed = 0;
                            break;
                        default:
                            break;
                    }

                    //The following code just gets rid of some issues with input lagging. It clears the "input buffer",
                    //but may add some parts of the buffer into my own custom-made buffer.
                    while (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo DisposableKey = Console.ReadKey(true);
                        //You may ask why the key is disposable. We're throwing it away, but we have to acknowledge its existence first.
                        //That's why we use the "ReadKey()" method.
                        if (!(DisposableKey == KeyPressed))
                        {
                            InputBuffer.Add(DisposableKey);
                            //Disposable key is actually different from what's come before, therefore it is no longer disposable. 
                            //This definitely doesn't reflect any mentality.
                        }
                    }
                }

                //Comment the following out to make the level end.
                LevelList.Add("A                                                                                                  A");

                Console.SetCursorPosition(0, WriteY);
                Thread.Sleep(50);
            }
        }

        static int SlideCode(int Divisor, int Max, int FramesSinceJPressed, int FramesSinceLPressed, int PlayerX, int PlayerY, List<string> LevelList)
        {
            if (FramesSinceJPressed < FramesSinceLPressed)
            {
                if (FramesSinceJPressed < Max && Math.Round(Convert.ToDouble(FramesSinceJPressed) / Divisor) == Convert.ToDouble(FramesSinceJPressed) / Divisor)
                {
                    PlayerX = ChangePlayerX(PlayerX, PlayerY, -1, LevelList);
                }
            }
            else if (FramesSinceLPressed < FramesSinceJPressed)
            {
                if (FramesSinceLPressed < Max && Math.Round(Convert.ToDouble(FramesSinceLPressed) / Divisor) == Convert.ToDouble(FramesSinceLPressed) / Divisor)
                {
                    PlayerX = ChangePlayerX(PlayerX, PlayerY, 1, LevelList);
                }
            }
            return PlayerX;
        }

        static int ChangePlayerX(int PlayerX, int PlayerY, int ChangeBy, List<string> LevelList)
        {
            if (PlayerY > -1)
            {
                for (int DistanceMoved = 1; DistanceMoved <= Math.Abs(ChangeBy); DistanceMoved++)
                {
                    if (ChangeBy > 0)
                    {
                        char LevelChar = LevelList[PlayerY].ToArray()[PlayerX + DistanceMoved];
                        if (LevelChar == ' ')
                        {
                            PlayerX += 1;
                        }
                        else
                        {
                            DistanceMoved = ChangeBy + 1;
                        }
                    }
                    else if (ChangeBy < 0)
                    {
                        char LevelChar = LevelList[PlayerY].ToArray()[PlayerX - DistanceMoved];
                        if (LevelChar == ' ')
                        {
                            PlayerX += -1;
                        }
                        else
                        {
                            DistanceMoved = Math.Abs(ChangeBy) + 1;
                        }
                    }
                }
            }
            return PlayerX;
        }

        /// <summary>
        /// Adds a string at a certain X and Y position.
        /// </summary>
        static void AddStringAtPos(string StringToWrite, int X, int Y)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(StringToWrite);
        }

        /// <summary>
        /// Display a continue screen with a generic message.
        /// </summary>
        static void DisplayContinueScreen()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Display a continue screen with a message based on program context.
        /// You can use the DisplayContinueScreen() method and not worry about entering in a special message, 
        /// but thanks to stacked methods the option for custom text is there.
        /// </summary>
        /// <param name="DisplayMessage">The custom message to display.</param>
        static void DisplayContinueScreen(string DisplayMessage)
        {
            Console.WriteLine(DisplayMessage);
            Console.ReadKey();
        }

    }
}