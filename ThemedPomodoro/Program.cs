using static ThemedPomodoro.Mode;

bool routineLoaded = true;
bool routineTypeDaily = false;
bool correctUserInput = false;
string validMainMenuUserChoice;
string loadedRoutineName = "test";
MainMenu();

void MainMenu()
{
    ReadGlobalConfigFile();

    Console.WriteLine("Themed Pomodoro");
    Console.WriteLine("---------------");
    Console.WriteLine();

    DisplayMainMenuChoices();
    validMainMenuUserChoice = MainMenuUserInput();
    GoToMenu(validMainMenuUserChoice);

    void GoToMenu(string vMainMenuUserChoice)
    {
        switch (vMainMenuUserChoice)
        {
            case "RunRoutinePrimary":
                RunRoutine("primary");
                break;
            case "RunRoutineSecondary":
                RunRoutine("secondary");
                break;
            case "SelectRoutine":
                SelectRoutine();
                break;
            case "EditRoutine":
                EditRoutine();
                break;
            case "CreateRoutine":
                CreateRoutine();
                break;
        }
    }
    void ReadGlobalConfigFile()
    {
        Console.Write("");
    }
    string MainMenuUserInput()
    {
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

Exit();
