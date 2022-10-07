using System.Text.RegularExpressions;

namespace ThemedPomodoro
{
    public partial class Mode
    {
        public static void SelectRoutine()
        {
            Console.Clear();
            string configRootFolder = Environment.CurrentDirectory + @"\config\";
            List<string> dirNames = new();
            string[] directories = Directory.GetDirectories(configRootFolder);

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Select the Default Routine");
            Console.WriteLine("--------------------------");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Here is where you can select which routine is going to be loaded to start by default.");
            Console.WriteLine("Enter the name of the routine created previously.");

            DisplayDirectories();
            LoadRoutine();

            void LoadRoutine()
            {
                Console.WriteLine();
                Console.Write("Enter the name of the routine you wish to load: ");
                Console.WriteLine("Cancel by typing in a double dash (--)");
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
                    if (input == "--")
                    {
                        inputTestPass = true;
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
                        Console.WriteLine("Press a key to return to Main Menu.");
                        LoadAsDefaultRoutine(input);
                        inputTestPass = true;
                        Console.ReadKey();
                    }

                } while (inputTestPass == false);
            }

            void LoadAsDefaultRoutine(string input)
            {
                File.Delete(configRootFolder + "config.txt");
                TextWriter tw = new StreamWriter(configRootFolder + "config.txt");
                tw.WriteLine(input);
                tw.Close();
            }

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
        }
    }
}
