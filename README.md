# "Themed Pomodoro"

  *Giving a distinct purpose and theme to Pomodoro sessions to stay on focused on priorities. Tracking progress and designing schedule to support freelancing, learning or working with use of an enhaced Pomodoro Technique. It encourages rethinking of the time dedicated to a range of activities in order to maintain balance.*
 

**MVP v1.0 To-Do/Feature List:**
 

v0.1 - Main menu:

- [x] Q to start/resume the default routine (only if loaded the default one)
- [x] A to run alternate option start/resume (only if loaded the default one)
- [x] W to select the default Pomodoro routine
- [x] E to edit an existing routine
- [x] Explain how it works briefly
- [x] *Main menu* **COMPLETE**

  
v0.2 -  Create and Save Routine:

- [x] Explain the dictionary:
  - [x] Session: a Pomodoro session followed by a short or a long break
  - [x] Set: intended number of repetitions of the sessions ended by a long break
  - [x] Routine: sets combined into one day worth of activities like work or learning
- [x] Define the length of the Pomodoro session (in minutes)
- [x] Define the length of the short break (in minutes)
- [x] Define how many sessions in a single set followed by a long break (for example, six sessions followed by one long break)
- [x] Define the number of sets (if two then for example 6 sessions + 1 one long break + 6 sessions

- [x] Calculate full session length
- [x] Calculate work/break ratio
- [x] Calculate hours/minutes worked
- [x] Calculate hours/minutes resting
- [x] Inform how many hours it will take to complete all sessions for the day

- [x] Ask if user wants to modify the counts and lengths.

- [x] Select mode: daily/cycle
- [x] If daily mode is selected, then the number of sessions to input is fixed
- [x] Else process of adding sessions is limited to the number of sessions that can fit in the day
- [x] Input every session theme one by one daily as an array or a list, (break with some symbol to stop adding to the list) by asking the session numbers and sessions in a row
- [x] Add Classic Pomodoro mode.
- [x] Automatically add X/X when typing the same type of and +x/x if the tasks are disjointed
- [x] Preview

- [X] Ask if the user wishes to correct one of the settings
- [X] Request to name the routine to be saved (to the root folder? or documents?)
- [X] Save to file.
- [x] Go back to the main menu
- [x] *Create and Save Routine* **COMPLETE**

v0.2.1
- [x] Changed default location for saving routines.


v0.3 - *Select Default Routine* into the config file

-
- [X] Create a config file or load config when exists at at startup
- [X] Request the name of the routine to be autoloaded (from the root folder? documents?) during the startup.
- [X] Go back to the main menu
- [X] *Load Default Routine* **COMPLETE**

v0.3.1
- [x] In Program.cs, show running routine option only if a routine is loaded.
- [x] In Program.cs, read routine type.

  
v0.4 - Run Default Routine

- [x] Prompt there’s no routine selected as default
- [x] Display the complete routine
- [x] When in daily mode, ask to resume the previous routine from a session or to start a new one
- [x] When in cycle mode, ask to resume the previous cycle or to start a new one
- [x] Design basic count-down function  
- [x] Track current progress and save it into the routine file
- [x] *Run Default Routine* **COMPLETE**

v0.9 - Edit and Save Routine - Working Beta

- [x] Load the routine that the user has typed in
- [x] Go back to the main menu
- [x] Play a sound when in a new session

v0.9.1 Working Beta
- [x] Make the app portable (move config and profiles to .exe root folder)
- [x] Add option to resume interrupted cycle with key "Z" (make use of beginAt variable)
- [x] Preview next session

v0.9.2 Working Beta - Testing
- [x] Added taskbar tray icon progress.

v0.9.2.1 Working Beta - Testing
- [x] Efficient tray icon code.

v0.9.2.2 Working Beta - Testing
- [x] Resuming interrupted session at exact second the application has been closed.
- [x] Fixed a bug where program skipped over initial message for the Create Routine mode.
- [x] Pause on ReadKey instead of ReadLine.

v0.9.2.3 Working Beta - Testing
- [x] Resume interrupted 1st session
- [x] Update tray icon only when reached a breakpoint, not every second.

v0.9.2.4 Working Beta - Testing
- [x] Beep once when session is nearing the end.

v0.9.2.5 Working Beta - Testing
- [x] Added session color label for current session distintion when away from the monitor.
- [x] Fixed issue where session did not resume at the correct tick.
- [x] Improved option readability in the main menu.

v1.0 - Stable
- [ ] Tested
- [ ] Build

**Update To-Do List:**

v1.1

- [ ] Better progress visualization (a progress bar next to each scheduled session)


v2.0

- [ ] Convert into a ELECTRON Javascript app with GUI
- [ ] Session controls: Pause/resume, stop, next

v2.1

- [ ] Add tracking stats for encouragement
