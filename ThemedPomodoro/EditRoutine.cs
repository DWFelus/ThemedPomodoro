namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void EditRoutine()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Edit a Routine");
            Console.WriteLine("--------------");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Here is where you can edit a part of an existing routine.");
            Console.WriteLine("Select which part of the routine you would like to change.");
        }
    }

}
