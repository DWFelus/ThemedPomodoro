namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void SelectRoutine()
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Select the Default Routine");
            Console.WriteLine("--------------------------");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Here is where you can select which routine is going to be loaded to start by default.");
            Console.WriteLine("Enter the name of the file stored in %%Location%%");
        }
    }

}
