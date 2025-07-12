Notes:

Game loads, I entered command "savemonitor 100" when I saw no debug log output, I tried again "save 50" incase a limit was programmed in. 
Still no output to log. I also tryed the "warn <message>" and the message did not show in log output.

Ensure all logging commands exist, and that every console command should work with the logger debug messages.

You may remove this file, and leave the ForCopilot\ directory. I will use it for other things.

Do not add this directory to the .gitignore or you wont see it.

Crashed after saving, after test of the above console commands. Got further in the process this time!

Do not reference in the project files either. This folder is for me and you to communicate large files :)

================================================================================================================================================

[14:45:15.581] ------------------------------
[14:45:15.608] MelonLoader v0.7.1 Open-Beta
[14:45:15.611] OS: Windows 11
[14:45:15.611] Hash Code: 92E7D4D50B8F627A23C38EDB9DA9426B6DF6561D86F10DE03DC5831D8AC76AF0
[14:45:15.611] ------------------------------
[14:45:15.651] Game Type: MonoBleedingEdge
[14:45:15.652] Game Arch: x64
[14:45:15.652] ------------------------------
[14:45:15.652] Command-Line: 
[14:45:15.652] ------------------------------
[14:45:15.653] Core::BasePath = C:\Program Files (x86)\Steam\steamapps\common\Schedule I
[14:45:15.653] Game::BasePath = C:\Program Files (x86)\Steam\steamapps\common\Schedule I
[14:45:15.653] Game::DataPath = C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule I_Data
[14:45:15.653] Game::ApplicationPath = C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule I.exe
[14:45:15.653] Runtime Type: net35
[14:45:15.812] ------------------------------
[14:45:15.812] Game Name: Schedule I
[14:45:15.813] Game Developer: TVGS
[14:45:15.814] Unity Version: 2022.3.32f1
[14:45:15.814] Game Version: 0.3.6f6 Alternate
[14:45:15.814] ------------------------------

[14:45:16.063] Preferences Loaded!

[14:45:16.101] Loading UserLibs...
[14:45:16.107] 0 UserLibs loaded.

[14:45:16.108] Loading Plugins...
[14:45:16.111] 0 Plugins loaded.

[14:45:17.136] Hooked into static void UnityEngine.SceneManagement.SceneManager::Internal_ActiveSceneChanged(UnityEngine.SceneManagement.Scene previousActiveScene, UnityEngine.SceneManagement.Scene newActiveScene)

[14:45:18.851] Loading Mods...
[14:45:19.064] ------------------------------
[14:45:19.069] Melon Assembly loaded: '.\Mods\MixerThreholdMod-1_0_0.dll'
[14:45:19.069] SHA256 Hash: 'BD344EA52405D29FB12AABE9C3214A94BC7A605C8418FD9D78CBCA1B0CA4CB7E'

[14:45:19.088] ------------------------------
[14:45:19.089] MixerThreholdMod v1.0.0
[14:45:19.089] by mooleshacat
[14:45:19.089] Assembly: MixerThreholdMod-1_0_0.dll
[14:45:19.089] ------------------------------
[14:45:19.091] ------------------------------
[14:45:19.092] 1 Mod loaded.

[14:45:19.099] Support Module Loaded: C:\Program Files (x86)\Steam\steamapps\common\Schedule I\MelonLoader\Dependencies\SupportModules\Mono.dll
[14:45:19.131] [MixerThreholdMod] [Info][3]>=[1] === LOGGER TEST: MixerThreholdMod v1.0.0 Starting ===
[14:45:19.132] [MixerThreholdMod] [Info][3]>=[1] === MixerThreholdMod v1.0.0 Initializing ===
[14:45:19.133] [MixerThreholdMod] [Info][3]>=[1] currentMsgLogLevel: 3
[14:45:19.133] [MixerThreholdMod] [Info][3]>=[1] currentWarnLogLevel: 2
[14:45:19.133] [MixerThreholdMod] [Info][3]>=[1] Phase 1: Basic initialization complete
[14:45:19.133] [MixerThreholdMod] [Info][3]>=[1] Phase 2: Looking up MixingStationConfiguration constructor...
[14:45:19.133] [MixerThreholdMod] [Info][3]>=[1] Phase 2: Constructor found successfully
[14:45:19.133] [MixerThreholdMod] [Info][3]>=[1] Phase 3: Applying Harmony patch...
[14:45:19.138] [MixerThreholdMod] [Info][3]>=[1] Phase 3: Harmony patch applied successfully
[14:45:19.139] [MixerThreholdMod] [Info][3]>=[1] Phase 4: Registering console commands...
[14:45:19.141] [MixerThreholdMod] [Info][3]>=[3] [CONSOLE] MixerConsoleHook.Awake called
[14:45:19.141] [MixerThreholdMod] [Info][3]>=[3] [CONSOLE] MixerConsoleHook instance created
[14:45:19.141] [MixerThreholdMod] [Info][3]>=[2] [CONSOLE] Console commands registered successfully
[14:45:19.141] [MixerThreholdMod] [Info][3]>=[1] Phase 4: Console commands registered successfully
[14:45:19.142] [MixerThreholdMod] [Info][3]>=[1] === MixerThreholdMod Initialization COMPLETE ===
[14:45:22.424] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Menu
[14:45:34.897] [MixerThreholdMod] [Info][3]>=[2] [PATCH] LoadManager postfix: Game loading from C:/Users/shawn/AppData/LocalLow/TVGS/Schedule I\Saves\76561197961254567\SaveGame_2
[14:45:34.899] [MixerThreholdMod] [Info][3]>=[2] [SAVE] LoadMixerValuesWhenReady: Starting load process
[14:45:34.899] [MixerThreholdMod] [Info][3]>=[2] [SAVE] LoadMixerValuesWhenReady: Save path available: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
[14:45:34.905] [MixerThreholdMod] [Info][3]>=[2] [SAVE] LoadMixerValuesWhenReady: No save files found - starting fresh
[14:45:34.905] [MixerThreholdMod] [Info][3]>=[3] [SAVE] LoadMixerValuesWhenReady: Completed
[14:45:34.905] [MixerThreholdMod] [Info][3]>=[2] [PATCH] Load mixer values coroutine started
[14:45:42.085] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Main
[14:45:42.086] [MixerThreholdMod] [Info][3]>=[3] MixerIDManager: Reset stable ID counter to 1
[14:45:42.087] [MixerThreholdMod] [Info][3]>=[3] Current Save Path at scene load: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
[14:45:42.087] [MixerThreholdMod] [Info][3]>=[2] [SAVE] LoadMixerValuesWhenReady: Starting load process
[14:45:42.087] [MixerThreholdMod] [Info][3]>=[2] [SAVE] LoadMixerValuesWhenReady: Save path available: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
[14:45:42.088] [MixerThreholdMod] [Info][3]>=[2] [SAVE] LoadMixerValuesWhenReady: No save files found - starting fresh
[14:45:42.088] [MixerThreholdMod] [Info][3]>=[3] [SAVE] LoadMixerValuesWhenReady: Completed
[14:45:45.369] [MixerThreholdMod] [Info][3]>=[3] QueueInstance: Successfully queued MixingStationConfiguration (Total: 1)
[14:45:45.392] [MixerThreholdMod] [Info][3]>=[3] QueueInstance: Successfully queued MixingStationConfiguration (Total: 2)
[14:45:45.396] [MixerThreholdMod] [Info][3]>=[3] QueueInstance: Successfully queued MixingStationConfiguration (Total: 3)
[14:45:45.398] [MixerThreholdMod] [Info][3]>=[3] QueueInstance: Successfully queued MixingStationConfiguration (Total: 4)
[14:45:45.433] [MixerThreholdMod] [Info][3]>=[3] QueueInstance: Successfully queued MixingStationConfiguration (Total: 5)
[14:45:46.555] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Starting cleanup and processing
[14:45:46.560] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Processing 5 queued instances
[14:45:46.562] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Configuring new mixer...
[14:45:46.562] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)
[14:45:46.564] [MixerThreholdMod] [Info][3]>=[3] Assigned new ID 1 to instance: ScheduleOne.Management.MixingStationConfiguration
[14:45:46.570] [MixerThreholdMod] [Info][3]>=[3] TrackedMixers.AddAsync: Added TrackedMixer[ID:1, Created:07/12/2025 14:45:46, Updated:07/12/2025 14:45:46, HasListener:False]
[14:45:46.571] [MixerThreholdMod] [Info][3]>=[2] Created mixer with Stable ID: 1
[14:45:46.571] [MixerThreholdMod] [Info][3]>=[3] Attaching listener for Mixer 1
[14:45:46.573] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Starting for Mixer 1
[14:45:46.573] [MixerThreholdMod] [Info][3]>=[2] [SAVE] AttachListenerWhenReady: Direct event attached for Mixer 1
[14:45:46.573] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Completed for Mixer 1
[14:45:46.574] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Configuring new mixer...
[14:45:46.575] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)
[14:45:46.575] [MixerThreholdMod] [Info][3]>=[3] Assigned new ID 2 to instance: ScheduleOne.Management.MixingStationConfiguration
[14:45:46.575] [MixerThreholdMod] [Info][3]>=[3] TrackedMixers.AddAsync: Added TrackedMixer[ID:2, Created:07/12/2025 14:45:46, Updated:07/12/2025 14:45:46, HasListener:False]
[14:45:46.575] [MixerThreholdMod] [Info][3]>=[2] Created mixer with Stable ID: 2
[14:45:46.576] [MixerThreholdMod] [Info][3]>=[3] Attaching listener for Mixer 2
[14:45:46.576] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Starting for Mixer 2
[14:45:46.576] [MixerThreholdMod] [Info][3]>=[2] [SAVE] AttachListenerWhenReady: Direct event attached for Mixer 2
[14:45:46.577] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Completed for Mixer 2
[14:45:46.577] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Configuring new mixer...
[14:45:46.577] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)
[14:45:46.577] [MixerThreholdMod] [Info][3]>=[3] Assigned new ID 3 to instance: ScheduleOne.Management.MixingStationConfiguration
[14:45:46.577] [MixerThreholdMod] [Info][3]>=[3] TrackedMixers.AddAsync: Added TrackedMixer[ID:3, Created:07/12/2025 14:45:46, Updated:07/12/2025 14:45:46, HasListener:False]
[14:45:46.578] [MixerThreholdMod] [Info][3]>=[2] Created mixer with Stable ID: 3
[14:45:46.578] [MixerThreholdMod] [Info][3]>=[3] Attaching listener for Mixer 3
[14:45:46.578] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Starting for Mixer 3
[14:45:46.578] [MixerThreholdMod] [Info][3]>=[2] [SAVE] AttachListenerWhenReady: Direct event attached for Mixer 3
[14:45:46.579] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Completed for Mixer 3
[14:45:46.579] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Configuring new mixer...
[14:45:46.579] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)
[14:45:46.580] [MixerThreholdMod] [Info][3]>=[3] Assigned new ID 4 to instance: ScheduleOne.Management.MixingStationConfiguration
[14:45:46.580] [MixerThreholdMod] [Info][3]>=[3] TrackedMixers.AddAsync: Added TrackedMixer[ID:4, Created:07/12/2025 14:45:46, Updated:07/12/2025 14:45:46, HasListener:False]
[14:45:46.580] [MixerThreholdMod] [Info][3]>=[2] Created mixer with Stable ID: 4
[14:45:46.580] [MixerThreholdMod] [Info][3]>=[3] Attaching listener for Mixer 4
[14:45:46.580] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Starting for Mixer 4
[14:45:46.581] [MixerThreholdMod] [Info][3]>=[2] [SAVE] AttachListenerWhenReady: Direct event attached for Mixer 4
[14:45:46.581] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Completed for Mixer 4
[14:45:46.581] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Configuring new mixer...
[14:45:46.583] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Mixer configured successfully (1-20 range)
[14:45:46.584] [MixerThreholdMod] [Info][3]>=[3] Assigned new ID 5 to instance: ScheduleOne.Management.MixingStationConfiguration
[14:45:46.584] [MixerThreholdMod] [Info][3]>=[3] TrackedMixers.AddAsync: Added TrackedMixer[ID:5, Created:07/12/2025 14:45:46, Updated:07/12/2025 14:45:46, HasListener:False]
[14:45:46.584] [MixerThreholdMod] [Info][3]>=[2] Created mixer with Stable ID: 5
[14:45:46.585] [MixerThreholdMod] [Info][3]>=[3] Attaching listener for Mixer 5
[14:45:46.585] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Starting for Mixer 5
[14:45:46.585] [MixerThreholdMod] [Info][3]>=[2] [SAVE] AttachListenerWhenReady: Direct event attached for Mixer 5
[14:45:46.586] [MixerThreholdMod] [Info][3]>=[3] [SAVE] AttachListenerWhenReady: Completed for Mixer 5
[14:45:46.586] [MixerThreholdMod] [Info][3]>=[3] ProcessQueuedInstancesAsync: Completed successfully
[14:45:46.865] [MixerThreholdMod] [Info][3]>=[3] [SAVE] Value changed: Mixer 1 = 10
[14:45:46.867] [MixerThreholdMod] [Info][3]>=[2] [SAVE] PerformCrashResistantSave: Starting save operation
[14:45:46.868] [MixerThreholdMod] [Info][3]>=[3] [SAVE] CreateSafeBackup: Creating backup
[14:45:46.967] [MixerThreholdMod] [Info][3]>=[1] [SAVE] PerformCrashResistantSave: Successfully saved 1 mixer values
[14:45:46.967] [MixerThreholdMod] [Info][3]>=[3] [SAVE] PerformCrashResistantSave: Completed
[14:45:47.213] [MixerThreholdMod] [Info][3]>=[3] [SAVE] Value changed: Mixer 1 = 10
[14:45:47.214] [MixerThreholdMod] [Info][3]>=[3] [SAVE] TriggerSaveWithCooldown: Skipping due to cooldown
[14:47:24.990] [MixerThreholdMod] [Info][3]>=[2] [PATCH] SaveManager.Save postfix triggered
[14:47:24.991] [MixerThreholdMod] [Info][3]>=[1] [PATCH] Save path captured: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
[14:47:24.992] [MixerThreholdMod] [Info][3]>=[2] [SAVE] PerformCrashResistantSave: Starting save operation
[14:47:24.993] [MixerThreholdMod] [Info][3]>=[3] [SAVE] CreateSafeBackup: Creating backup
[14:47:24.998] [MixerThreholdMod] [Info][3]>=[2] [SAVE] CreateSafeBackup: Backup created successfully
[14:47:24.999] [MixerThreholdMod] [Info][3]>=[2] [PATCH] Crash-resistant save triggered successfully
[14:47:25.046] [MixerThreholdMod] [Info][3]>=[1] [SAVE] PerformCrashResistantSave: Successfully saved 1 mixer values
[14:47:25.047] [MixerThreholdMod] [Info][3]>=[3] [SAVE] PerformCrashResistantSave: Completed
[14:48:50.684] [MixerThreholdMod] [Info][3]>=[2] [PATCH] SaveManager.Save postfix triggered
