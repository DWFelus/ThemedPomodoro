# "Themed Pomodoro"

  *Giving a distinct purpose and theme to Pomodoro sessions to stay on focused on priorities. Tracking progress and designing schedule to support freelancing, learning or working with use of an enhaced Pomodoro Technique. It encourages rethinking of the time dedicated to a range of activities in order to maintain balance.*
 

**MVP 1.0 To-Do/Feature List:**
 

1. Main menu:

- [ ] Q to start/resume the default routine (only if loaded the default one)
- [ ] A to run alternate option start/resume (only if loaded the default one)
- [ ] W to select the default Pomodoro routine
- [ ] E to edit an existing routine
- [ ] Explain how it works briefly
- [ ] *Main menu* **COMPLETE**

  
2. Create and Save Routine:
- [ ] Explain the dictionary:
   - [ ] Session: a Pomodoro session followed by a short or a long break
  - [ ] Set: intended number of repetitions of the sessions ended by a long break
  - [ ] Routine: sets combined into one day worth of activities like work or learning
- [ ] Define the length of the Pomodoro session (in minutes)
- [ ] Define the length of the short break (in minutes)
- [ ] Set a hard limit on the number of daily sessions (so it's not more than 24 hours)
- [ ] Define how many sessions in a single set followed by a long break (for example, six sessions followed by one long break)
- [ ] Define the number of sets (if two then for example 6 sessions + 1 one long break + 6 sessions
- [ ] Calculate full session length
- [ ] Calculate work/break ratio
- [ ] Calculate hours/minutes worked
- [ ] Calculate hours/minutes resting
- [ ] Inform how many hours it will take to complete all sessions for the day
- [ ] Select mode: daily/cycle
- [ ] If daily mode is selected, then the number of sessions to input is fixed
- [ ] Else process of adding sessions is limited to the number of sessions that can fit in the day
- [ ] Input every session theme one by one daily as an array or a list, (break with some symbol to stop adding to the list) by asking the session numbers and sessions in a row
- [ ] Automatically add X/X when typing the same type of and +x/x if the tasks are disjointed
- [ ] Preview
- [ ] Ask if the user wishes to correct one of the settings
- [ ] Request to name the routine to be saved (to the root folder? or documents?)
- [ ] Go back to the main menu
- [ ] *Create and Save Routine* **COMPLETE**

  

3. *Load Default Routine* into the config file
- [ ] Request the name of the routine to be autoloaded (from the root folder? documents?) during the startup.
- [ ] Go back to the main menu
- [ ] *Load Default Routine* **COMPLETE**

  
4. Run Default Routine

- [ ] Prompt there’s no routine selected as default
- [ ] Display the complete routine
- [ ] When in daily mode, ask to resume the previous routine from a session or to start a new one
- [ ] When in cycle mode, ask to resume the previous cycle or to start a new one
- [ ] Track current progress and save it into the routine file
- [ ] Design basic count-down function  

5. Edit and Save Routine

- [ ] Load the routine that the user has typed in
- [ ] Preview it
- [ ] Ask if the user wants to edit a segment
- [ ] If the user wants to change the type to cycle or daily mode, warn that the session themes will be lost
- [ ] Go back to the main menu  

**Update To-Do List:**

v1.1

- [ ] Session controls: Pause/resume, stop, next
- [ ]  Better progress visualization (a progress bar next to each scheduled session)
- [ ] add tracking stats for encouragement

  v2.0
- [ ] Convert into a WPF app with GUI
