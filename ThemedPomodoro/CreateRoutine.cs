namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void CreateRoutine()
        {
            int sessionLength;
            int sessionsCount;

            int shortBreakLength;
            int shortBreakCount;

            int longBreakLength;
            int longBreakCount;

            int setsCount;

            int focusRoutineMinutes;
            int shortBreaksRoutineMinutes;
            int longBreaksRoutineMinutes;

            int totalRoutineMinutes;

            bool routineTypeDaily;

            int timeMin = 1;
            int timeMax = 90;
            int amountMin = 1;
            int amountMax = 15;

            InitialMessage(); // help the user to understand how it works
            DefineRoutine(); // get the input from the user

            string[] dailyThemes = new string[sessionsCount * setsCount];
            List<string> cycleThemes = new();

            SelectMode(); //choose between daily or cycle mode

            void RoutineMode(bool mode)
            {
                if (mode)
                {
                    InputDailyMode();
                }
                else
                {
                    cycleThemes.Clear();
                    InputCycleMode();
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
                            RoutineMode(routineTypeDaily);
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
                Console.WriteLine("Stop the input loop by entering a dot (\".\")");
                for (int i = 0; ; i++)
                {
                    Console.Write("Enter theme of session {0}: ", i + 1);
                    cycleThemes.Add(Console.ReadLine() + " ");
                    if (i > 0 && cycleThemes[i] == ". ")
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

            void SelectMode()
            {
                DisplayModesMessage();

                bool correctUserInput = false;
                while (!correctUserInput)
                {
                    Console.WriteLine("Press a button to set a mode to your routine:");
                    Console.WriteLine("D - Daily Mode.");
                    Console.WriteLine("C - Cycle Mode.");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    string mainMenuUserInput = Console.ReadLine().ToUpper();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    switch (mainMenuUserInput)
                    {

                        case "D":
                            correctUserInput = true;
                            routineTypeDaily = true;
                            Console.Clear();
                            RoutineMode(routineTypeDaily);
                            Console.WriteLine();
                            break;
                        case "C":
                            correctUserInput = true;
                            routineTypeDaily = false;
                            Console.Clear();
                            RoutineMode(routineTypeDaily);
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

            void DefineRoutine()
            {
                InputSessionLength();
                InputShortBreakLength();
                InputLongBreakLength();
                InputSessionAmount();
                InputSetsAmount();
                CalculateRoutine();
                OptionModRoutine();
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

            void CalculateRoutine()
            {
                shortBreakCount = sessionsCount - 1;
                longBreakCount = setsCount - 1;

                focusRoutineMinutes = (sessionsCount * sessionLength) * setsCount;
                shortBreaksRoutineMinutes = (shortBreakCount * shortBreakLength) * setsCount;
                longBreaksRoutineMinutes = (longBreakCount * longBreakLength);

                totalRoutineMinutes = focusRoutineMinutes + shortBreaksRoutineMinutes + longBreaksRoutineMinutes;

                Console.WriteLine();// list all the components
                Console.WriteLine("Number of sets: " + setsCount);
                Console.WriteLine("Number of sessions within a set: " + sessionsCount);
                Console.WriteLine("Length of focus session: " + sessionLength + " minutes.");
                Console.WriteLine("Total number of focus sessions: " + (setsCount * sessionsCount));
                Console.WriteLine("Estimated time spent in focus mode: {0}h {1}min", (focusRoutineMinutes / 60), (focusRoutineMinutes % 60));
                Console.WriteLine("Estimated time spent on a break: {0}h {1}min", ((shortBreaksRoutineMinutes + longBreaksRoutineMinutes) / 60),
                                                                                      ((shortBreaksRoutineMinutes + longBreaksRoutineMinutes) % 60)); Console.WriteLine();
                int routineHours = totalRoutineMinutes / 60;
                int routineLeftoverMinutes = totalRoutineMinutes % 60;
                Console.WriteLine("---");
                Console.WriteLine("Your routine will take {0}h {1}min to complete.", routineHours, routineLeftoverMinutes);

                double ratio = (Convert.ToDouble(sessionLength) / (Convert.ToDouble(sessionLength) + Convert.ToDouble(shortBreakLength))) * 100;
                Console.WriteLine("Average focus percentage: {0}%.", Convert.ToInt32(ratio));
                Console.WriteLine();
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
        }

        private static void DisplayModesMessage()
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
        }
    }
}
