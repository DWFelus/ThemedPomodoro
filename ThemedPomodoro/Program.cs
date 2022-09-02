using static ThemedPomodoro.Mode;
public partial class Mode
{
    public static void Main()
    {
        MainMenu();
        static void MainMenu()
        {
            Console.Clear();

            bool routineLoaded = true;
            bool routineTypeDaily = false;
            string validMainMenuUserChoice;
            string loadedRoutineName = "test";

            ReadGlobalConfigFile();

            Console.WriteLine("Themed Pomodoro");
            Console.WriteLine("---------------");
            Console.WriteLine();

            DisplayMainMenuChoices(); // displaying the options to the user depending on the loaded routine
            validMainMenuUserChoice = MainMenuUserInput(); // getting input from the user
            GoToMenu(validMainMenuUserChoice); // going into sub menus

            void GoToMenu(string vMainMenuUserChoice)
            {
                switch (vMainMenuUserChoice)
                {
                    case "RunRoutinePrimary":
                        RunRoutine("primary");
                        MainMenu();
                        break;
                    case "RunRoutineSecondary":
                        RunRoutine("secondary");
                        MainMenu();
                        break;
                    case "SelectRoutine":
                        SelectRoutine();
                        MainMenu();
                        break;
                    case "EditRoutine":
                        EditRoutine();
                        MainMenu();
                        break;
                    case "CreateRoutine":
                        CreateRoutine();
                        MainMenu();
                        break;
                }
            }
            void ReadGlobalConfigFile() // loading initial settings
            {
                Console.Write(""); // placeholder for now
            }
            string MainMenuUserInput()
            {
                bool correctUserInput = false;
                string goToMenu = "";
                while (!correctUserInput)
                {
                    Console.Write("Enter Q/W/E/R/T to choose an option: ");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string mainMenuUserInput = Console.ReadLine().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    switch (mainMenuUserInput)
                    {
                        case "Q":
                            if (routineLoaded == true)
                            {
                                correctUserInput = true;
                                goToMenu = "RunRoutinePrimary";
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice");
                            }

                            break;
                        case "A":
                            if (routineLoaded == true)
                            {
                                correctUserInput = true;
                                goToMenu = "RunRoutineSecondary";
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice");
                            }
                            break;
                        case "W":
                            correctUserInput = true;
                            goToMenu = "SelectRoutine";
                            Console.WriteLine();
                            break;
                        case "E":
                            goToMenu = "EditRoutine";
                            Console.WriteLine();
                            correctUserInput = true;
                            break;
                        case "R":
                            goToMenu = "CreateRoutine";
                            Console.WriteLine();
                            correctUserInput = true;
                            break;

                        case "T":
                            Exit();
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("Invalid option, try again.");
                            Console.WriteLine();
                            break;
                    }
                }
                return goToMenu;
            }
            void DisplayMainMenuChoices()
            {
                Console.WriteLine("Main Menu");
                Console.WriteLine("---------");
                Console.WriteLine();

                if (routineLoaded) Console.WriteLine("Loaded Default Routine - " + loadedRoutineName);
                Console.WriteLine();

                if (routineLoaded)
                {
                    if (routineTypeDaily)
                    {
                        Console.WriteLine("Q - start the default daily routine");
                        Console.WriteLine("A - resume the previous daily routine");
                    }
                    else
                    {
                        Console.WriteLine("Q - resume the default cycle routine");
                        Console.WriteLine("A - start the default cycle routine from it's starting point");
                    }
                }

                Console.WriteLine("W - select the default startup routine.");
                Console.WriteLine("E - edit an existing routine");
                Console.WriteLine("R - create a new routine");
                Console.WriteLine("T - exit");

                Console.WriteLine();
            }
            void Exit()
            {
                Console.WriteLine();
                Console.WriteLine(".");
                Console.WriteLine(".");
                Console.WriteLine(".");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}