using System.Text.RegularExpressions;

namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void EditRoutine()
        {
            Console.Clear();
            string configRootFolder = Environment.CurrentDirectory + @"\config\";
            string routineToEdit = "";
            string routineToEditPath = "";
            string[] directories = Directory.GetDirectories(configRootFolder);

            int timeMin = 1;
            int timeMax = 90;

            int sessionLength = 0;
            int shortBreakLength = 0;
            int longBreakLength = 0;

            bool edit = true;

            List<string> dirNames = new();

            DisplayInitialMessage();
            DisplayDirectories();
            LoadRoutine();
            if (edit)
            {
                InputSessionLength();
                InputShortBreakLength();
                InputLongBreakLength();
                MakeChanges();
                Console.WriteLine("Routine edited successfully. Press ENTER to return to the main menu.");
            }

            else
            {
                Console.WriteLine("Editing Canceled. Press ENTER to return to the main menu.");
            }
            Console.ReadLine();
            Console.Clear();


            // ---------------------------------------------------------------------------------------------------------
            // ----------------------------------------------- FUNCTIONS -----------------------------------------------
            // ---------------------------------------------------------------------------------------------------------

            //
            // Read values
            //

            void LoadRoutine()
            {
                Console.WriteLine();
                Console.Write("Enter the name of the routine you wish to load: ");
                Console.WriteLine("Cancel by typing in a double dash (--)");
                bool inputTestPass = false;
                string input = "";
                do
                {
                    input = Console.ReadLine();

                    input = input.Trim().ToUpper();
                    if (input == "--")
                    {
                        inputTestPass = true;
                        edit = false;
                    }

                    else if (input == null || !Regex.IsMatch(input, @"[a-zA-Z0-9]") || input == "")
                    {
                        Console.Write("Name field cannot be empty and must contain at least one letter or number. Try again: ");
                    }

                    else if (!Directory.Exists(configRootFolder + input))
                    {
                        Console.Write("No such routine. Try again: ");
                    }

                    else
                    {
                        Console.WriteLine("Routine loaded: " + input);
                        routineToEdit = input;
                        routineToEditPath += configRootFolder + routineToEdit + @"\";
                        inputTestPass = true;
                    }

                } while (inputTestPass == false);
            }

            void InputSessionLength()
            {
                Console.Clear();
                Console.WriteLine("---");
                Console.WriteLine("Enter the length of a FOCUS session in minutes, ranging from {0} to {1}:", timeMin, timeMax);
                sessionLength = validateNumericInput(timeMin, timeMax);
                Console.WriteLine("Session length is {0} minutes.", sessionLength);
                Console.WriteLine("---");
                Console.WriteLine();
            }

            void InputShortBreakLength()
            {
                Console.Clear();
                Console.WriteLine("---");
                Console.WriteLine("Enter the length of a SHORT BREAK in minutes, ranging from ranging from {0} to {1}:", timeMin, timeMax);
                shortBreakLength = validateNumericInput(timeMin, timeMax);
                Console.WriteLine("Short break length is {0} minutes.", shortBreakLength);
                Console.WriteLine("---");
                Console.WriteLine();
            }

            void InputLongBreakLength()
            {
                Console.Clear();
                Console.WriteLine("---");
                Console.WriteLine("Enter the length of a LONG BREAK in minutes, ranging from {0} to {1}:", timeMin, timeMax);
                longBreakLength = validateNumericInput(timeMin, timeMax);
                Console.WriteLine("Long break length is {0} minutes.", longBreakLength);
                Console.WriteLine("---");
                Console.WriteLine();


            }

            int validateNumericInput(int min, int max)
            {
                bool inputTestPass = false;
                int input;
                do
                {
                    if (int.TryParse(Console.ReadLine(), out input))
                    {
                        if (input < min || input > max)
                        {
                            Console.WriteLine("Please enter a number from {0} to {1}", min, max);
                        }
                        else
                        {
                            inputTestPass = true;
                        }
                    }

                    else
                    {
                        Console.WriteLine("Please enter a number from {0} to {1}", min, max);
                    }
                } while (inputTestPass == false);


                return input;
            }

            //
            // Save values
            //

            void MakeChanges()
            {
                ChangeLine(sessionLength.ToString(), routineToEditPath + routineToEdit + "_config.txt", 2);
                ChangeLine(shortBreakLength.ToString(), routineToEditPath + routineToEdit + "_config.txt", 3);
                ChangeLine(longBreakLength.ToString(), routineToEditPath + routineToEdit + "_config.txt", 4);
            }

            void ChangeLine(string newText, string fileName, int lineToEdit)
            {
                string[] arrLine = File.ReadAllLines(fileName);
                arrLine[lineToEdit] = newText;
                File.WriteAllLines(fileName, arrLine);
            }

            //
            // Messages
            //

            void DisplayDirectories()
            {
                int count = 0;
                foreach (string directory in directories)
                {
                    dirNames.Add(directory.Remove(0, (directory.LastIndexOf('\\')) + 1));
                    Console.WriteLine((count + 1) + ": " + dirNames[count]);
                    count++;
                }
            }

            void DisplayInitialMessage()
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Edit a Routine");
                Console.WriteLine("--------------");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Here is where you can edit a part of an existing routine.");
                Console.WriteLine("If you wish to edit themes, go to \\config folder");
                Console.WriteLine("and edit \"RoutineName_userThemes.txt\" file");
                Console.WriteLine("Below, make changes to Routine's session lenght's.");
                Console.WriteLine();
            }
        }

    }
}
