namespace Hackathon
{
    using System;

    public static class Settings
    {
        // Maximum time in milliseconds that your bot can have in its time bank.
        public static int Timebank { get; set; }
        // Time in milliseconds that is added to your bot's time bank each move.
        public static int TimePerMove { get; set; }
        // The id of this bot
        public static int MyBotId { get; set; }
        // The id of the enemy bot
        public static int EnemyBotId { get; set; }

        public static int FieldWidth { get; set; }

        public static int FieldHeight { get; set; }

        public static int MaxRounds { get; set; }

        public static void ReadSetting(string[] stringParts)
        {
            switch (stringParts[1])
            {
                case "timebank":
                    Timebank = int.Parse(stringParts[2]);
                    break;

                case "time_per_move":
                    TimePerMove = int.Parse(stringParts[2]);
                    break;

                case "your_botid":
                    MyBotId = int.Parse(stringParts[2]);
                    EnemyBotId = MyBotId == 0 ? 1 : 0;
                    break;

                case "field_width":
                    FieldWidth = int.Parse(stringParts[2]);
                    break;

                case "field_height":
                    FieldHeight = int.Parse(stringParts[2]);
                    break;

                case "max_rounds":
                    MaxRounds = int.Parse(stringParts[2]);
                    break;

                default:
                    Console.Error.WriteLine("Unknown input string '" + string.Join(" ", stringParts) + "'.");
                    break;
            }
        }
    }
}
