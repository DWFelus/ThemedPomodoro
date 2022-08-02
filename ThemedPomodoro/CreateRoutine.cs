namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void CreateRoutine()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Creating a routine");
            Console.WriteLine("------------------");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Here is where you are going to create your own routine to run from the main menu.");
            Console.WriteLine("Carefully answer each query to create a schedule.");
            Console.WriteLine();
        }
    }
}
