using System.Text.RegularExpressions;

namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void CreateRoutine()
        {
            string routineName = "";
            string routineMode = "";

            int sessionLength;
            int sessionsCount;

            int shortBreakLength;
            int shortBreakCount;

            int longBreakLength;
            int longBreakCount;

            int setsCount;

            // variables for calculations 
            int routineHours;
            int routineLeftoverMinutes;
            double ratio;

            int focusRoutineMinutes;
            int shortBreaksRoutineMinutes;
            int longBreaksRoutineMinutes;

            int totalRoutineMinutes;

            string pomodoroDefaultFocusSessionName = "";

            int timeMin = 1;
            int timeMax = 90;
            int amountMin = 1;
            int amountMax = 15;

            // CreateRoutine Start

            InitialMessage();
            DefineRoutine();

            string[] dailyThemes = new string[sessionsCount * setsCount];
            List<string> cycleThemes = new();

            SelectMode();

            List<string> userThemes = new();

            ProcessThemes();

            List<string> generatedRoutine = new();

            ExportRoutine();

            //
            // Session parameter input, validation, calculation and modification
            //

            void DefineRoutine()
            {
                InputSessionLength();
                InputShortBreakLength();
                InputLongBreakLength();
                InputSessionAmount();
                InputSetsAmount();
                CalculateRoutine();
                DisplayRoutineCalculations();
                OptionModRoutine();
            }

            void InputSessionLength()
            {
                Console.WriteLine("---");
                Console.WriteLine("Enter the length of a focus session in minutes, ranging from {0} to {1}:", timeMin, timeMax);
                sessionLength = validateNumericInput(timeMin, timeMax);
                Console.WriteLine("Session length is {0} minutes.", sessionLength);
                Console.WriteLine("---");
                Console.WriteLine();
            }

            void InputShortBreakLength()
            {
                Console.WriteLine("---");
                Console.WriteLine("Enter the length of a short break in minutes, ranging from ranging from {0} to {1}:", timeMin, timeMax);
                shortBreakLength = validateNumericInput(timeMin, timeMax);
                Console.WriteLine("Short break length is {0} minutes.", shortBreakLength);
                Console.WriteLine("---");
                Console.WriteLine();
            }

            void InputLongBreakLength()
            {
                Console.WriteLine("---");
                Console.WriteLine("Enter the length of a long break in minutes, ranging from {0} to {1}:", timeMin, timeMax);
                longBreakLength = validateNumericInput(timeMin, timeMax);
                Console.WriteLine("Long break length is {0} minutes.", longBreakLength);
                Console.WriteLine("---");
                Console.WriteLine();
            }

            void InputSessionAmount()
            {
                Console.WriteLine("---");
                Console.WriteLine("Enter the amount of sessions within a single set (sets are separated by long breaks), ranging from {0} to {1}:", amountMin, amountMax);
                sessionsCount = validateNumericInput(amountMin, amountMax);
                Console.WriteLine("There will be {0} sessions within a single set.", sessionsCount);
                Console.WriteLine("---");
                Console.WriteLine();
            }

            void InputSetsAmount()
            {
                Console.WriteLine("---");
                Console.WriteLine("Enter the amount of sets (sets are separated by long breaks), ranging from {0} to {1}:", amountMin, amountMax);
                setsCount = validateNumericInput(amountMin, amountMax);
                Console.WriteLine("There will be {0} sets.", setsCount);
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

            void CalculateRoutine()
            {
                shortBreakCount = sessionsCount - 1;
                longBreakCount = setsCount - 1;

                focusRoutineMinutes = (sessionsCount * sessionLength) * setsCount;
                shortBreaksRoutineMinutes = (shortBreakCount * shortBreakLength) * setsCount;
                longBreaksRoutineMinutes = (longBreakCount * longBreakLength);

                totalRoutineMinutes = focusRoutineMinutes + shortBreaksRoutineMinutes + longBreaksRoutineMinutes;

                routineHours = totalRoutineMinutes / 60;
                routineLeftoverMinutes = totalRoutineMinutes % 60;
                ratio = (Convert.ToDouble(sessionLength) / (Convert.ToDouble(sessionLength) + Convert.ToDouble(shortBreakLength))) * 100;
            }

            void OptionModRoutine()
            {
                Console.WriteLine("Do you accept this routine?");
                Console.WriteLine("---");

                bool correctUserInput = false;
                while (!correctUserInput)
                {
                    Console.WriteLine("Y - Yes, it's great.");
                    Console.WriteLine("N - No, I need to work on the timing.");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string mainMenuUserInput = Console.ReadLine().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    switch (mainMenuUserInput)
                    {

                        case "Y":
                            correctUserInput = true;
                            Console.WriteLine();
                            break;
                        case "N":
                            correctUserInput = true;
                            DefineRoutine();
                            Console.WriteLine();
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("Invalid option, try again.");
                            Console.WriteLine();
                            break;
                    }
                }
            }

            //
            // Selecting mode, acquiring theme input
            //

            void SelectMode()
            {
                DisplayModesMessage();

                bool correctUserInput = false;
                while (!correctUserInput)
                {
                    Console.WriteLine("Press a button to set a mode to your routine:");
                    Console.WriteLine("D - Daily Mode.");
                    Console.WriteLine("C - Cycle Mode.");
                    Console.WriteLine("P - Pomodoro Classic Mode");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string mainMenuUserInput = Console.ReadLine().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    switch (mainMenuUserInput)
                    {

                        case "D":
                            correctUserInput = true;
                            routineMode = "daily";
                            Console.Clear();
                            RoutineMode(routineMode);
                            Console.WriteLine();
                            break;
                        case "C":
                            correctUserInput = true;
                            routineMode = "cycle";
                            Console.Clear();
                            RoutineMode(routineMode);
                            Console.WriteLine();
                            break;

                        case "P":
                            correctUserInput = true;
                            routineMode = "pomodoro";
                            Console.Clear();
                            RoutineMode(routineMode);
                            Console.WriteLine();
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("Invalid option, try again.");
                            Console.WriteLine();
                            break;
                    }
                }
            }

            void RoutineMode(string mode)
            {
                cycleThemes.Clear();
                Array.Clear(dailyThemes);

                if (mode == "daily")
                {
                    InputDailyMode();
                }

                else if (mode == "cycle")
                {
                    InputCycleMode();
                }

                else if (mode == "pomodoro")
                {
                    InputPomodoroMode();
                }

                bool correctUserInput = false;
                while (!correctUserInput)
                {
                    Console.WriteLine("Is the above correct?");
                    Console.WriteLine("Y - Yes.");
                    Console.WriteLine("N - No.");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string mainMenuUserInput = Console.ReadLine().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    switch (mainMenuUserInput)
                    {

                        case "Y":
                            correctUserInput = true;
                            Console.Clear();
                            break;
                        case "N":
                            correctUserInput = true;
                            Console.Clear();
                            RoutineMode(routineMode);
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("Invalid option, try again.");
                            Console.WriteLine();
                            break;
                    }
                }
            }

            void InputDailyMode()
            {
                for (int i = 0; i < dailyThemes.Length; i++)
                {
                    Console.Write("Session theme {0}/{1}: ", i + 1, dailyThemes.Length);
                    dailyThemes[i] = Console.ReadLine() + " ";
                }
            }

            void InputCycleMode()
            {
                Console.WriteLine("Stop the input loop by entering a double dash (\"--\")");
                for (int i = 0; ; i++)
                {
                    Console.Write("Enter theme of session {0}: ", i + 1);
                    cycleThemes.Add(Console.ReadLine() + " ");
                    if (i > 0 && cycleThemes[i] == "-- ")
                    {
                        cycleThemes.Remove(cycleThemes[i]);
                        break;
                    }
                }

                Console.Clear();

                for (int i = 0; i < cycleThemes.Count; i++)
                {
                    Console.WriteLine("Session{0}: {1}", i + 1, cycleThemes[i]);
                }
            }

            void InputPomodoroMode()
            {
                Console.Write("Custom session name: ");
                pomodoroDefaultFocusSessionName = Console.ReadLine() + " ";
            }

            //
            // Processing Themes (numbering, naming)
            //

            void ProcessThemes()
            {
                StreamlineThemes();
                OccurrenceCount();
                TrimThemes();
                NameRoutine();
                PressEnter();
            }

            void StreamlineThemes()
            {
                if (routineMode == "daily")
                {
                    userThemes = dailyThemes.ToList();
                }

                else if (routineMode == "cycle")
                {
                    userThemes = cycleThemes.ToList();
                }

                else if (routineMode == "pomodoro")
                {
                    userThemes.Add(pomodoroDefaultFocusSessionName);
                }
            }

            void OccurrenceCount()
            {
                var occurrences = userThemes.GroupBy(x => x).ToDictionary(y => y.Key, z => z.Count()); // storing occurences in a dictionary

                List<string> userThemesByOccurence = new();
                Console.Clear();
                Console.WriteLine("Full Routine Preview");
                Console.WriteLine("---");
                for (int i = 0; i < userThemes.Count; i++)
                {
                    if (occurrences.ContainsKey(userThemes[i]))
                    {
                        userThemesByOccurence.Add(userThemes[i]);
                        if (occurrences[userThemes[i]] > 1)
                        {
                            userThemes[i] += userThemesByOccurence.Count(x => x == userThemes[i]) + "/" + occurrences[userThemes[i]];
                        }
                        Console.WriteLine(userThemes[i]);
                    }
                }
            }

            void TrimThemes()
            {
                for (int i = 0; i < userThemes.Count; i++)
                {
                    userThemes[i] = userThemes[i].Trim();
                }
            }

            void NameRoutine()
            {
                Console.WriteLine();
                Console.Write("Enter a name for your routine: ");
                bool inputTestPass = false;
                string input = "";
                do
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    input = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    input = input.Trim().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (input == null || !Regex.IsMatch(input, @"[a-zA-Z0-9]"))
                    {
                        Console.WriteLine("Name field cannot be empty and must contain at least one letter or number");
                    }

                    else
                    {
                        Console.WriteLine(input);
                        routineName = input;
                        inputTestPass = true;
                    }

                } while (inputTestPass == false);
            }

            void PressEnter()
            {
                Console.WriteLine("Press Enter to Continue...");
                Console.Read();
            }

            //
            // Exporting routine to a file
            //

            void ExportRoutine()
            {
                GenerateRoutine();
                SaveRoutine();
            }

            void GenerateRoutine()
            {
                string focusSession = "--FOCUS";
                string shortBreak = "--SHORT";
                string longBreak = "--LONG";


                for (int i = 0; i < setsCount; i++)
                {
                    for (int j = 0; j < sessionsCount; j++)
                    {
                        generatedRoutine.Add(focusSession);
                        generatedRoutine.Add(shortBreak);
                    }
                    generatedRoutine.RemoveAt(generatedRoutine.Count - 1);
                    if (i != setsCount - 1)
                    {
                        generatedRoutine.Add(longBreak);
                    }
                }
            }

            void SaveRoutine()
            {
                List<string> routine = new();
                routine.Add(routineName);
                routine.Add(routineMode);
                routine.Add(sessionLength.ToString());
                routine.Add(shortBreakLength.ToString());
                routine.Add(longBreakLength.ToString());

                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    + @"\ThemedPomodoro\" + routineName + @"\";
                System.IO.Directory.CreateDirectory(filePath);
                TextWriter tw = new StreamWriter(filePath + routineName + "_config.txt");
                for (int i = 0; i < routine.Count; i++)
                {
                    tw.WriteLine(routine[i]);
                }
                tw.Close();

                TextWriter tw2 = new StreamWriter(filePath + routineName + "_userThemes.txt");
                for (int i = 0; i < userThemes.Count; i++)
                {
                    tw2.WriteLine(userThemes[i]);
                }
                tw2.Close();

                TextWriter tw3 = new StreamWriter(filePath + routineName + "_routine.txt");
                for (int i = 0; i < generatedRoutine.Count; i++)
                {
                    tw3.WriteLine(generatedRoutine[i]);
                }
                tw3.Close();

                TextWriter tw4 = new StreamWriter(filePath + routineName + "_lastSession.txt");
                tw4.WriteLine("0");
                tw4.Close();
            }

            //
            // Messages
            //

            void InitialMessage()
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Creating a routine");
                Console.WriteLine("------------------");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Here is where you are going to create your own routine to run from the main menu.");
                Console.WriteLine("Carefully answer each query to create a schedule.");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Session: a Pomodoro session followed by a short or a long break");
                Console.WriteLine("Set: intended number of repetitions of the sessions ended by a long break");
                Console.WriteLine("Routine: sets combined into one day worth of activities like work or learning");
                Console.WriteLine();
            }

            void DisplayModesMessage()
            {
                Console.Clear();
                Console.WriteLine("Choose mode a mode for this routine.");
                Console.WriteLine("---");
                Console.WriteLine("Daily mode is great when you have a set, strict daily routine and you spend your days in similar ways.");
                Console.WriteLine("It's a great option when you want to spend a set amount of time on limited number of activities.");
                Console.WriteLine("By choosing daily mode you're limited to entering fixed amount of themes for the sessions, so your days will have a set routine.");
                Console.WriteLine("Themed Pomodoro will remind you what you are supposed to be doing at this time.");
                Console.WriteLine();
                Console.WriteLine("Cycle mode is useful when number of activities you want to keep tabs on exceed the number of sessions within a day.");
                Console.WriteLine("Your next day will begin with a session you left off on the previous day, and the sessions will continue in order.");
                Console.WriteLine("Choosing cycle mode will let you enter infinite amount of themes for your sessions. All within a reason.");
                Console.WriteLine("Themed Pomodoro will help you to retain knowledge and skills through systematic repetition.");
                Console.WriteLine();
                Console.WriteLine("Pomodoro Classic mode works as in the original pomodoro technique, focus sessions have no theme to it.");
                Console.WriteLine("---");
                Console.WriteLine();
            }

            void DisplayRoutineCalculations()
            {
                Console.Clear();
                Console.WriteLine("Number of sets: " + setsCount);
                Console.WriteLine("Number of sessions within a set: " + sessionsCount);
                Console.WriteLine("Length of focus session: " + sessionLength + " minutes.");
                Console.WriteLine("Total number of focus sessions: " + (setsCount * sessionsCount));
                Console.WriteLine("Estimated time spent in focus mode: {0}h {1}min", (focusRoutineMinutes / 60), (focusRoutineMinutes % 60));
                Console.WriteLine("Estimated time spent on a break: {0}h {1}min", ((shortBreaksRoutineMinutes + longBreaksRoutineMinutes) / 60),
                                                                                      ((shortBreaksRoutineMinutes + longBreaksRoutineMinutes) % 60)); Console.WriteLine();
                Console.WriteLine("---");
                Console.WriteLine("Your routine will take {0}h {1}min to complete.", routineHours, routineLeftoverMinutes);
                Console.WriteLine("Average focus percentage: {0}%.", Convert.ToInt32(ratio));
                Console.WriteLine();
            }
        }
    }
}