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

            int Level = 1;
            while (Level == 1)
            {
                DisplayTitleScreen();
                if (PlayLevel("Level_1.txt"))
                {
                    Level += 1;
                }
            }
            while (Level == 2)
            {
                DisplayTitleScreen();
                if (PlayLevel("Level_2.txt"))
                {
                    Level += 1;
                }
            }
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Thank you for playing Console Diver!");
            Console.WriteLine("     -Joshua Feiger");
            Console.WriteLine();
            DisplayContinueScreen("Press any key to exit.");
            
        }

        static void DisplayTitleScreen()
        {
            Console.Clear();
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
        static bool PlayLevel(string LevelName)
        {
            //Initialize several variables.

            //WriteY is the area of the level being written to the screen-- it's always the line at the bottom of the screen.
            int WriteY = 0;
            //PlayerY is the Y position of the player-- it's usually always going to be 26 rows above the WriteY row.
            //PlayerX is the column the player is on.
            int PlayerY = -35;
            int PlayerX = 50;

            int PastPlayerX = 50;
            int Speed = 50;
            int MovementSpeed = 1;

            bool IsBoosting = false;

            double TimeTaken = 0;

            //The FramesSince(key)Pressed variables are used to determine when the slide state should end.
            int FramesSinceJPressed = 100;
            int FramesSinceLPressed = 100;

            //"Level" is created as a string array to load the level in in its original state, but the slightly more versatile "LevelList" is what will be used by the program.
            string[] Level;
            Level = File.ReadAllLines("Content\\Levels\\" + LevelName);
            List<string> LevelList = Level.ToList();

            //A few variables for inputs are included. 
            //KeyPressed is used to determine what key has most recently been reacted to by the console, 
            //while InputBuffer stores some input that might otherwise get ignored accidentally.
            ConsoleKeyInfo KeyPressed;
            List<ConsoleKeyInfo> InputBuffer = new List<ConsoleKeyInfo>();

            //Clear the console and initialize one more variable, Line-- the line that will be written to the screen.
            Console.Clear();
            string Line = LevelList[WriteY];
            bool LevelCompleted = false;
            while (!(WriteY >= LevelList.Count()) && !LevelCompleted)
            {
                Console.SetWindowSize(100, 50);
                Console.SetBufferSize(100, 9999);

                Line = LevelList[WriteY];
                WriteY += 1;
                PlayerY += 1;

                FramesSinceJPressed += 1;
                FramesSinceLPressed += 1;

                Console.SetCursorPosition(0, WriteY - 1);
                Console.WriteLine(Line);

                

                if (PlayerY > -1)
                {
                    if (IsCollisionObjectOn(PlayerX, PlayerY, LevelList))
                    {
                        GameOverScreen(WriteY);
                        break;
                    }
                    else if (IsCharOn(PlayerX, PlayerY, LevelList, '+'))
                    {
                        Speed += 2;
                    }
                    else if (IsCharOn(PlayerX, PlayerY, LevelList, 'U'))
                    {
                        LevelComplete(WriteY, LevelName, TimeTaken);
                        LevelCompleted = true;
                        break;
                    }
                    Console.SetCursorPosition(PlayerX, PlayerY);
                    Console.Write("@");
                }

                if (PlayerY > 0)
                {
                    DrawPlayerTrail(PlayerX, PlayerY, PastPlayerX);
                }

                PastPlayerX = PlayerX;

                if (MovementSpeed == 1)
                {
                    PlayerX = SlideCode(2, Convert.ToInt32(Convert.ToDouble(Speed) - 40), FramesSinceJPressed, FramesSinceLPressed, PlayerX, PlayerY, LevelList);
                }
                else
                {
                    PlayerX = SlideCode(1, Convert.ToInt32((Convert.ToDouble(Speed) - 40)), FramesSinceJPressed, FramesSinceLPressed, PlayerX, PlayerY, LevelList);
                }

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

                    if (KeyPressed.Modifiers == ConsoleModifiers.Shift)
                    {
                        MovementSpeed = 2;
                    }
                    else
                    {
                        MovementSpeed = 1;
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
                            PlayerX = ChangePlayerX(PlayerX, PlayerY, -MovementSpeed, LevelList);
                            FramesSinceJPressed = 0;
                            break;
                        case ConsoleKey.L:
                            PlayerX = ChangePlayerX(PlayerX, PlayerY, MovementSpeed, LevelList);
                            FramesSinceLPressed = 0;
                            break;
                        case ConsoleKey.Z:
                            if (IsBoosting == false)
                            {
                                Speed += 15;
                                FramesSinceJPressed = 1000;
                                FramesSinceLPressed = 1000;
                                IsBoosting = true;
                            }
                            break;
                        case ConsoleKey.X:
                            if (IsBoosting == true)
                            {
                                Speed += -15;
                                FramesSinceJPressed = 1000;
                                FramesSinceLPressed = 1000;
                                IsBoosting = false;
                            }
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

                ////Comment the following out to make the level end.
                //LevelList.Add("A                                                                                                  A");

                //if (WriteY - 50 > -1)
                //{
                //    Console.SetCursorPosition(0, WriteY - 50);
                //}
                //else
                //{
                //    Console.SetCursorPosition(0, WriteY);
                //}

                Console.SetCursorPosition(0, WriteY);

                if (!((100 - Speed) < 1))
                {
                    Thread.Sleep(100 - Speed);
                    TimeTaken += 100 - Speed;
                }
            }
            return LevelCompleted;
        }

        static void GameOverScreen(int WriteY)
        {
            if (WriteY - 50 > -1)
            {
                Console.SetCursorPosition(0, WriteY - 50);
            }
            else
            {
                Console.SetCursorPosition(0, 0);
            }

            WriteWithTransparancy("        ^^^    ^^^   ^ ^  ^^^^     ^^^  ^   ^ ^^^^  ^^^^              ^^^^^^^^                      ");
            WriteWithTransparancy("       ^%%%^  ^%%%^ ^%^%^ %%%%^   ^%%%^^%^ ^%^%%%%^^%%%%^            ^^^%^^%^^^                     ");
            WriteWithTransparancy("      ^%^^^^ ^%^^^%^%^%^%^%^^^   ^%^ ^%^%^ ^%^%^^  ^%^^^%^           ^^^%^^%^^^                     ");
            WriteWithTransparancy("      ^%^^%%^^%%%%%^%^%^%^%%%^   ^%^ ^%^%^ ^%^%%%^ ^%%%%^            ^^^^^^^^^^                     ");
            WriteWithTransparancy("      ^%^^^%^^%^^^%^%^ ^%^%^^    ^%^ ^%^^%^%^^%^^^^^%^^^%^           ^^%%%%%%^^                     ");
            WriteWithTransparancy("       ^%%%^ ^%^ ^%^%^ ^%^%%%%%^  ^%%%^  ^%^ ^%%%%%^%^ ^%^           ^%^^^^^^%^                     ");
            WriteWithTransparancy("        ^^^   ^   ^ ^   ^ ^^^^^    ^^^    ^   ^^^^^ ^   ^             ^^^^^^^^                      ");

            DisplayContinueScreen("");
        }

        static void LevelComplete(int WriteY, string LevelName, double TimeTaken)
        {
            if (WriteY - 15 > -1)
            {
                Console.SetCursorPosition(0, WriteY - 15);
            }
            else
            {
                Console.SetCursorPosition(0, 0);
            }

            string[] HighScoreArray;
            HighScoreArray = File.ReadAllLines("SaveData\\HighScores\\" + LevelName);
            double HighScore = double.Parse(HighScoreArray[0]);

            Console.WriteLine("                                                                                                    ");
            Console.WriteLine("   Level Complete!                                                                                  ");
            Console.WriteLine("                                                                                                    ");
            Console.WriteLine($"                  Time: {TimeTaken}  ");
            if (TimeTaken < HighScore)
            {
                Console.WriteLine("                                                        NEW RECORD!                                 ");
                HighScoreArray[0] = TimeTaken.ToString();
                File.WriteAllLines("SaveData\\HighScores\\" + LevelName, HighScoreArray);
            }
            else
            {
                Console.WriteLine($"                                                        Record: {HighScore}  ");
            }
            
            Console.WriteLine("                                                                                                    ");
            DisplayContinueScreen("");
        }

        static bool IsCollisionObjectOn(int Xpos, int Ypos, List<string> LevelList)
        {
            List<char> CollisionObjects = new List<char>();
            CollisionObjects.Add('#');
            CollisionObjects.Add('A');

            bool IsCollisionObject;

            char LevelChar;

            try
            {
                LevelChar = LevelList[Ypos].ToArray()[Xpos];
            }
            catch (Exception)
            {
                LevelChar = '#';
            }

            if (CollisionObjects.Contains(LevelChar))
            {
                IsCollisionObject = true;
            }
            else
            {
                IsCollisionObject = false;
            }
            return IsCollisionObject;
        }

        static bool IsCharOn(int Xpos, int Ypos, List<string> LevelList, char Char)
        {
            bool IsCharAtPos;
            char LevelChar;

            try
            {
                LevelChar = LevelList[Ypos].ToArray()[Xpos];
                if (LevelChar == Char)
                {
                    IsCharAtPos = true;
                }
                else
                {
                    IsCharAtPos = false;
                }
            }
            catch (Exception)
            {
                IsCharAtPos = false;
            }

            return IsCharAtPos;
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
                        if (!IsCollisionObjectOn(PlayerX + 1, PlayerY, LevelList))
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
                        if (!IsCollisionObjectOn(PlayerX - 1, PlayerY, LevelList))
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

        static void DrawPlayerTrail(int PlayerX, int PlayerY, int PastPlayerX)
        {
            int Length = PastPlayerX - PlayerX;
            for (int i = 0; i < Math.Abs(Length); i++)
            {
                if (Length < 0)
                {
                    AddStringAtPos("0", PastPlayerX + i, PlayerY - 1);
                }
                else
                {
                    AddStringAtPos("0", PastPlayerX - i, PlayerY - 1);
                }
            }
            if (Length == 0)
            {
                AddStringAtPos("0", PastPlayerX, PlayerY - 1);
            }
        }

        static void WriteWithTransparancy(string String)
        {
            char[] StringToArray = String.ToArray();
            foreach (char Char in StringToArray)
            {
                if (!(Char == ' '))
                {
                    if (Char == '^')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(Char);
                    }
                }
                else
                {
                    try
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                    catch (Exception)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                    }
                }
            }
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