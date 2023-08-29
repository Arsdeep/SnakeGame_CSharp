using System;
using System.Collections.Generic;
using System.Threading;

namespace Game
{
    internal class Program
    {
        static char keyRead = '\0';

        public static bool GameOver = false;
        public static void ReadKey()                    // Function for a thread which reads key input constantly
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                    if (OppositeDir((char)keyInfo.Key))
                        keyRead = (char)keyInfo.Key;
                }
            }
        }

        public static bool OppositeDir(char c)          // Make sure the snake doesnt go Backwards
        {
            if (c == 'A' && keyRead != 'D')
                return true;

            else if (c == 'W' && keyRead != 'S')
                return true;

            else if (c == 'D' && keyRead != 'A')
                return true;

            else if (c == 'S' && keyRead != 'W')
                return true;

            return false;
        }

        static void Main()
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Green;


            Thread readKeyThread = new Thread(ReadKey);    // Creates a Constant Read Key thread
            readKeyThread.Start();

            int w = 40;                     // Default Screen Size
            int h = 20;


            Console.WriteLine("======================================================================================\n");

            Console.WriteLine("██╗ ██╗      ███████╗███╗   ██╗ █████╗ ██╗  ██╗███████╗   ██╗ ██████╗       ██╗ ██╗\r\n╚██╗╚██╗     ██╔════╝████╗  ██║██╔══██╗██║ ██╔╝██╔════╝   ██║██╔═══██╗     ██╔╝██╔╝\r\n ╚██╗╚██╗    ███████╗██╔██╗ ██║███████║█████╔╝ █████╗     ██║██║   ██║    ██╔╝██╔╝ \r\n ██╔╝██╔╝    ╚════██║██║╚██╗██║██╔══██║██╔═██╗ ██╔══╝     ██║██║   ██║    ╚██╗╚██╗ \r\n██╔╝██╔╝     ███████║██║ ╚████║██║  ██║██║  ██╗███████╗██╗██║╚██████╔╝     ╚██╗╚██╗\r\n╚═╝ ╚═╝      ╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝╚═╝ ╚═════╝       ╚═╝ ╚═╝");

            Console.WriteLine("\n======================================================================================");
            Console.WriteLine($"\t\t\t\t\t\t\t\t©2023 Arsdeep Dewangan\n\n");

            Console.WriteLine("\t\t\t   ╔═══════════════════╗");
            Console.WriteLine("\t\t\t    Press ENTER to PLAY");
            Console.WriteLine("\t\t\t   ╚═══════════════════╝");

            Console.WriteLine("\nControls -");
            Console.WriteLine("   W - Up");
            Console.WriteLine("   A - Left");
            Console.WriteLine("   S - Down");
            Console.WriteLine("   D - Right");

            Console.Write("\n\n\nEnter Custom Box Size if you want to - ");


            string input = Console.ReadLine(); // Replace with your comma-separated input string

            if (input != "")
            {
                string[] parts = input.Split(',');

                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out int firstNumber) && int.TryParse(parts[1], out int secondNumber))
                    {
                        w = firstNumber;
                        h = secondNumber;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input format. Unable to parse integers. Using Default Values");
                        Thread.Sleep(3000);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input format. Input must contain two comma-separated integers. Using Default Values");
                    Thread.Sleep(3000);
                }
            }


            Console.Clear();

            new Screen(w, h, Border: true);      // Create a screen
            Snake S = new Snake(Convert.ToInt32(w / 3), Convert.ToInt32(h / 3));   // Creates a snake

            while (true)
            {
                S.Update(keyRead);                         // Updates Snakes position

                Screen.display();

                if (GameOver)
                {
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("Game Over :(");
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine($"Total Score - {S.getScore()}");
                    break;
                }
            }
        }
    }

    class Screen
    {
        static int[][] scr;
        public static int width;
        public static int height;
        public static int slpTime;

        public Screen(int size, bool On = false, bool Border = false, int sleepTime = 200)
        {
            width = size;
            height = size / 2;
            slpTime = sleepTime;

            Alloc();

            if (On)
                PreTurnOn();
            if (Border)
                BorderInit();
        }

        public Screen(int x, int y, bool On = false, bool Border = false, int sleepTime = 200)
        {
            width = x;
            height = y;
            slpTime = sleepTime;

            Alloc();

            if (On)
                PreTurnOn();
            if (Border)
                BorderInit();
        }

        public static void Alloc()
        {
            scr = new int[height][];
            for (int i = 0; i < height; i++)
            {
                scr[i] = new int[width];
            }
        }

        public static void PreTurnOn()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    scr[i][j] = 1;
                }
            }
        }

        public static void BorderInit()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1)
                        scr[i][j] = 1;
                    if (j == 0 || j == width - 1)
                        scr[i][j] = 1;
                }
            }
        }


        public static void display()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    switch (scr[i][j])
                    {
                        case 1:
                            Console.Write("⬛ ");
                            break;

                        case 2:
                            Console.Write("● ");
                            break;

                        case 3:
                            Console.Write("○ ");
                            break;

                        case 4:
                            Console.Write("▲ ");
                            break;

                        case 5:
                            Console.Write("△ ");
                            break;

                        case 0:
                            Console.Write("  ");
                            break;
                    }

                }
                Console.WriteLine("");
            }
            Thread.Sleep(slpTime);

            if (!Program.GameOver)
                Console.Clear();
        }

        public static void On(int x, int y, int Type = 1)
        {
            scr[y][x] = Type;
        }
        public static void Off(int x, int y)
        {
            scr[y][x] = 0;
        }

        public static int at(int x, int y)
        {
            return scr[y][x];
        }
    }

    class Snake
    {
        Random random = new Random();

        public List<int[]> coords = new List<int[]>();

        int foodX = -1, foodY = -1;


        public Snake(int x, int y)
        {
            coords.Add(new int[] { x, y });
            coords.Add(new int[] { x, y });
            coords.Add(new int[] { x, y });
        }

        public void Update(char keyRead)
        {
            Screen.Off(coords[coords.Count - 1][0], coords[coords.Count - 1][1]); // Turn off the last blip

            for (int i = coords.Count - 1; i > 0; i--)     // Change every bits Coordinates
            {
                coords[i][0] = coords[i - 1][0];
                coords[i][1] = coords[i - 1][1];
            }

            switch (keyRead)
            {
                case 'W':
                    if (Screen.at(coords[0][0], coords[0][1] - 1) != 0 && Screen.at(coords[0][0], coords[0][1] - 1) != 4)
                    {
                        Program.GameOver = true;
                        return;
                    }
                    coords[0][1] -= 1;
                    break;

                case 'S':
                    if (Screen.at(coords[0][0], coords[0][1] + 1) != 0 && Screen.at(coords[0][0], coords[0][1] + 1) != 4)
                    {
                        Program.GameOver = true;
                        return;
                    }
                    coords[0][1] += 1;
                    break;

                case 'A':
                    if (Screen.at(coords[0][0] - 1, coords[0][1]) != 0 && Screen.at(coords[0][0] - 1, coords[0][1]) != 4)
                    {
                        Program.GameOver = true;
                        return;
                    }
                    coords[0][0] -= 1;
                    break;

                case 'D':
                    if (Screen.at(coords[0][0] + 1, coords[0][1]) != 0 && Screen.at(coords[0][0] + 1, coords[0][1]) != 4)
                    {
                        Program.GameOver = true;
                        return;
                    }
                    coords[0][0] += 1;
                    break;
            }

            Console.WriteLine($"Score - {getScore()}");

            for (int i = 0; i < coords.Count; i++)
            {
                Screen.On(coords[i][0], coords[i][1], Type: 2);
            }

            Food();
        }

        public void Food()
        {
            if (foodX == -1 && foodY == -1)
            {
                do
                {
                    foodX = random.Next(2, Screen.width - 2);
                    foodY = random.Next(2, Screen.height - 2);
                }
                while (Screen.at(foodX, foodY) != 0);

                Screen.On(foodX, foodY, Type: 4);
            }

            if (coords[0][0] == foodX && coords[0][1] == foodY)
            {
                foodX = -1;
                foodY = -1;
                coords.Add(new int[] { coords[0][0], coords[0][1] });

                if (Screen.slpTime > 30)
                    Screen.slpTime -= 10;
            }

            Console.WriteLine($"Food at - {foodX},{foodY}");
        }

        public int getScore()
        {
            return coords.Count - 3;
        }
    }
}
