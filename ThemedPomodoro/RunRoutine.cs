namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void RunRoutine(string m, string d, bool t, int s)
        {
            NotifyIcon tray = new NotifyIcon();
            tray.Visible = true;
            tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\orange.ico");

            int tick = 1000; // default: 1000
            int divider = 1; // default: 1, speed up = 200;
            Console.Clear();

            //Input from Program.cs
            string defaultRoutine = d;
            string routineMode = m;
            bool routineTypeDaily = t;
            int currentThemeIndex = s;

            //Pathing

            string rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ThemedPomodoro\";
            string rPath = rootFolder + @"\" + defaultRoutine + @"\" + defaultRoutine;

            string rConfigPath = rPath + "_config.txt";
            string rLastSessionPath = rPath + "_lastSession.txt";
            string rOrderPath = rPath + "_routine.txt";
            string rThemesPath = rPath + "_userThemes.txt";
            string rBeginAtPath = rPath + "_beginAt.txt";

            int sessionsCount = File.ReadAllLines(rOrderPath).Length;
            int themesCount = File.ReadAllLines(rThemesPath).Length;

            //Reading additional Data
            string routineType = "";
            if (routineTypeDaily) routineType = "Daily"; else routineType = "Cycle";

            double sets = double.Parse(File.ReadLines(rConfigPath).ElementAtOrDefault(5));
            double sessions = double.Parse(File.ReadLines(rConfigPath).ElementAtOrDefault(6));
            double focusLength = double.Parse(File.ReadLines(rConfigPath).ElementAtOrDefault(2)) * 60;
            double shortBreakLength = double.Parse(File.ReadLines(rConfigPath).ElementAtOrDefault(3)) * 60;
            double longBreakLength = double.Parse(File.ReadLines(rConfigPath).ElementAtOrDefault(4)) * 60;

            focusLength /= divider;
            shortBreakLength /= divider;
            longBreakLength /= divider;

            //interating through these lists
            List<string> routineOrder = new();
            List<string> routineThemes = new();

            //
            string currentSession;
            string sessionType;
            int sessionCounter = 1;
            double ticksCount;
            int beginAt = 0;
            int beginIndex = 0;
            int beginThemeIndex = 0;

            DisplayInitialMessage();
            LoadRoutineToRun();

            Console.WriteLine();
            Console.WriteLine("Press any key to run the routine");
            Console.ReadKey(); // pause here before coming back
            Console.Clear();
            Console.WriteLine("Running the routine...");
            Console.WindowHeight = 5;

            Run();
            RunComplete();

            // ---------------------------------------------------------------------------------------------------------
            // ----------------------------------------------- FUNCTIONS -----------------------------------------------
            // ---------------------------------------------------------------------------------------------------------

            void RunComplete()
            {
                tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\orange.ico");
                Console.Beep(2000, 1000);
                if (File.Exists(rBeginAtPath)) File.Delete(rBeginAtPath);
                if (routineTypeDaily)
                {
                    if (File.Exists(rLastSessionPath)) File.Delete(rLastSessionPath);
                }
                Console.WriteLine("Press a key to return to the main menu.");
                Console.ReadKey();
                tray.Dispose();
            }

            //
            // Loading Routine
            //

            void LoadRoutineToRun()
            {
                PopulateLists();
                DisplayRoutineInfo();
                EstablishLastSession(routineType, routineMode);
            }

            void PopulateLists()
            {
                // reading order of sessions to run/loop
                for (int i = 0; i < sessionsCount; i++)
                {
                    string reader = File.ReadLines(rOrderPath).ElementAtOrDefault(i);
                    routineOrder.Add(reader);
                }

                // read all sessions themes to run/loop
                for (int i = 0; i < themesCount; i++)
                {
                    string reader = File.ReadLines(rThemesPath).ElementAtOrDefault(i);
                    routineThemes.Add(reader);
                }
            }

            void EstablishLastSession(string rType, string rMode)
            {
                if (rType == "Daily" && rMode == "primary")
                {
                    currentThemeIndex = 0;
                }

                if (rType == "Cycle" && rMode == "secondary")
                {
                    currentThemeIndex = 0;
                }

                if (rType == "Daily" && rMode == "secondary")
                {
                    if (File.Exists(rBeginAtPath))
                    {
                        beginAt = int.Parse(File.ReadLines(rBeginAtPath).ElementAtOrDefault(0)) + 1;
                        currentThemeIndex = int.Parse(File.ReadLines(rBeginAtPath).ElementAtOrDefault(1));
                        sessionCounter = currentThemeIndex + 1;
                        if (routineOrder[beginAt] == "--SHORT" || routineOrder[beginAt] == "--LONG")
                        {
                            beginAt++;
                        }
                        if (beginAt > routineOrder.Count)
                        {
                            beginAt = 0;
                            sessionCounter = 1;
                        }
                    }
                }
            }

            //
            // Running Routine
            //

            void Run()
            {

                for (int i = beginAt; i < routineOrder.Count; i++)
                {
                    if (sessionCounter > (sets * sessions))
                    {
                        break;
                    }
                    beginIndex = i;
                    currentSession = routineOrder[i];
                    if (currentSession == "--FOCUS")
                    {

                        if (currentThemeIndex >= themesCount)
                        {
                            currentThemeIndex = 0;
                            beginIndex = 0;
                        }
                        Session("--FOCUS");
                        currentThemeIndex++;
                        sessionCounter++;

                        SaveLastSession();
                    }

                    else if (currentSession == "--SHORT")
                    {
                        Session("--SHORT");
                    }

                    else
                    {
                        Session("--LONG");
                    }
                    SaveBeginPoint();
                }

            }

            void Session(string session)
            {
                sessionType = session;
                bool firstTick = true;
                ticksCount = 0;
                if (sessionType == "--FOCUS")
                {
                    ticksCount = focusLength;
                    tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red.ico");
                    Console.Beep(700, 1000);
                }
                else if (sessionType == "--SHORT")
                {
                    tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green.ico");
                    ticksCount = shortBreakLength;
                    Console.Beep(1500, 1000);
                }
                else
                {
                    ticksCount = longBreakLength;
                    tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue.ico");
                    Console.Beep(2000, 1000);
                }
                // full session cycle below
                for (double i = ticksCount; i >= 0; i--)
                {
                    Tick(i, tick);
                    if (!firstTick && i % 60 == 0 && sessionType == "--FOCUS" && i > 60)
                    {
                        Console.Beep(500, 100);
                    }
                    firstTick = false;
                }
            }

            void SaveLastSession()
            {
                File.Delete(rLastSessionPath);
                TextWriter lastSessionWriter = new StreamWriter(rLastSessionPath);
                lastSessionWriter.WriteLine(currentThemeIndex);
                lastSessionWriter.Close();
            }

            void SaveBeginPoint()
            {
                if (File.Exists(rBeginAtPath))
                {
                    File.Delete(rBeginAtPath);
                }

                TextWriter beginWriter = new StreamWriter(rBeginAtPath);
                beginWriter.WriteLine(beginIndex);
                beginWriter.WriteLine(currentThemeIndex);
                beginWriter.Close();
            }

            void Tick(double timeLeft, int tickTock)
            {
                Console.Clear();
                var time = TimeSpan.FromSeconds((long)timeLeft);
                var vis = VisualiseProgressBar(timeLeft);
                if (sessionType == "--FOCUS")
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(routineThemes[currentThemeIndex]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" - " + vis + " - " + time.ToString("mm':'ss") + " - ");
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Focus Session " + (sessionCounter) + "/" + (sets * sessions));
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                else if (sessionType == "--SHORT")
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write("SHORT BREAK Session");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(" - " + vis + time.ToString("mm':'ss"));
                }

                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write("LONG BREAK Session");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(" - " + vis + time.ToString("mm':'ss"));
                }

                System.Threading.Thread.Sleep(tickTock);
            }

            //
            // Messages
            //

            string VisualiseProgressBar(double timeLeft)
            {
                double timeDiffrence = (ticksCount - timeLeft) * 100;
                double timeProgress = (timeDiffrence / ticksCount) / 100;
                string progressBar = "";
                int progressBarSize = 20;
                double meter = timeProgress * progressBarSize;

                for (int i = 1; i < meter; i++)
                {
                    progressBar += "#";
                }

                progressBar += ">";

                for (int i = (int)meter; i < progressBarSize; i++)
                {
                    progressBar += ".";
                }
                string res = progressBar + " " + ((int)(timeProgress * 100)) + "% ";
                return res;
            }

            void DisplayRoutineInfo()
            {
                Console.WriteLine();
                Console.WriteLine(defaultRoutine);
                Console.WriteLine("---");
                Console.WriteLine("Routine Mode: " + routineType);
                Console.WriteLine("Numer of sets: " + sets);
                Console.WriteLine("Numer of focus sessions: " + sessions);
                Console.WriteLine("Focus session length: " + focusLength / 60 + " min.");
                Console.WriteLine("Short break length: " + shortBreakLength / 60 + " min.");
                Console.WriteLine("Long break length: " + longBreakLength / 60 + " min.");
                Console.WriteLine();
                Console.WriteLine("Routine Set Preview:");
                Console.WriteLine("---");
                int foreachCount = 1;
                foreach (var item in routineThemes)
                {
                    Console.WriteLine("Session " + foreachCount + " : " + item);
                    foreachCount++;
                }
            }

            void DisplayInitialMessage()
            {
                if (m == "primary")
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Running Primary Routine Mode");
                    Console.WriteLine("----------------------------");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Running Secondary Routine Mode");
                    Console.WriteLine("------------------------------");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
        }
    }
}
