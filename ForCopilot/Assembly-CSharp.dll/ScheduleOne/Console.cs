using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.Cutscenes;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.GameTime;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Levelling;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne
{
	// Token: 0x0200023F RID: 575
	public class Console : Singleton<Console>
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00037F4C File Offset: 0x0003614C
		private static Player player
		{
			get
			{
				return Player.Local;
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00037F53 File Offset: 0x00036153
		private static void LogCommandError(string error)
		{
			Console.LogWarning(error, null);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00037F5C File Offset: 0x0003615C
		private static void LogUnrecognizedFormat(string[] correctExamples)
		{
			string text = string.Empty;
			for (int i = 0; i < correctExamples.Length; i++)
			{
				if (i > 0)
				{
					text += ",";
				}
				text = text + "'" + correctExamples[i] + "'";
			}
			Console.LogWarning("Unrecognized command format. Correct format example(s): " + text, null);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00037FB4 File Offset: 0x000361B4
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Console>.Instance != this)
			{
				return;
			}
			if (Console.commands.Count == 0)
			{
				Console.commands.Add("freecam", new Console.FreeCamCommand());
				Console.commands.Add("save", new Console.Save());
				Console.commands.Add("settime", new Console.SetTimeCommand());
				Console.commands.Add("give", new Console.AddItemToInventoryCommand());
				Console.commands.Add("clearinventory", new Console.ClearInventoryCommand());
				Console.commands.Add("changecash", new Console.ChangeCashCommand());
				Console.commands.Add("changebalance", new Console.ChangeOnlineBalanceCommand());
				Console.commands.Add("addxp", new Console.GiveXP());
				Console.commands.Add("spawnvehicle", new Console.SpawnVehicleCommand());
				Console.commands.Add("setmovespeed", new Console.SetMoveSpeedCommand());
				Console.commands.Add("setjumpforce", new Console.SetJumpMultiplier());
				Console.commands.Add("teleport", new Console.Teleport());
				Console.commands.Add("setowned", new Console.SetPropertyOwned());
				Console.commands.Add("packageproduct", new Console.PackageProduct());
				Console.commands.Add("setstaminareserve", new Console.SetStaminaReserve());
				Console.commands.Add("raisewanted", new Console.RaisedWanted());
				Console.commands.Add("lowerwanted", new Console.LowerWanted());
				Console.commands.Add("clearwanted", new Console.ClearWanted());
				Console.commands.Add("sethealth", new Console.SetHealth());
				Console.commands.Add("settimescale", new Console.SetTimeScale());
				Console.commands.Add("setvar", new Console.SetVariableValue());
				Console.commands.Add("setqueststate", new Console.SetQuestState());
				Console.commands.Add("setquestentrystate", new Console.SetQuestEntryState());
				Console.commands.Add("setemotion", new Console.SetEmotion());
				Console.commands.Add("setunlocked", new Console.SetUnlocked());
				Console.commands.Add("setrelationship", new Console.SetRelationship());
				Console.commands.Add("addemployee", new Console.AddEmployeeCommand());
				Console.commands.Add("setdiscovered", new Console.SetDiscovered());
				Console.commands.Add("growplants", new Console.GrowPlants());
				Console.commands.Add("setlawintensity", new Console.SetLawIntensity());
				Console.commands.Add("setquality", new Console.SetQuality());
				Console.commands.Add("bind", new Console.Bind());
				Console.commands.Add("unbind", new Console.Unbind());
				Console.commands.Add("clearbinds", new Console.ClearBinds());
				Console.commands.Add("hideui", new Console.HideUI());
				Console.commands.Add("disable", new Console.Disable());
				Console.commands.Add("enable", new Console.Enable());
				Console.commands.Add("endtutorial", new Console.EndTutorial());
				Console.commands.Add("disablenpcasset", new Console.DisableNPCAsset());
				Console.commands.Add("showfps", new Console.ShowFPS());
				Console.commands.Add("hidefps", new Console.HideFPS());
				Console.commands.Add("cleartrash", new Console.ClearTrash());
				Console.commands.Add("playcutscene", new Console.PlayCutscene());
			}
			foreach (KeyValuePair<string, Console.ConsoleCommand> keyValuePair in Console.commands)
			{
				Console.Commands.Add(keyValuePair.Value);
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.RunStartupCommands));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.RunStartupCommands));
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000383D0 File Offset: 0x000365D0
		private void RunStartupCommands()
		{
			if (Application.isEditor || Debug.isDebugBuild)
			{
				foreach (string args in this.startupCommands)
				{
					Console.SubmitCommand(args);
				}
			}
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00038430 File Offset: 0x00036630
		[HideInCallstack]
		public static void Log(object message, UnityEngine.Object context = null)
		{
			Debug.Log(message, context);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00038439 File Offset: 0x00036639
		[HideInCallstack]
		public static void LogWarning(object message, UnityEngine.Object context = null)
		{
			Debug.LogWarning(message, context);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00038442 File Offset: 0x00036642
		[HideInCallstack]
		public static void LogError(object message, UnityEngine.Object context = null)
		{
			Debug.LogError(message, context);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0003844C File Offset: 0x0003664C
		public static void SubmitCommand(List<string> args)
		{
			if (args.Count == 0)
			{
				return;
			}
			if (!InstanceFinder.IsHost && !Application.isEditor && !Debug.isDebugBuild)
			{
				return;
			}
			for (int i = 0; i < args.Count; i++)
			{
				args[i] = args[i].ToLower();
			}
			string text = args[0];
			Console.ConsoleCommand consoleCommand;
			if (Console.commands.TryGetValue(text, out consoleCommand))
			{
				args.RemoveAt(0);
				consoleCommand.Execute(args);
				return;
			}
			Console.LogWarning("Command '" + text + "' not found.", null);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000384D8 File Offset: 0x000366D8
		public static void SubmitCommand(string args)
		{
			Console.SubmitCommand(new List<string>(args.Split(new char[]
			{
				' '
			}, StringSplitOptions.RemoveEmptyEntries)));
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000384F8 File Offset: 0x000366F8
		public void AddBinding(KeyCode key, string command)
		{
			Console.Log("Binding " + key.ToString() + " to " + command, null);
			if (this.keyBindings.ContainsKey(key))
			{
				this.keyBindings[key] = command;
				return;
			}
			this.keyBindings.Add(key, command);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00038551 File Offset: 0x00036751
		public void RemoveBinding(KeyCode key)
		{
			Console.Log("Unbinding " + key.ToString(), null);
			this.keyBindings.Remove(key);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0003857D File Offset: 0x0003677D
		public void ClearBindings()
		{
			Console.Log("Clearing all key bindings", null);
			this.keyBindings.Clear();
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00038598 File Offset: 0x00036798
		private void Update()
		{
			if (!GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused)
			{
				foreach (KeyValuePair<KeyCode, string> keyValuePair in this.keyBindings)
				{
					if (Input.GetKeyDown(keyValuePair.Key))
					{
						Console.SubmitCommand(keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x04000D6D RID: 3437
		public Transform TeleportPointsContainer;

		// Token: 0x04000D6E RID: 3438
		public List<Console.LabelledGameObject> LabelledGameObjectList;

		// Token: 0x04000D6F RID: 3439
		[Tooltip("Commands that run on startup (Editor only)")]
		public List<string> startupCommands = new List<string>();

		// Token: 0x04000D70 RID: 3440
		public static List<Console.ConsoleCommand> Commands = new List<Console.ConsoleCommand>();

		// Token: 0x04000D71 RID: 3441
		private static Dictionary<string, Console.ConsoleCommand> commands = new Dictionary<string, Console.ConsoleCommand>();

		// Token: 0x04000D72 RID: 3442
		private Dictionary<KeyCode, string> keyBindings = new Dictionary<KeyCode, string>();

		// Token: 0x02000240 RID: 576
		public abstract class ConsoleCommand
		{
			// Token: 0x17000266 RID: 614
			// (get) Token: 0x06000C2F RID: 3119
			public abstract string CommandWord { get; }

			// Token: 0x17000267 RID: 615
			// (get) Token: 0x06000C30 RID: 3120
			public abstract string CommandDescription { get; }

			// Token: 0x17000268 RID: 616
			// (get) Token: 0x06000C31 RID: 3121
			public abstract string ExampleUsage { get; }

			// Token: 0x06000C32 RID: 3122
			public abstract void Execute(List<string> args);
		}

		// Token: 0x02000241 RID: 577
		public class SetTimeCommand : Console.ConsoleCommand
		{
			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00038648 File Offset: 0x00036848
			public override string CommandWord
			{
				get
				{
					return "settime";
				}
			}

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0003864F File Offset: 0x0003684F
			public override string CommandDescription
			{
				get
				{
					return "Sets the time of day to the specified 24-hour time";
				}
			}

			// Token: 0x1700026B RID: 619
			// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00038656 File Offset: 0x00036856
			public override string ExampleUsage
			{
				get
				{
					return "settime 1530";
				}
			}

			// Token: 0x06000C37 RID: 3127 RVA: 0x00038660 File Offset: 0x00036860
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0 || !TimeManager.IsValid24HourTime(args[0]))
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'settime 1530'", null);
					return;
				}
				if (Player.Local.IsSleeping)
				{
					Console.LogWarning("Can't set time whilst sleeping", null);
					return;
				}
				Console.Log("Time set to " + args[0], null);
				NetworkSingleton<TimeManager>.Instance.SetTime(int.Parse(args[0]), false);
			}
		}

		// Token: 0x02000242 RID: 578
		public class SpawnVehicleCommand : Console.ConsoleCommand
		{
			// Token: 0x1700026C RID: 620
			// (get) Token: 0x06000C39 RID: 3129 RVA: 0x000386DE File Offset: 0x000368DE
			public override string CommandWord
			{
				get
				{
					return "spawnvehicle";
				}
			}

			// Token: 0x1700026D RID: 621
			// (get) Token: 0x06000C3A RID: 3130 RVA: 0x000386E5 File Offset: 0x000368E5
			public override string CommandDescription
			{
				get
				{
					return "Spawns a vehicle at the player's location";
				}
			}

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06000C3B RID: 3131 RVA: 0x000386EC File Offset: 0x000368EC
			public override string ExampleUsage
			{
				get
				{
					return "spawnvehicle shitbox";
				}
			}

			// Token: 0x06000C3C RID: 3132 RVA: 0x000386F4 File Offset: 0x000368F4
			public override void Execute(List<string> args)
			{
				bool flag = false;
				if (args.Count > 0 && NetworkSingleton<VehicleManager>.Instance.GetVehiclePrefab(args[0]) != null)
				{
					flag = true;
					Console.Log("Spawning '" + args[0] + "'...", null);
					Vector3 position = Console.player.transform.position + Console.player.transform.forward * 4f + Console.player.transform.up * 1f;
					Quaternion rotation = Console.player.transform.rotation;
					NetworkSingleton<VehicleManager>.Instance.SpawnAndReturnVehicle(args[0], position, rotation, true);
				}
				if (!flag)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'spawnvehicle shitbox'", null);
				}
			}
		}

		// Token: 0x02000243 RID: 579
		public class AddItemToInventoryCommand : Console.ConsoleCommand
		{
			// Token: 0x1700026F RID: 623
			// (get) Token: 0x06000C3E RID: 3134 RVA: 0x000387C8 File Offset: 0x000369C8
			public override string CommandWord
			{
				get
				{
					return "give";
				}
			}

			// Token: 0x17000270 RID: 624
			// (get) Token: 0x06000C3F RID: 3135 RVA: 0x000387CF File Offset: 0x000369CF
			public override string CommandDescription
			{
				get
				{
					return "Gives the player the specified item. Optionally specify a quantity.";
				}
			}

			// Token: 0x17000271 RID: 625
			// (get) Token: 0x06000C40 RID: 3136 RVA: 0x000387D6 File Offset: 0x000369D6
			public override string ExampleUsage
			{
				get
				{
					return "give ogkush 5";
				}
			}

			// Token: 0x06000C41 RID: 3137 RVA: 0x000387E0 File Offset: 0x000369E0
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'give watering_can', 'give watering_can 5'", null);
					return;
				}
				ItemDefinition item = Registry.GetItem(args[0]);
				if (!(item != null))
				{
					Console.LogWarning("Unrecognized item code '" + args[0] + "'", null);
					return;
				}
				ItemInstance defaultInstance = item.GetDefaultInstance(1);
				if (args[0] == "cash")
				{
					Console.LogWarning("Unrecognized item code '" + args[0] + "'", null);
					return;
				}
				if (PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(defaultInstance, 1))
				{
					int num = 1;
					if (args.Count > 1)
					{
						bool flag = false;
						if (int.TryParse(args[1], out num) && num > 0)
						{
							flag = true;
						}
						if (!flag)
						{
							Console.LogWarning("Unrecognized quantity '" + args[1] + "'. Please provide a positive integer", null);
						}
					}
					int num2 = 0;
					while (num > 0 && PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(defaultInstance, 1))
					{
						PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(defaultInstance);
						num--;
						num2++;
					}
					Console.Log(string.Concat(new string[]
					{
						"Added ",
						num2.ToString(),
						" ",
						item.Name,
						" to inventory"
					}), null);
					return;
				}
				Console.LogWarning("Insufficient inventory space", null);
			}
		}

		// Token: 0x02000244 RID: 580
		public class ClearInventoryCommand : Console.ConsoleCommand
		{
			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0003893A File Offset: 0x00036B3A
			public override string CommandWord
			{
				get
				{
					return "clearinventory";
				}
			}

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00038941 File Offset: 0x00036B41
			public override string CommandDescription
			{
				get
				{
					return "Clears the player's inventory";
				}
			}

			// Token: 0x17000274 RID: 628
			// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0003893A File Offset: 0x00036B3A
			public override string ExampleUsage
			{
				get
				{
					return "clearinventory";
				}
			}

			// Token: 0x06000C46 RID: 3142 RVA: 0x00038948 File Offset: 0x00036B48
			public override void Execute(List<string> args)
			{
				Console.Log("Clearing player inventory...", null);
				PlayerSingleton<PlayerInventory>.Instance.ClearInventory();
			}
		}

		// Token: 0x02000245 RID: 581
		public class ChangeCashCommand : Console.ConsoleCommand
		{
			// Token: 0x17000275 RID: 629
			// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0003895F File Offset: 0x00036B5F
			public override string CommandWord
			{
				get
				{
					return "changecash";
				}
			}

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00038966 File Offset: 0x00036B66
			public override string CommandDescription
			{
				get
				{
					return "Changes the player's cash balance by the specified amount";
				}
			}

			// Token: 0x17000277 RID: 631
			// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0003896D File Offset: 0x00036B6D
			public override string ExampleUsage
			{
				get
				{
					return "changecash 5000";
				}
			}

			// Token: 0x06000C4B RID: 3147 RVA: 0x00038974 File Offset: 0x00036B74
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num))
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'changecash 5000', 'changecash -5000'", null);
					return;
				}
				if (num > 0f)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(num, true, false);
					Console.Log("Gave player " + MoneyManager.FormatAmount(num, false, false) + " cash", null);
					return;
				}
				if (num < 0f)
				{
					num = Mathf.Clamp(num, -NetworkSingleton<MoneyManager>.Instance.cashBalance, 0f);
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(num, true, false);
					Console.Log("Removed " + MoneyManager.FormatAmount(num, false, false) + " cash from player", null);
				}
			}
		}

		// Token: 0x02000246 RID: 582
		public class ChangeOnlineBalanceCommand : Console.ConsoleCommand
		{
			// Token: 0x17000278 RID: 632
			// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00038A27 File Offset: 0x00036C27
			public override string CommandWord
			{
				get
				{
					return "changebalance";
				}
			}

			// Token: 0x17000279 RID: 633
			// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00038A2E File Offset: 0x00036C2E
			public override string CommandDescription
			{
				get
				{
					return "Changes the player's online balance by the specified amount";
				}
			}

			// Token: 0x1700027A RID: 634
			// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00038A35 File Offset: 0x00036C35
			public override string ExampleUsage
			{
				get
				{
					return "changebalance 5000";
				}
			}

			// Token: 0x06000C50 RID: 3152 RVA: 0x00038A3C File Offset: 0x00036C3C
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num))
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'changebalance 5000', 'changebalance -5000'", null);
					return;
				}
				if (num > 0f)
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Added online balance", num, 1f, "Added by developer console");
					Console.Log("Increased online balance by " + MoneyManager.FormatAmount(num, false, false), null);
					return;
				}
				if (num < 0f)
				{
					num = Mathf.Clamp(num, -NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance, 0f);
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Removed online balance", num, 1f, "Removed by developer console");
					Console.Log("Decreased online balance by " + MoneyManager.FormatAmount(num, false, false), null);
				}
			}
		}

		// Token: 0x02000247 RID: 583
		public class SetMoveSpeedCommand : Console.ConsoleCommand
		{
			// Token: 0x1700027B RID: 635
			// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00038AFF File Offset: 0x00036CFF
			public override string CommandWord
			{
				get
				{
					return "setmovespeed";
				}
			}

			// Token: 0x1700027C RID: 636
			// (get) Token: 0x06000C53 RID: 3155 RVA: 0x00038B06 File Offset: 0x00036D06
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's move speed multiplier";
				}
			}

			// Token: 0x1700027D RID: 637
			// (get) Token: 0x06000C54 RID: 3156 RVA: 0x00038B0D File Offset: 0x00036D0D
			public override string ExampleUsage
			{
				get
				{
					return "setmovespeed 1";
				}
			}

			// Token: 0x06000C55 RID: 3157 RVA: 0x00038B14 File Offset: 0x00036D14
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setmovespeed 1'", null);
					return;
				}
				Console.Log("Setting player move speed multiplier to " + num.ToString(), null);
				PlayerMovement.StaticMoveSpeedMultiplier = num;
			}
		}

		// Token: 0x02000248 RID: 584
		public class SetJumpMultiplier : Console.ConsoleCommand
		{
			// Token: 0x1700027E RID: 638
			// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00038B70 File Offset: 0x00036D70
			public override string CommandWord
			{
				get
				{
					return "setjumpforce";
				}
			}

			// Token: 0x1700027F RID: 639
			// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00038B77 File Offset: 0x00036D77
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's jump force multiplier";
				}
			}

			// Token: 0x17000280 RID: 640
			// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00038B7E File Offset: 0x00036D7E
			public override string ExampleUsage
			{
				get
				{
					return "setjumpforce 1";
				}
			}

			// Token: 0x06000C5A RID: 3162 RVA: 0x00038B88 File Offset: 0x00036D88
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setjumpforce 1'", null);
					return;
				}
				Console.Log("Setting player jump force multiplier to " + num.ToString(), null);
				PlayerMovement.JumpMultiplier = num;
			}
		}

		// Token: 0x02000249 RID: 585
		public class SetPropertyOwned : Console.ConsoleCommand
		{
			// Token: 0x17000281 RID: 641
			// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00038BE4 File Offset: 0x00036DE4
			public override string CommandWord
			{
				get
				{
					return "setowned";
				}
			}

			// Token: 0x17000282 RID: 642
			// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00038BEB File Offset: 0x00036DEB
			public override string CommandDescription
			{
				get
				{
					return "Sets the specified property or business as owned";
				}
			}

			// Token: 0x17000283 RID: 643
			// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00038BF2 File Offset: 0x00036DF2
			public override string ExampleUsage
			{
				get
				{
					return "setowned barn, setowned laundromat";
				}
			}

			// Token: 0x06000C5F RID: 3167 RVA: 0x00038BFC File Offset: 0x00036DFC
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"setowned barn",
						"setowned manor"
					});
					return;
				}
				string code = args[0].ToLower();
				Property property = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == code);
				Business business = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == code);
				if (property == null && business == null)
				{
					Console.LogCommandError("Could not find unowned property with code '" + code + "'");
					return;
				}
				if (property != null)
				{
					property.SetOwned();
				}
				if (business != null)
				{
					business.SetOwned();
				}
				Console.Log("Property with code '" + code + "' is now owned", null);
			}
		}

		// Token: 0x0200024B RID: 587
		public class Teleport : Console.ConsoleCommand
		{
			// Token: 0x17000284 RID: 644
			// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00038CF5 File Offset: 0x00036EF5
			public override string CommandWord
			{
				get
				{
					return "teleport";
				}
			}

			// Token: 0x17000285 RID: 645
			// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00038CFC File Offset: 0x00036EFC
			public override string CommandDescription
			{
				get
				{
					return "Teleports the player to the specified location";
				}
			}

			// Token: 0x17000286 RID: 646
			// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00038D03 File Offset: 0x00036F03
			public override string ExampleUsage
			{
				get
				{
					return "teleport townhall, teleport barn";
				}
			}

			// Token: 0x06000C67 RID: 3175 RVA: 0x00038D0C File Offset: 0x00036F0C
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"teleport docks",
						"teleport barn"
					});
					return;
				}
				string text = args[0].ToLower();
				Transform transform = null;
				Vector3 b = Vector3.zero;
				for (int i = 0; i < Singleton<Console>.Instance.TeleportPointsContainer.childCount; i++)
				{
					if (Singleton<Console>.Instance.TeleportPointsContainer.GetChild(i).name.ToLower() == text)
					{
						transform = Singleton<Console>.Instance.TeleportPointsContainer.GetChild(i);
						break;
					}
				}
				if (transform == null)
				{
					for (int j = 0; j < Property.Properties.Count; j++)
					{
						if (Property.Properties[j].PropertyCode.ToLower() == text)
						{
							transform = Property.Properties[j].SpawnPoint;
							b = Vector3.up * 1f;
							break;
						}
					}
				}
				if (transform == null)
				{
					for (int k = 0; k < Business.Businesses.Count; k++)
					{
						if (Business.Businesses[k].PropertyCode.ToLower() == text)
						{
							transform = Business.Businesses[k].SpawnPoint;
							b = Vector3.up * 1f;
							break;
						}
					}
				}
				if (transform == null)
				{
					Console.LogCommandError("Unrecognized destination");
					return;
				}
				PlayerSingleton<PlayerMovement>.Instance.Teleport(transform.transform.position + b);
				Player.Local.transform.forward = transform.transform.forward;
				Console.Log("Teleported to '" + text + "'", null);
			}
		}

		// Token: 0x0200024C RID: 588
		public class PackageProduct : Console.ConsoleCommand
		{
			// Token: 0x17000287 RID: 647
			// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00038ECC File Offset: 0x000370CC
			public override string CommandWord
			{
				get
				{
					return "packageprodcut";
				}
			}

			// Token: 0x17000288 RID: 648
			// (get) Token: 0x06000C6A RID: 3178 RVA: 0x00038ED3 File Offset: 0x000370D3
			public override string CommandDescription
			{
				get
				{
					return "Packages the equipped product with the specified packaging";
				}
			}

			// Token: 0x17000289 RID: 649
			// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00038EDA File Offset: 0x000370DA
			public override string ExampleUsage
			{
				get
				{
					return "packageproduct jar, packageproduct baggie";
				}
			}

			// Token: 0x06000C6C RID: 3180 RVA: 0x00038EE4 File Offset: 0x000370E4
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"packageproduct jar",
						"packageproduct baggie"
					});
					return;
				}
				PackagingDefinition packagingDefinition = Registry.GetItem(args[0].ToLower()) as PackagingDefinition;
				if (packagingDefinition == null)
				{
					Console.LogCommandError("Unrecognized packaging ID");
					return;
				}
				if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance is ProductItemInstance)
				{
					(PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance as ProductItemInstance).SetPackaging(packagingDefinition);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
					Console.Log("Applied packaging '" + packagingDefinition.Name + "' to equipped product", null);
					return;
				}
				Console.LogCommandError("No product equipped");
			}
		}

		// Token: 0x0200024D RID: 589
		public class SetStaminaReserve : Console.ConsoleCommand
		{
			// Token: 0x1700028A RID: 650
			// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00038FBC File Offset: 0x000371BC
			public override string CommandWord
			{
				get
				{
					return "setstaminareserve";
				}
			}

			// Token: 0x1700028B RID: 651
			// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00038FC3 File Offset: 0x000371C3
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's stamina reserve (default 100) to the specified amount.";
				}
			}

			// Token: 0x1700028C RID: 652
			// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00038FCA File Offset: 0x000371CA
			public override string ExampleUsage
			{
				get
				{
					return "setstaminareserve 200";
				}
			}

			// Token: 0x06000C71 RID: 3185 RVA: 0x00038FD4 File Offset: 0x000371D4
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setstaminareserve 200'", null);
					return;
				}
				Console.Log("Setting player stamina reserve to " + num.ToString(), null);
				PlayerMovement.StaminaReserveMax = num;
				PlayerSingleton<PlayerMovement>.Instance.SetStamina(num, true);
			}
		}

		// Token: 0x0200024E RID: 590
		public class RaisedWanted : Console.ConsoleCommand
		{
			// Token: 0x1700028D RID: 653
			// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0003903C File Offset: 0x0003723C
			public override string CommandWord
			{
				get
				{
					return "raisewanted";
				}
			}

			// Token: 0x1700028E RID: 654
			// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00039043 File Offset: 0x00037243
			public override string CommandDescription
			{
				get
				{
					return "Raises the player's wanted level";
				}
			}

			// Token: 0x1700028F RID: 655
			// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0003903C File Offset: 0x0003723C
			public override string ExampleUsage
			{
				get
				{
					return "raisewanted";
				}
			}

			// Token: 0x06000C76 RID: 3190 RVA: 0x0003904C File Offset: 0x0003724C
			public override void Execute(List<string> args)
			{
				Console.Log("Raising wanted level...", null);
				if (Console.player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
				{
					Singleton<LawManager>.Instance.PoliceCalled(Console.player, new Crime());
				}
				Console.player.CrimeData.Escalate();
			}
		}

		// Token: 0x0200024F RID: 591
		public class LowerWanted : Console.ConsoleCommand
		{
			// Token: 0x17000290 RID: 656
			// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00039098 File Offset: 0x00037298
			public override string CommandWord
			{
				get
				{
					return "lowerwanted";
				}
			}

			// Token: 0x17000291 RID: 657
			// (get) Token: 0x06000C79 RID: 3193 RVA: 0x0003909F File Offset: 0x0003729F
			public override string CommandDescription
			{
				get
				{
					return "Lowers the player's wanted level";
				}
			}

			// Token: 0x17000292 RID: 658
			// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00039098 File Offset: 0x00037298
			public override string ExampleUsage
			{
				get
				{
					return "lowerwanted";
				}
			}

			// Token: 0x06000C7B RID: 3195 RVA: 0x000390A6 File Offset: 0x000372A6
			public override void Execute(List<string> args)
			{
				Console.Log("Lowering wanted level...", null);
				Console.player.CrimeData.Deescalate();
			}
		}

		// Token: 0x02000250 RID: 592
		public class ClearWanted : Console.ConsoleCommand
		{
			// Token: 0x17000293 RID: 659
			// (get) Token: 0x06000C7D RID: 3197 RVA: 0x000390C2 File Offset: 0x000372C2
			public override string CommandWord
			{
				get
				{
					return "clearwanted";
				}
			}

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000390C9 File Offset: 0x000372C9
			public override string CommandDescription
			{
				get
				{
					return "Clears the player's wanted level";
				}
			}

			// Token: 0x17000295 RID: 661
			// (get) Token: 0x06000C7F RID: 3199 RVA: 0x000390C2 File Offset: 0x000372C2
			public override string ExampleUsage
			{
				get
				{
					return "clearwanted";
				}
			}

			// Token: 0x06000C80 RID: 3200 RVA: 0x000390D0 File Offset: 0x000372D0
			public override void Execute(List<string> args)
			{
				Console.Log("Clearing wanted level...", null);
				Console.player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
				Console.player.CrimeData.ClearCrimes();
			}
		}

		// Token: 0x02000251 RID: 593
		public class SetHealth : Console.ConsoleCommand
		{
			// Token: 0x17000296 RID: 662
			// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000390FC File Offset: 0x000372FC
			public override string CommandWord
			{
				get
				{
					return "sethealth";
				}
			}

			// Token: 0x17000297 RID: 663
			// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00039103 File Offset: 0x00037303
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's health to the specified amount";
				}
			}

			// Token: 0x17000298 RID: 664
			// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0003910A File Offset: 0x0003730A
			public override string ExampleUsage
			{
				get
				{
					return "sethealth 100";
				}
			}

			// Token: 0x06000C85 RID: 3205 RVA: 0x00039114 File Offset: 0x00037314
			public override void Execute(List<string> args)
			{
				if (!Console.player.Health.IsAlive)
				{
					Console.LogWarning("Can't set health whilst dead", null);
					return;
				}
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'sethealth 100'", null);
					return;
				}
				Console.Log("Setting player health to " + num.ToString(), null);
				Console.player.Health.SetHealth(num);
				if (num < 0f)
				{
					PlayerSingleton<PlayerCamera>.Instance.JoltCamera();
				}
			}
		}

		// Token: 0x02000252 RID: 594
		public class SetEnergy : Console.ConsoleCommand
		{
			// Token: 0x17000299 RID: 665
			// (get) Token: 0x06000C87 RID: 3207 RVA: 0x000391A9 File Offset: 0x000373A9
			public override string CommandWord
			{
				get
				{
					return "setenergy";
				}
			}

			// Token: 0x1700029A RID: 666
			// (get) Token: 0x06000C88 RID: 3208 RVA: 0x000391B0 File Offset: 0x000373B0
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's energy to the specified amount";
				}
			}

			// Token: 0x1700029B RID: 667
			// (get) Token: 0x06000C89 RID: 3209 RVA: 0x000391B7 File Offset: 0x000373B7
			public override string ExampleUsage
			{
				get
				{
					return "setenergy 100";
				}
			}

			// Token: 0x06000C8A RID: 3210 RVA: 0x000391C0 File Offset: 0x000373C0
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setenergy 100'", null);
					return;
				}
				num = Mathf.Clamp(num, 0f, 100f);
				Console.Log("Setting player energy to " + num.ToString(), null);
				Player.Local.Energy.SetEnergy(num);
			}
		}

		// Token: 0x02000253 RID: 595
		public class FreeCamCommand : Console.ConsoleCommand
		{
			// Token: 0x1700029C RID: 668
			// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00039237 File Offset: 0x00037437
			public override string CommandWord
			{
				get
				{
					return "freecam";
				}
			}

			// Token: 0x1700029D RID: 669
			// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0003923E File Offset: 0x0003743E
			public override string CommandDescription
			{
				get
				{
					return "Toggles free cam mode";
				}
			}

			// Token: 0x1700029E RID: 670
			// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00039237 File Offset: 0x00037437
			public override string ExampleUsage
			{
				get
				{
					return "freecam";
				}
			}

			// Token: 0x06000C8F RID: 3215 RVA: 0x00039245 File Offset: 0x00037445
			public override void Execute(List<string> args)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.FreeCamEnabled)
				{
					PlayerSingleton<PlayerCamera>.Instance.SetFreeCam(false, true);
					return;
				}
				PlayerSingleton<PlayerCamera>.Instance.SetFreeCam(true, true);
			}
		}

		// Token: 0x02000254 RID: 596
		public class Save : Console.ConsoleCommand
		{
			// Token: 0x1700029F RID: 671
			// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0003926C File Offset: 0x0003746C
			public override string CommandWord
			{
				get
				{
					return "save";
				}
			}

			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00039273 File Offset: 0x00037473
			public override string CommandDescription
			{
				get
				{
					return "Forces a save";
				}
			}

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0003926C File Offset: 0x0003746C
			public override string ExampleUsage
			{
				get
				{
					return "save";
				}
			}

			// Token: 0x06000C94 RID: 3220 RVA: 0x0003927A File Offset: 0x0003747A
			public override void Execute(List<string> args)
			{
				Console.Log("Forcing save...", null);
				Singleton<SaveManager>.Instance.Save();
			}
		}

		// Token: 0x02000255 RID: 597
		public class SetTimeScale : Console.ConsoleCommand
		{
			// Token: 0x170002A2 RID: 674
			// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00039291 File Offset: 0x00037491
			public override string CommandWord
			{
				get
				{
					return "settimescale";
				}
			}

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x06000C97 RID: 3223 RVA: 0x00039298 File Offset: 0x00037498
			public override string CommandDescription
			{
				get
				{
					return "Sets the time scale. Default 1";
				}
			}

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0003929F File Offset: 0x0003749F
			public override string ExampleUsage
			{
				get
				{
					return "settimescale 1";
				}
			}

			// Token: 0x06000C99 RID: 3225 RVA: 0x000392A8 File Offset: 0x000374A8
			public override void Execute(List<string> args)
			{
				if (!Singleton<Settings>.Instance.PausingFreezesTime)
				{
					Console.LogWarning("Can't set time scale right now.", null);
					return;
				}
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'settimescale 1'", null);
					return;
				}
				num = Mathf.Clamp(num, 0f, 20f);
				Console.Log("Setting time scale to " + num.ToString(), null);
				Time.timeScale = num;
			}
		}

		// Token: 0x02000256 RID: 598
		public class SetVariableValue : Console.ConsoleCommand
		{
			// Token: 0x170002A5 RID: 677
			// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0003932D File Offset: 0x0003752D
			public override string CommandWord
			{
				get
				{
					return "setvar";
				}
			}

			// Token: 0x170002A6 RID: 678
			// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00039334 File Offset: 0x00037534
			public override string CommandDescription
			{
				get
				{
					return "Sets the value of the specified variable";
				}
			}

			// Token: 0x170002A7 RID: 679
			// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0003933B File Offset: 0x0003753B
			public override string ExampleUsage
			{
				get
				{
					return "setvar <variable> <value>";
				}
			}

			// Token: 0x06000C9E RID: 3230 RVA: 0x00039344 File Offset: 0x00037544
			public override void Execute(List<string> args)
			{
				if (args.Count >= 2)
				{
					string variableName = args[0].ToLower();
					string value = args[1];
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(variableName, value, true);
					return;
				}
				Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
			}
		}

		// Token: 0x02000257 RID: 599
		public class SetQuestState : Console.ConsoleCommand
		{
			// Token: 0x170002A8 RID: 680
			// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00039393 File Offset: 0x00037593
			public override string CommandWord
			{
				get
				{
					return "setqueststate";
				}
			}

			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x0003939A File Offset: 0x0003759A
			public override string CommandDescription
			{
				get
				{
					return "Sets the state of the specified quest";
				}
			}

			// Token: 0x170002AA RID: 682
			// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x000393A1 File Offset: 0x000375A1
			public override string ExampleUsage
			{
				get
				{
					return "setqueststate <quest name> <state>";
				}
			}

			// Token: 0x06000CA3 RID: 3235 RVA: 0x000393A8 File Offset: 0x000375A8
			public override void Execute(List<string> args)
			{
				if (args.Count < 2)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				string text2 = args[1];
				text = text.Replace("_", " ");
				Quest quest = Quest.GetQuest(text);
				if (quest == null)
				{
					Console.LogWarning("Failed to find quest with name '" + text + "'", null);
					return;
				}
				EQuestState state = EQuestState.Inactive;
				if (!Enum.TryParse<EQuestState>(text2, true, out state))
				{
					Console.LogWarning("Failed to parse quest state '" + text2 + "'", null);
					return;
				}
				quest.SetQuestState(state, true);
			}
		}

		// Token: 0x02000258 RID: 600
		public class SetQuestEntryState : Console.ConsoleCommand
		{
			// Token: 0x170002AB RID: 683
			// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0003944E File Offset: 0x0003764E
			public override string CommandWord
			{
				get
				{
					return "setquestentrystate";
				}
			}

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00039455 File Offset: 0x00037655
			public override string CommandDescription
			{
				get
				{
					return "Sets the state of the specified quest entry";
				}
			}

			// Token: 0x170002AD RID: 685
			// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0003945C File Offset: 0x0003765C
			public override string ExampleUsage
			{
				get
				{
					return "setquestentrystate <quest name> <entry index> <state>";
				}
			}

			// Token: 0x06000CA8 RID: 3240 RVA: 0x00039464 File Offset: 0x00037664
			public override void Execute(List<string> args)
			{
				if (args.Count < 3)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				int num = int.TryParse(args[1], out num) ? num : -1;
				string text2 = args[2];
				text = text.Replace("_", " ");
				Quest quest = Quest.GetQuest(text);
				if (quest == null)
				{
					Console.LogWarning("Failed to find quest with name '" + text + "'", null);
					return;
				}
				if (num < 0 || num >= quest.Entries.Count)
				{
					Console.LogWarning("Invalid entry index", null);
					return;
				}
				EQuestState state = EQuestState.Inactive;
				if (!Enum.TryParse<EQuestState>(text2, true, out state))
				{
					Console.LogWarning("Failed to parse quest state '" + text2 + "'", null);
					return;
				}
				quest.SetQuestEntryState(num, state, true);
			}
		}

		// Token: 0x02000259 RID: 601
		public class SetEmotion : Console.ConsoleCommand
		{
			// Token: 0x170002AE RID: 686
			// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00039543 File Offset: 0x00037743
			public override string CommandWord
			{
				get
				{
					return "setemotion";
				}
			}

			// Token: 0x170002AF RID: 687
			// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0003954A File Offset: 0x0003774A
			public override string CommandDescription
			{
				get
				{
					return "Sets the facial expression of the player's avatar.";
				}
			}

			// Token: 0x170002B0 RID: 688
			// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00039551 File Offset: 0x00037751
			public override string ExampleUsage
			{
				get
				{
					return "setemotion cheery";
				}
			}

			// Token: 0x06000CAD RID: 3245 RVA: 0x00039558 File Offset: 0x00037758
			public override void Execute(List<string> args)
			{
				if (!Singleton<Settings>.Instance.PausingFreezesTime)
				{
					Console.LogWarning("Can't set time scale right now.", null);
					return;
				}
				if (args.Count == 0)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				if (!Player.Local.Avatar.EmotionManager.HasEmotion(text))
				{
					Console.LogWarning("Unrecognized emotion '" + text + "'", null);
					return;
				}
				Console.Log("Setting emotion to " + text, null);
				Player.Local.Avatar.EmotionManager.AddEmotionOverride(text, "console", 0f, 0);
			}
		}

		// Token: 0x0200025A RID: 602
		public class SetUnlocked : Console.ConsoleCommand
		{
			// Token: 0x170002B1 RID: 689
			// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00039608 File Offset: 0x00037808
			public override string CommandWord
			{
				get
				{
					return "setunlocked";
				}
			}

			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0003960F File Offset: 0x0003780F
			public override string CommandDescription
			{
				get
				{
					return "Unlocks the given NPC";
				}
			}

			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00039616 File Offset: 0x00037816
			public override string ExampleUsage
			{
				get
				{
					return "setunlocked <npc_id>";
				}
			}

			// Token: 0x06000CB2 RID: 3250 RVA: 0x00039620 File Offset: 0x00037820
			public override void Execute(List<string> args)
			{
				if (args.Count < 1)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				NPC npc = NPCManager.GetNPC(text);
				if (npc == null)
				{
					Console.LogWarning("Failed to find NPC with ID '" + text + "'", null);
					return;
				}
				npc.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, true);
			}
		}

		// Token: 0x0200025B RID: 603
		public class SetRelationship : Console.ConsoleCommand
		{
			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0003968E File Offset: 0x0003788E
			public override string CommandWord
			{
				get
				{
					return "setrelationship";
				}
			}

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00039695 File Offset: 0x00037895
			public override string CommandDescription
			{
				get
				{
					return "Sets the relationship scalar of the given NPC. Range is 0-5.";
				}
			}

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0003969C File Offset: 0x0003789C
			public override string ExampleUsage
			{
				get
				{
					return "setrelationship <npc_id> 5";
				}
			}

			// Token: 0x06000CB7 RID: 3255 RVA: 0x000396A4 File Offset: 0x000378A4
			public override void Execute(List<string> args)
			{
				if (args.Count < 2)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				NPC npc = NPCManager.GetNPC(text);
				if (npc == null)
				{
					Console.LogWarning("Failed to find NPC with ID '" + text + "'", null);
					return;
				}
				float num = 0f;
				if (!float.TryParse(args[1], out num) || num < 0f || num > 5f)
				{
					Console.LogWarning("Invalid scalar value. Must be between 0 and 5.", null);
					return;
				}
				npc.RelationData.SetRelationship(num);
			}
		}

		// Token: 0x0200025C RID: 604
		public class AddEmployeeCommand : Console.ConsoleCommand
		{
			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00039743 File Offset: 0x00037943
			public override string CommandWord
			{
				get
				{
					return "addemployee";
				}
			}

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0003974A File Offset: 0x0003794A
			public override string CommandDescription
			{
				get
				{
					return "Adds an employee of the specified type to the given property.";
				}
			}

			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00039751 File Offset: 0x00037951
			public override string ExampleUsage
			{
				get
				{
					return "addemployee botanist barn";
				}
			}

			// Token: 0x06000CBC RID: 3260 RVA: 0x00039758 File Offset: 0x00037958
			public override void Execute(List<string> args)
			{
				if (args.Count < 2)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"setowned barn",
						"setowned manor"
					});
					return;
				}
				args[0].ToLower();
				EEmployeeType type = EEmployeeType.Botanist;
				if (!Enum.TryParse<EEmployeeType>(args[0], true, out type))
				{
					Console.LogCommandError("Unrecognized employee type '" + args[0] + "'");
					return;
				}
				string code = args[1].ToLower();
				Property property = Property.OwnedProperties.Find((Property x) => x.PropertyCode.ToLower() == code);
				if (property == null)
				{
					Console.LogCommandError("Could not find property with code '" + code + "'");
					return;
				}
				NetworkSingleton<EmployeeManager>.Instance.CreateNewEmployee(property, type);
				Console.Log(string.Concat(new string[]
				{
					"Adding employee of type '",
					type.ToString(),
					"' to property '",
					property.PropertyCode,
					"'"
				}), null);
			}
		}

		// Token: 0x0200025E RID: 606
		public class SetDiscovered : Console.ConsoleCommand
		{
			// Token: 0x170002BA RID: 698
			// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00039882 File Offset: 0x00037A82
			public override string CommandWord
			{
				get
				{
					return "setdiscovered";
				}
			}

			// Token: 0x170002BB RID: 699
			// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00039889 File Offset: 0x00037A89
			public override string CommandDescription
			{
				get
				{
					return "Sets the specified product as discovered";
				}
			}

			// Token: 0x170002BC RID: 700
			// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00039890 File Offset: 0x00037A90
			public override string ExampleUsage
			{
				get
				{
					return "setdiscovered ogkush";
				}
			}

			// Token: 0x06000CC3 RID: 3267 RVA: 0x00039898 File Offset: 0x00037A98
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string text = args[0].ToLower();
				ProductDefinition productDefinition = Registry.GetItem(text) as ProductDefinition;
				if (productDefinition == null)
				{
					Console.LogCommandError("Unrecognized product code '" + text + "'");
					return;
				}
				NetworkSingleton<ProductManager>.Instance.DiscoverProduct(productDefinition.ID);
				Console.Log(productDefinition.Name + " now discovered", null);
			}
		}

		// Token: 0x0200025F RID: 607
		public class GrowPlants : Console.ConsoleCommand
		{
			// Token: 0x170002BD RID: 701
			// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00039921 File Offset: 0x00037B21
			public override string CommandWord
			{
				get
				{
					return "growplants";
				}
			}

			// Token: 0x170002BE RID: 702
			// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00039928 File Offset: 0x00037B28
			public override string CommandDescription
			{
				get
				{
					return "Sets ALL plants in the world fully grown";
				}
			}

			// Token: 0x170002BF RID: 703
			// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00039921 File Offset: 0x00037B21
			public override string ExampleUsage
			{
				get
				{
					return "growplants";
				}
			}

			// Token: 0x06000CC8 RID: 3272 RVA: 0x00039930 File Offset: 0x00037B30
			public override void Execute(List<string> args)
			{
				Plant[] array = UnityEngine.Object.FindObjectsOfType<Plant>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Pot.FullyGrowPlant();
				}
			}
		}

		// Token: 0x02000260 RID: 608
		public class SetLawIntensity : Console.ConsoleCommand
		{
			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0003995E File Offset: 0x00037B5E
			public override string CommandWord
			{
				get
				{
					return "setlawintensity";
				}
			}

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00039965 File Offset: 0x00037B65
			public override string CommandDescription
			{
				get
				{
					return "Sets the intensity of law enforcement activity on a scale of 0-10.";
				}
			}

			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0003996C File Offset: 0x00037B6C
			public override string ExampleUsage
			{
				get
				{
					return "setlawintensity 6";
				}
			}

			// Token: 0x06000CCD RID: 3277 RVA: 0x00039974 File Offset: 0x00037B74
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): " + this.ExampleUsage, null);
					return;
				}
				float num2 = Mathf.Clamp(num, 0f, 10f);
				Console.Log("Setting law enforcement intensity to " + num2.ToString(), null);
				Singleton<LawController>.Instance.SetInternalIntensity(num2 / 10f);
			}
		}

		// Token: 0x02000261 RID: 609
		public class SetQuality : Console.ConsoleCommand
		{
			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x06000CCF RID: 3279 RVA: 0x000399F7 File Offset: 0x00037BF7
			public override string CommandWord
			{
				get
				{
					return "setquality";
				}
			}

			// Token: 0x170002C4 RID: 708
			// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x000399FE File Offset: 0x00037BFE
			public override string CommandDescription
			{
				get
				{
					return "Sets the quality of the currently equipped item.";
				}
			}

			// Token: 0x170002C5 RID: 709
			// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00039A05 File Offset: 0x00037C05
			public override string ExampleUsage
			{
				get
				{
					return "setquality standard, setquality heavenly";
				}
			}

			// Token: 0x06000CD2 RID: 3282 RVA: 0x00039A0C File Offset: 0x00037C0C
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string text = args[0].ToLower();
				EQuality quality;
				if (!Enum.TryParse<EQuality>(text, true, out quality))
				{
					Console.LogCommandError("Unrecognized quality '" + text + "'");
				}
				if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance is QualityItemInstance)
				{
					(PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance as QualityItemInstance).SetQuality(quality);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
					Console.Log("Set quality to " + quality.ToString(), null);
					return;
				}
				Console.LogCommandError("No quality item equipped");
			}
		}

		// Token: 0x02000262 RID: 610
		public class Bind : Console.ConsoleCommand
		{
			// Token: 0x170002C6 RID: 710
			// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x00039AE1 File Offset: 0x00037CE1
			public override string CommandWord
			{
				get
				{
					return "bind";
				}
			}

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00039AE8 File Offset: 0x00037CE8
			public override string CommandDescription
			{
				get
				{
					return "Binds the given key to the given command.";
				}
			}

			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00039AEF File Offset: 0x00037CEF
			public override string ExampleUsage
			{
				get
				{
					return "bind t 'settime 1200'";
				}
			}

			// Token: 0x06000CD7 RID: 3287 RVA: 0x00039AF8 File Offset: 0x00037CF8
			public override void Execute(List<string> args)
			{
				if (args.Count > 1)
				{
					string text = args[0].ToLower();
					KeyCode key;
					if (!Enum.TryParse<KeyCode>(text, true, out key))
					{
						Console.LogCommandError("Unrecognized keycode '" + text + "'");
					}
					string command = string.Join(" ", args.ToArray()).Substring(text.Length + 1);
					Singleton<Console>.Instance.AddBinding(key, command);
					return;
				}
				Console.LogUnrecognizedFormat(new string[]
				{
					this.ExampleUsage
				});
			}
		}

		// Token: 0x02000263 RID: 611
		public class Unbind : Console.ConsoleCommand
		{
			// Token: 0x170002C9 RID: 713
			// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00039B7A File Offset: 0x00037D7A
			public override string CommandWord
			{
				get
				{
					return "unbind";
				}
			}

			// Token: 0x170002CA RID: 714
			// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00039B81 File Offset: 0x00037D81
			public override string CommandDescription
			{
				get
				{
					return "Removes the given bind.";
				}
			}

			// Token: 0x170002CB RID: 715
			// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00039B88 File Offset: 0x00037D88
			public override string ExampleUsage
			{
				get
				{
					return "unbind t";
				}
			}

			// Token: 0x06000CDC RID: 3292 RVA: 0x00039B90 File Offset: 0x00037D90
			public override void Execute(List<string> args)
			{
				if (args.Count > 0)
				{
					string text = args[0].ToLower();
					KeyCode key;
					if (!Enum.TryParse<KeyCode>(text, true, out key))
					{
						Console.LogCommandError("Unrecognized keycode '" + text + "'");
					}
					Singleton<Console>.Instance.RemoveBinding(key);
					return;
				}
				Console.LogUnrecognizedFormat(new string[]
				{
					this.ExampleUsage
				});
			}
		}

		// Token: 0x02000264 RID: 612
		public class ClearBinds : Console.ConsoleCommand
		{
			// Token: 0x170002CC RID: 716
			// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00039BF3 File Offset: 0x00037DF3
			public override string CommandWord
			{
				get
				{
					return "clearbinds";
				}
			}

			// Token: 0x170002CD RID: 717
			// (get) Token: 0x06000CDF RID: 3295 RVA: 0x00039BFA File Offset: 0x00037DFA
			public override string CommandDescription
			{
				get
				{
					return "Clears ALL binds.";
				}
			}

			// Token: 0x170002CE RID: 718
			// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00039BF3 File Offset: 0x00037DF3
			public override string ExampleUsage
			{
				get
				{
					return "clearbinds";
				}
			}

			// Token: 0x06000CE1 RID: 3297 RVA: 0x00039C01 File Offset: 0x00037E01
			public override void Execute(List<string> args)
			{
				Singleton<Console>.Instance.ClearBindings();
			}
		}

		// Token: 0x02000265 RID: 613
		public class HideUI : Console.ConsoleCommand
		{
			// Token: 0x170002CF RID: 719
			// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00039C0D File Offset: 0x00037E0D
			public override string CommandWord
			{
				get
				{
					return "hideui";
				}
			}

			// Token: 0x170002D0 RID: 720
			// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00039C14 File Offset: 0x00037E14
			public override string CommandDescription
			{
				get
				{
					return "Hides all on-screen UI.";
				}
			}

			// Token: 0x170002D1 RID: 721
			// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00039C0D File Offset: 0x00037E0D
			public override string ExampleUsage
			{
				get
				{
					return "hideui";
				}
			}

			// Token: 0x06000CE6 RID: 3302 RVA: 0x00039C1B File Offset: 0x00037E1B
			public override void Execute(List<string> args)
			{
				Singleton<HUD>.Instance.canvas.enabled = false;
			}
		}

		// Token: 0x02000266 RID: 614
		public class GiveXP : Console.ConsoleCommand
		{
			// Token: 0x170002D2 RID: 722
			// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00039C2D File Offset: 0x00037E2D
			public override string CommandWord
			{
				get
				{
					return "addxp";
				}
			}

			// Token: 0x170002D3 RID: 723
			// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00039C34 File Offset: 0x00037E34
			public override string CommandDescription
			{
				get
				{
					return "Adds the specified amount of experience points.";
				}
			}

			// Token: 0x170002D4 RID: 724
			// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00039C3B File Offset: 0x00037E3B
			public override string ExampleUsage
			{
				get
				{
					return "addxp 100";
				}
			}

			// Token: 0x06000CEB RID: 3307 RVA: 0x00039C44 File Offset: 0x00037E44
			public override void Execute(List<string> args)
			{
				int num = 0;
				if (args.Count == 0 || !int.TryParse(args[0], out num) || num < 0)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): " + this.ExampleUsage, null);
					return;
				}
				Console.Log("Giving " + num.ToString() + " experience points", null);
				NetworkSingleton<LevelManager>.Instance.AddXP(num);
			}
		}

		// Token: 0x02000267 RID: 615
		public class Disable : Console.ConsoleCommand
		{
			// Token: 0x170002D5 RID: 725
			// (get) Token: 0x06000CED RID: 3309 RVA: 0x00039CAD File Offset: 0x00037EAD
			public override string CommandWord
			{
				get
				{
					return "disable";
				}
			}

			// Token: 0x170002D6 RID: 726
			// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00039CB4 File Offset: 0x00037EB4
			public override string CommandDescription
			{
				get
				{
					return "Disables the specified GameObject";
				}
			}

			// Token: 0x170002D7 RID: 727
			// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00039CBB File Offset: 0x00037EBB
			public override string ExampleUsage
			{
				get
				{
					return "disable pp";
				}
			}

			// Token: 0x06000CF0 RID: 3312 RVA: 0x00039CC4 File Offset: 0x00037EC4
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string code = args[0].ToLower();
				Console.LabelledGameObject labelledGameObject = Singleton<Console>.Instance.LabelledGameObjectList.Find((Console.LabelledGameObject x) => x.Label.ToLower() == code);
				if (labelledGameObject == null)
				{
					Console.LogCommandError("Could not find GameObject with label '" + code + "'");
					return;
				}
				labelledGameObject.GameObject.SetActive(false);
			}
		}

		// Token: 0x02000269 RID: 617
		public class Enable : Console.ConsoleCommand
		{
			// Token: 0x170002D8 RID: 728
			// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00039D65 File Offset: 0x00037F65
			public override string CommandWord
			{
				get
				{
					return "enable";
				}
			}

			// Token: 0x170002D9 RID: 729
			// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00039D6C File Offset: 0x00037F6C
			public override string CommandDescription
			{
				get
				{
					return "Enables the specified GameObject";
				}
			}

			// Token: 0x170002DA RID: 730
			// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00039D73 File Offset: 0x00037F73
			public override string ExampleUsage
			{
				get
				{
					return "enable pp";
				}
			}

			// Token: 0x06000CF7 RID: 3319 RVA: 0x00039D7C File Offset: 0x00037F7C
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string code = args[0].ToLower();
				Console.LabelledGameObject labelledGameObject = Singleton<Console>.Instance.LabelledGameObjectList.Find((Console.LabelledGameObject x) => x.Label.ToLower() == code);
				if (labelledGameObject == null)
				{
					Console.LogCommandError("Could not find GameObject with label '" + code + "'");
					return;
				}
				labelledGameObject.GameObject.SetActive(true);
			}
		}

		// Token: 0x0200026B RID: 619
		public class EndTutorial : Console.ConsoleCommand
		{
			// Token: 0x170002DB RID: 731
			// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00039E1D File Offset: 0x0003801D
			public override string CommandWord
			{
				get
				{
					return "endtutorial";
				}
			}

			// Token: 0x170002DC RID: 732
			// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00039E24 File Offset: 0x00038024
			public override string CommandDescription
			{
				get
				{
					return "Forces the tutorial to end immediately (only if the player is actually in the tutorial).";
				}
			}

			// Token: 0x170002DD RID: 733
			// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00039E1D File Offset: 0x0003801D
			public override string ExampleUsage
			{
				get
				{
					return "endtutorial";
				}
			}

			// Token: 0x06000CFE RID: 3326 RVA: 0x00039E2B File Offset: 0x0003802B
			public override void Execute(List<string> args)
			{
				NetworkSingleton<GameManager>.Instance.EndTutorial(false);
			}
		}

		// Token: 0x0200026C RID: 620
		public class DisableNPCAsset : Console.ConsoleCommand
		{
			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00039E38 File Offset: 0x00038038
			public override string CommandWord
			{
				get
				{
					return "disablenpcasset";
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00039E3F File Offset: 0x0003803F
			public override string CommandDescription
			{
				get
				{
					return "Disabled the given asset under all NPCs";
				}
			}

			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00039E46 File Offset: 0x00038046
			public override string ExampleUsage
			{
				get
				{
					return "disablenpcasset avatar";
				}
			}

			// Token: 0x06000D03 RID: 3331 RVA: 0x00039E50 File Offset: 0x00038050
			public override void Execute(List<string> args)
			{
				if (args.Count > 0)
				{
					string text = args[0];
					using (List<NPC>.Enumerator enumerator = NPCManager.NPCRegistry.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NPC npc = enumerator.Current;
							for (int i = 0; i < npc.transform.childCount; i++)
							{
								Transform child = npc.transform.GetChild(i);
								if (text == "all" || child.name.ToLower() == text.ToLower())
								{
									child.gameObject.SetActive(false);
								}
							}
						}
						return;
					}
				}
				Console.LogUnrecognizedFormat(new string[]
				{
					this.ExampleUsage
				});
			}
		}

		// Token: 0x0200026D RID: 621
		public class ShowFPS : Console.ConsoleCommand
		{
			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00039F1C File Offset: 0x0003811C
			public override string CommandWord
			{
				get
				{
					return "showfps";
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00039F23 File Offset: 0x00038123
			public override string CommandDescription
			{
				get
				{
					return "Shows FPS label.";
				}
			}

			// Token: 0x170002E3 RID: 739
			// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00039F1C File Offset: 0x0003811C
			public override string ExampleUsage
			{
				get
				{
					return "showfps";
				}
			}

			// Token: 0x06000D08 RID: 3336 RVA: 0x00039F2A File Offset: 0x0003812A
			public override void Execute(List<string> args)
			{
				Singleton<HUD>.Instance.fpsLabel.gameObject.SetActive(true);
			}
		}

		// Token: 0x0200026E RID: 622
		public class HideFPS : Console.ConsoleCommand
		{
			// Token: 0x170002E4 RID: 740
			// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00039F41 File Offset: 0x00038141
			public override string CommandWord
			{
				get
				{
					return "hidefps";
				}
			}

			// Token: 0x170002E5 RID: 741
			// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00039F48 File Offset: 0x00038148
			public override string CommandDescription
			{
				get
				{
					return "Hides FPS label.";
				}
			}

			// Token: 0x170002E6 RID: 742
			// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00039F41 File Offset: 0x00038141
			public override string ExampleUsage
			{
				get
				{
					return "hidefps";
				}
			}

			// Token: 0x06000D0D RID: 3341 RVA: 0x00039F4F File Offset: 0x0003814F
			public override void Execute(List<string> args)
			{
				Singleton<HUD>.Instance.fpsLabel.gameObject.SetActive(false);
			}
		}

		// Token: 0x0200026F RID: 623
		public class ClearTrash : Console.ConsoleCommand
		{
			// Token: 0x170002E7 RID: 743
			// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00039F66 File Offset: 0x00038166
			public override string CommandWord
			{
				get
				{
					return "cleartrash";
				}
			}

			// Token: 0x170002E8 RID: 744
			// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00039F6D File Offset: 0x0003816D
			public override string CommandDescription
			{
				get
				{
					return "Instantly removes all trash from the world.";
				}
			}

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000D11 RID: 3345 RVA: 0x00039F66 File Offset: 0x00038166
			public override string ExampleUsage
			{
				get
				{
					return "cleartrash";
				}
			}

			// Token: 0x06000D12 RID: 3346 RVA: 0x00039F74 File Offset: 0x00038174
			public override void Execute(List<string> args)
			{
				NetworkSingleton<TrashManager>.Instance.DestroyAllTrash();
			}
		}

		// Token: 0x02000270 RID: 624
		public class PlayCutscene : Console.ConsoleCommand
		{
			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00039F80 File Offset: 0x00038180
			public override string CommandWord
			{
				get
				{
					return "playcutscene";
				}
			}

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00039F87 File Offset: 0x00038187
			public override string CommandDescription
			{
				get
				{
					return "Plays the cutscene with the given name";
				}
			}

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00039F8E File Offset: 0x0003818E
			public override string ExampleUsage
			{
				get
				{
					return "playcutscene Tutorial end";
				}
			}

			// Token: 0x06000D17 RID: 3351 RVA: 0x00039F98 File Offset: 0x00038198
			public override void Execute(List<string> args)
			{
				if (args.Count > 0)
				{
					string name = string.Join(" ", args).ToLower();
					if (Singleton<CutsceneManager>.InstanceExists)
					{
						Singleton<CutsceneManager>.Instance.Play(name);
						return;
					}
				}
				else
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
				}
			}
		}

		// Token: 0x02000271 RID: 625
		[Serializable]
		public class LabelledGameObject
		{
			// Token: 0x04000D77 RID: 3447
			public string Label;

			// Token: 0x04000D78 RID: 3448
			public GameObject GameObject;
		}
	}
}
