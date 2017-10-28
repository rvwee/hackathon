namespace Hackathon
{
    using System;
    using System.IO;

    public static class Program
    {
        public static void Main(string[] args)
        {
            // Increase the buffer size of the input stream
            Console.SetIn(new StreamReader(Console.OpenStandardInput(512)));

            string input;
            while ((input = Console.ReadLine()) != null)
            {
                var stringParts = input.Split(' ');

                switch (stringParts[0])
                {
                    case "update":
                        HandleUpdate(stringParts);
                        break;

                    case "action":
                        if (stringParts[1] != "move")
                        {
                            Console.Error.WriteLine("Unknown input string '" + string.Join(" ", stringParts) + "'.");
                            break;
                        }

                        PerformAction(int.Parse(stringParts[2]));
                        break;

                    case "settings":
                        Settings.ReadSetting(stringParts);
                        break;

                    case "debug":
                        HandleDebug(stringParts);
                        break;

                    default:
                        Console.Error.WriteLine("Unknown input string '" + input + "'.");
                        break;
                }
            }
        }

        private static void HandleDebug(string[] stringParts)
        {
            switch (stringParts[1])
            {
                case "field":
                    Console.Error.WriteLine(new string('-', Settings.FieldWidth));
                    for (var y = 0; y < Settings.FieldHeight; ++y)
                    {
                        for (var x = 0; x < Settings.FieldWidth; ++x)
                        {
                            Console.Error.Write((int)Game.Board[x, y]);
                        }
                        Console.Error.WriteLine();
                    }
                    Console.Error.WriteLine(new string('-', Settings.FieldWidth));
                    break;
            }
        }

        private static void HandleUpdate(string[] stringParts)
        {
            switch (stringParts[1])
            {
                case "game":
                    HandleGameUpdate(stringParts);
                    break;
                     
                case "player0":
                    HandlePlayerUpdate(0, stringParts);
                    break;

                case "player1":
                    HandlePlayerUpdate(1, stringParts);
                    break;

                default:
                    Console.Error.WriteLine("Unknown input string '" + string.Join(" ", stringParts) + "'.");
                    break;
            }
        }

        private static void HandleGameUpdate(string[] stringParts)
        {
            switch (stringParts[2])
            {
                case "round":
                    Console.Error.WriteLine("Started round " + int.Parse(stringParts[3]) + ".");
                    break;

                case "field":
                    Game.Board = new Game.Celltype[Settings.FieldWidth, Settings.FieldHeight];
                    var fields = stringParts[3].Split(',');
                    var index = 0;
                    for (var y = 0; y < Settings.FieldHeight; ++y)
                    {
                        for (var x = 0; x < Settings.FieldWidth; ++x)
                        {
                            var field = fields[index++];
                            Game.Board[x, y] = field == "." ? Game.Celltype.Dead : (Game.Celltype) int.Parse(field);
                        }
                    }
                    break;

                default:
                    Console.Error.WriteLine("Unknown input string '" + string.Join(" ", stringParts) + "'.");
                    break;
            }
        }

        private static void HandlePlayerUpdate(int id, string[] stringParts)
        {
            // todo: update player0 living_cells 50
        }

        private static void PerformAction(int timeLeft)
        {
            Game.Tick();
            Console.WriteLine("up");
        }
    }
}
