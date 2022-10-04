﻿namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void RunRoutine(string m, string d, bool t, int s)
        {
            NotifyIcon tray = new NotifyIcon();
            tray.Visible = true;
            tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\orange.ico");

            int tick = 1000; // default: 1000
            int divider = 1; // default: 1, speed up = 300;
            Console.Clear();

            //Input from Program.cs
            string defaultRoutine = d;
            string routineMode = m;
            bool routineTypeDaily = t;
            int currentThemeIndex = s;

            //Pathing

            string configRootFolder = Environment.CurrentDirectory + @"\config\";
            string rPath = configRootFolder + @"\" + defaultRoutine + @"\" + defaultRoutine;

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
            var firstMessage = true;

            DisplayInitialMessage();
            LoadRoutineToRun();
            Run();
            RunComplete();

            // ---------------------------------------------------------------------------------------------------------
            // ----------------------------------------------- FUNCTIONS -----------------------------------------------
            // ---------------------------------------------------------------------------------------------------------



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
                    ResumeInterrupted("secondary");
                }

                if (rType == "Cycle" && rMode == "teriary")
                {
                    ResumeInterrupted("teriary");
                }
            }

            void ResumeInterrupted(string s)
            {
                if (File.Exists(rBeginAtPath))
                {
                    if (s != null)
                    {
                        beginAt = int.Parse(File.ReadLines(rBeginAtPath).ElementAtOrDefault(0)) + 1;
                        currentThemeIndex = int.Parse(File.ReadLines(rBeginAtPath).ElementAtOrDefault(1));
                        sessionCounter = currentThemeIndex + 1; //
                        if (sessionCounter > sessions * sets)
                        {
                            sessionCounter = 1;
                        } //
                        if (beginAt < routineOrder.Count)
                        {
                            if (routineOrder[beginAt] == "--SHORT" || routineOrder[beginAt] == "--LONG")
                            {
                                beginAt++;
                            }
                        }

                        if (beginAt >= routineOrder.Count)
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
                        if (firstMessage)
                        {
                            Console.WriteLine();
                            Console.Write("Next session: ");
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(routineThemes[currentThemeIndex]);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.WriteLine("Press any key to run the routine...");
                            Console.ReadKey(); // pause here before coming back
                            Console.Clear();
                            Console.WriteLine("Running the routine...");
                            Console.WindowHeight = 5;
                            firstMessage = false;
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
                    var a = routineThemes[currentThemeIndex];
                    Console.Write(a);
                    Console.BackgroundColor = ConsoleColor.Black;

                    var b = " - " + vis + " - " + time.ToString("mm':'ss") + " - ";
                    Console.Write(b);
                    Console.BackgroundColor = ConsoleColor.DarkRed;

                    var c = "Focus Session " + (sessionCounter) + "/" + (sets * sessions);
                    Console.Write(c);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");

                    DisplayTotalProgress();
                    DisplayNextSession();

                }

                else if (sessionType == "--SHORT")
                {
                    var a = "SHORT BREAK Session";
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write(a);

                    var b = " - " + vis + time.ToString("mm':'ss");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(b);
                    Console.Write(" ");

                    DisplayTotalProgress();
                    DisplayNextSession();
                }

                else
                {
                    var a = "LONG BREAK Session";
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(a);

                    var b = " - " + vis + time.ToString("mm':'ss");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(b);
                    Console.Write(" ");

                    DisplayTotalProgress();
                    DisplayNextSession();
                }

                System.Threading.Thread.Sleep(tickTock);
            }

            void DisplayTotalProgress()
            {
                Console.WriteLine(" - Total Progress: {0}/{1}", beginIndex + 1, routineOrder.Count);
            }

            void DisplayNextSession()
            {
                if (beginIndex + 1 < routineOrder.Count)
                {
                    Console.Write("                                                       ");
                    Console.Write("//Next Session: ");

                    if (routineOrder[beginIndex + 1] == "--FOCUS")
                    {
                        if (sessionCounter - sets * sessions != 0 && currentThemeIndex + 1 < routineThemes.Count)
                        {
                            Console.WriteLine(routineThemes[currentThemeIndex]);
                        }

                        else
                        {
                            Console.WriteLine(routineThemes[0]);
                        }

                    }
                    else if (routineOrder[beginIndex + 1] == "--SHORT")
                    {
                        Console.WriteLine("Short Break");
                    }

                    else
                    {
                        Console.WriteLine("Long Break");
                    }
                }
            }

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
                TaskbarIconProgress(timeProgress * 100);
                return res;

            }

            void TaskbarIconProgress(double time)
            {
                if (sessionType == "--FOCUS")
                {
                    if (time > 10) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red90.ico");
                    if (time > 20) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red80.ico");
                    if (time > 30) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red70.ico");
                    if (time > 40) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red60.ico");
                    if (time > 50) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red50.ico");
                    if (time > 60) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red40.ico");
                    if (time > 70) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red30.ico");
                    if (time > 80) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red20.ico");
                    if (time > 90) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red10.ico");
                    if (time > 95) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\red05.ico");
                }

                else if (sessionType == "--SHORT")
                {
                    if (time > 10) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green90.ico");
                    if (time > 20) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green80.ico");
                    if (time > 30) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green70.ico");
                    if (time > 40) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green60.ico");
                    if (time > 50) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green50.ico");
                    if (time > 60) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green40.ico");
                    if (time > 70) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green30.ico");
                    if (time > 80) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green20.ico");
                    if (time > 90) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green10.ico");
                    if (time > 95) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\green05.ico");
                }

                else
                {
                    if (time > 10) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue90.ico");
                    if (time > 20) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue80.ico");
                    if (time > 30) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue70.ico");
                    if (time > 40) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue60.ico");
                    if (time > 50) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue50.ico");
                    if (time > 60) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue40.ico");
                    if (time > 70) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue30.ico");
                    if (time > 80) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue20.ico");
                    if (time > 90) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue10.ico");
                    if (time > 95) tray.Icon = new Icon(Environment.CurrentDirectory + "\\icons\\blue05.ico");
                }
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
