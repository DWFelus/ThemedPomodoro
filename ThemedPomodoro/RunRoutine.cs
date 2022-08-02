namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void RunRoutine(string mode)
        {
            if (mode == "primary")
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Running Primary Routine");
                Console.WriteLine("-----------------------");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Running Secondary Routine");
                Console.WriteLine("-------------------------");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }

}
