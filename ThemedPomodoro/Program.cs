using static ThemedPomodoro.Mode;
public partial class Mode
{
    public static void Main()
    {
        //TestBox();
        MainMenu();
        static void MainMenu()
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 130;
            string rootFolderPath = Environment.CurrentDirectory;
            string configFolderPath = rootFolderPath + @"\config";
            bool rootExists = false;
            string defaultRoutine = "";
            string lastSession = "";

            bool routineLoaded = false;
            bool routineTypeDaily = false;
            string validMainMenuUserChoice;
            bool lastSessionPresent = false;
            bool routinesExist = false;

            bool dailySecondaryAvailable = false;
            bool cycleSecondaryAvailable = false;
            bool cycleTeriaryAvailable = false;

            Config();

            Console.Clear();

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Themed Pomodoro");
            Console.WriteLine("---------------");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            DisplayMainMenuChoices(); // displaying the options to the user depending on the loaded routine
            validMainMenuUserChoice = MainMenuUserInput(); // getting input from the user
            GoToMenu(validMainMenuUserChoice); // going into sub menus

            // ---------------------------------------------------------------------------------------------------------
            // ----------------------------------------------- FUNCTIONS -----------------------------------------------
            // ---------------------------------------------------------------------------------------------------------

            //
            // Setup
            //

            void Config() // Config Hub
            {
                CheckForConfigFolder();
                GenerateEmptyConfigFile();   //... if there is none.
                LoadConfigFile();
                CheckForSecondaryAvailibilty();
                CheckForExistingRoutines();
            }

            void CheckForConfigFolder()
            {
                if (!Directory.Exists(configFolderPath))
                {
                    Directory.CreateDirectory(configFolderPath);
                    rootExists = true;
                }

                else
                {
                    rootExists = true;
                }
            }

            void GenerateEmptyConfigFile()
            {
                if (rootExists && !File.Exists(configFolderPath + @"\config.txt"))
                {
                    TextWriter tw = new StreamWriter(configFolderPath + @"\config.txt");
                    tw.WriteLine("");
                    tw.Close();
                }
            }

            void LoadConfigFile()
            {
                TextReader tr = new StreamReader(configFolderPath + @"\config.txt");
                defaultRoutine = tr.ReadLine();
                tr.Close();
                Console.WriteLine("defaultRoutine: " + defaultRoutine); //debug
                if (!string.IsNullOrEmpty(defaultRoutine))
                {
                    routineLoaded = true;
                    CheckForLastSesion();
                    if (File.Exists(configFolderPath + @"\" + defaultRoutine + @"\" + defaultRoutine + @"_config.txt"))
                    {
                        if (File.ReadLines(configFolderPath + @"\" + defaultRoutine + @"\" + defaultRoutine + @"_config.txt").ElementAtOrDefault(1) == "daily")
                        {
                            routineTypeDaily = true;
                        }

                        else
                        {
                            routineTypeDaily = false;
                        }
                    }
                    else
                    {
                        routineLoaded = false;
                    }

                }

                else
                {
                    defaultRoutine = "No Routine Loaded";
                    routineLoaded = false;
                }
                Console.WriteLine("lastSession: " + lastSession); //debug
            }

            void CheckForLastSesion()
            {
                var rLastSessionPath = configFolderPath + @"\" + defaultRoutine + @"\" + defaultRoutine + @"_lastSession.txt";
                if (File.Exists(rLastSessionPath))
                {
                    TextReader tr1 = new StreamReader(configFolderPath + @"\" + defaultRoutine + @"\" + defaultRoutine + @"_lastSession.txt");
                    lastSession = tr1.ReadLine();
                    tr1.Close();
                    lastSessionPresent = true;
                }

                else
                {
                    lastSession = "0";
                    lastSessionPresent = false;
                }
            }

            void CheckForSecondaryAvailibilty()
            {
                string rLastSessionPath = configFolderPath + @"\" + defaultRoutine + @"\" + defaultRoutine + @"_lastSession.txt";
                string rBeginAtPath = configFolderPath + @"\" + defaultRoutine + @"\" + defaultRoutine + @"_beginAt.txt";
                string rLastTickPath = configFolderPath + @"\" + defaultRoutine + @"\" + defaultRoutine + @"_lastTick.txt";
                if (routineLoaded && !routineTypeDaily)
                {
                    if (!File.Exists(rLastSessionPath))
                    {
                        cycleSecondaryAvailable = true;
                    }
                    else
                    {
                        cycleSecondaryAvailable = false;
                    }
                }

                if (routineLoaded && routineTypeDaily)
                {
                    if (!File.Exists(rBeginAtPath))
                    {
                        dailySecondaryAvailable = false;
                    }
                    else
                    {
                        dailySecondaryAvailable = true;
                    }
                }

                if (routineLoaded && !routineTypeDaily)
                {
                    if (File.Exists(rBeginAtPath))
                    {
                        cycleTeriaryAvailable = true;
                    }
                    else
                    {
                        cycleTeriaryAvailable = false;
                    }
                }

                if (routineLoaded)
                {
                    if (File.Exists(rLastTickPath))
                    {
                        dailySecondaryAvailable = true;
                        cycleTeriaryAvailable = true;
                    }
                }
            }

            void CheckForExistingRoutines()
            {
                string[] directories = Directory.GetDirectories(configFolderPath);
                if (directories.Length == 0)
                {
                    routinesExist = false;
                }
                else
                {
                    routinesExist = true;
                }
            }

            //
            // Menu
            //

            void GoToMenu(string vMainMenuUserChoice)
            {
                switch (vMainMenuUserChoice)
                {
                    case "RunRoutinePrimary":
                        RunRoutine("primary", defaultRoutine, routineTypeDaily, int.Parse(lastSession));
                        MainMenu();
                        break;
                    case "RunRoutineSecondary":
                        RunRoutine("secondary", defaultRoutine, routineTypeDaily, int.Parse(lastSession));
                        MainMenu();
                        break;
                    case "CycleResume":
                        RunRoutine("teriary", defaultRoutine, routineTypeDaily, int.Parse(lastSession));
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

            string MainMenuUserInput()
            {
                bool correctUserInput = false;
                string goToMenu = "";
                while (!correctUserInput)
                {
                    Console.Write("Enter a letter to choose an option: ");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string mainMenuUserInput = Console.ReadLine().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    switch (mainMenuUserInput)
                    {
                        case "Q":
                            if (!lastSessionPresent && !routineTypeDaily)
                            {
                                Console.WriteLine("Invalid choice");
                            }

                            else if (routineLoaded == true)
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

                            if (routineLoaded == true && routineTypeDaily && dailySecondaryAvailable == false)
                            {
                                Console.WriteLine("Invalid choice");
                            }

                            /*else if (routineLoaded == true && !routineTypeDaily && cycleSecondaryAvailable == false)
                            {
                                Console.WriteLine("Invalid choicedd");
                            }*/

                            else if (routineLoaded == true)
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

                        case "Z":
                            if (routineLoaded == true && !routineTypeDaily && cycleTeriaryAvailable)
                            {
                                correctUserInput = true;
                                goToMenu = "CycleResume";
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice");
                            }
                            break;
                        case "W":
                            if (routinesExist == true)
                            {
                                correctUserInput = true;
                                goToMenu = "SelectRoutine";
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice");
                            }

                            break;
                        case "E":
                            if (routinesExist == true)
                            {
                                goToMenu = "EditRoutine";
                                Console.WriteLine();
                                correctUserInput = true;

                            }
                            else
                            {
                                Console.WriteLine("Invalid choice");
                            }
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

                if (routineLoaded)
                {
                    Console.WriteLine("Loaded Default Routine - " + defaultRoutine);
                    Console.WriteLine();

                    if (routineTypeDaily)
                    {
                        Console.Write("Q - start/restart the default daily routine from ");
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write("FIRST THEME");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" and the ");
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("FIRST SESSION");
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (dailySecondaryAvailable)
                        {
                            Console.Write("A - resume the default ");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("INTERRUPTED");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" daily routine from ");
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write("LAST THEME");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" and the ");
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine("LAST SESSION");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                    }
                    else
                    {
                        if (lastSessionPresent)
                        {
                            Console.Write("Q - resume the default cycle routine from ");
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write("LAST THEME");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" and the ");
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine("FIRST SESSION");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        Console.Write("A - start/restart the default, full cycle routine from it's ");
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write("FIRST THEME");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" and ");
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("FIRST SESSION");
                        Console.BackgroundColor = ConsoleColor.Black;


                        if (cycleTeriaryAvailable)
                        {
                            Console.Write("Z - resume the default ");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("INTERRUPTED");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" cycle routine from ");
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write("LAST THEME");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" and the ");
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine("LAST SESSION");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                    }
                    Console.WriteLine();
                }

                if (routinesExist)
                {
                    Console.WriteLine("W - select the default startup routine");
                    Console.WriteLine("E - edit an existing routine");
                }
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
                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}