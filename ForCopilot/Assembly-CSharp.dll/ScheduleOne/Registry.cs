using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet.Object;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.DevUtilities;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne
{
	// Token: 0x02000272 RID: 626
	public class Registry : PersistentSingleton<Registry>
	{
		// Token: 0x06000D1A RID: 3354 RVA: 0x00039FE8 File Offset: 0x000381E8
		private void OnValidate()
		{
			foreach (Registry.ItemRegister itemRegister in this.ItemRegistry)
			{
				if (string.IsNullOrEmpty(itemRegister.ID))
				{
					Console.LogError("Item ID is empty!", null);
				}
				else if (string.IsNullOrEmpty(itemRegister.AssetPath))
				{
					Console.LogError("Item AssetPath is empty!", null);
				}
				else if (itemRegister.Definition == null)
				{
					Console.LogError("Item Definition is null!", null);
				}
				else
				{
					itemRegister.name = itemRegister.Definition.Name + " (" + itemRegister.Definition.Category.ToString() + ")";
				}
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0003A0C0 File Offset: 0x000382C0
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Registry>.Instance == null || Singleton<Registry>.Instance != this)
			{
				return;
			}
			foreach (Registry.ItemRegister itemRegister in this.ItemRegistry)
			{
				if (this.ItemDictionary.ContainsKey(Registry.GetHash(itemRegister.ID)))
				{
					Console.LogError("Duplicate item ID: " + itemRegister.ID, null);
				}
				else
				{
					this.AddToItemDictionary(itemRegister);
				}
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0003A164 File Offset: 0x00038364
		public static GameObject GetPrefab(string id)
		{
			Registry.ObjectRegister objectRegister = Singleton<Registry>.Instance.ObjectRegistry.Find((Registry.ObjectRegister x) => x.ID.ToLower() == id.ToString());
			if (objectRegister == null)
			{
				return null;
			}
			return objectRegister.Prefab.gameObject;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0003A1AA File Offset: 0x000383AA
		public static ItemDefinition GetItem(string ID)
		{
			return Singleton<Registry>.Instance._GetItem(ID, true);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0003A1B8 File Offset: 0x000383B8
		public static bool ItemExists(string ID)
		{
			return Singleton<Registry>.Instance._GetItem(ID, false) != null;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0003A1CC File Offset: 0x000383CC
		public static T GetItem<T>(string ID) where T : ItemDefinition
		{
			return Singleton<Registry>.Instance._GetItem(ID, true) as T;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0003A1E4 File Offset: 0x000383E4
		public ItemDefinition _GetItem(string ID, bool warnIfNonExistent = true)
		{
			if (string.IsNullOrEmpty(ID))
			{
				return null;
			}
			if (this.itemIDAliases.ContainsKey(ID.ToLower()))
			{
				ID = this.itemIDAliases[ID.ToLower()];
			}
			int hash = Registry.GetHash(ID);
			if (!this.ItemDictionary.ContainsKey(hash))
			{
				if (Singleton<LoadManager>.InstanceExists && !Singleton<LoadManager>.Instance.IsLoading && warnIfNonExistent)
				{
					Console.LogError(string.Concat(new string[]
					{
						"Item '",
						ID,
						"' not found in registry! (Hash = ",
						hash.ToString(),
						")"
					}), null);
				}
				return null;
			}
			Registry.ItemRegister itemRegister = this.ItemDictionary[hash];
			if (itemRegister == null)
			{
				return null;
			}
			return itemRegister.Definition;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0003A2A4 File Offset: 0x000384A4
		public static Constructable GetConstructable(string id)
		{
			GameObject prefab = Registry.GetPrefab(id);
			if (!(prefab != null))
			{
				return null;
			}
			return prefab.GetComponent<Constructable>();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0003A2C9 File Offset: 0x000384C9
		private static int GetHash(string ID)
		{
			return ID.ToLower().GetHashCode();
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0003A2D8 File Offset: 0x000384D8
		private static string RemoveAssetsAndPrefab(string originalString)
		{
			int num = originalString.IndexOf("Assets/");
			if (num != -1)
			{
				originalString = originalString.Substring(num + "Assets/".Length);
			}
			int num2 = originalString.LastIndexOf(".prefab");
			if (num2 != -1)
			{
				originalString = originalString.Substring(0, num2);
			}
			return originalString;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0003A324 File Offset: 0x00038524
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.RemoveRuntimeItems));
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0003A348 File Offset: 0x00038548
		public void AddToRegistry(ItemDefinition item)
		{
			Registry.ItemRegister itemRegister = new Registry.ItemRegister
			{
				Definition = item,
				ID = item.ID,
				AssetPath = string.Empty
			};
			this.ItemRegistry.Add(itemRegister);
			this.AddToItemDictionary(itemRegister);
			if (Application.isPlaying)
			{
				this.ItemsAddedAtRuntime.Add(new Registry.ItemRegister
				{
					Definition = item,
					ID = item.ID,
					AssetPath = string.Empty
				});
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0003A3C1 File Offset: 0x000385C1
		public List<ItemDefinition> GetAllItems()
		{
			return this.ItemRegistry.ConvertAll<ItemDefinition>((Registry.ItemRegister x) => x.Definition);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0003A3F0 File Offset: 0x000385F0
		private void AddToItemDictionary(Registry.ItemRegister reg)
		{
			int hash = Registry.GetHash(reg.ID);
			if (this.ItemDictionary.ContainsKey(hash))
			{
				Console.LogError("Duplicate item ID: " + reg.ID, null);
				return;
			}
			this.ItemDictionary.Add(hash, reg);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0003A43C File Offset: 0x0003863C
		private void RemoveItemFromDictionary(Registry.ItemRegister reg)
		{
			int hash = Registry.GetHash(reg.ID);
			this.ItemDictionary.Remove(hash);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0003A464 File Offset: 0x00038664
		public void RemoveRuntimeItems()
		{
			foreach (Registry.ItemRegister itemRegister in new List<Registry.ItemRegister>(this.ItemsAddedAtRuntime))
			{
				this.RemoveFromRegistry(itemRegister.Definition);
			}
			this.ItemsAddedAtRuntime.Clear();
			Console.Log("Removed runtime items from registry", null);
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0003A4D8 File Offset: 0x000386D8
		public void RemoveFromRegistry(ItemDefinition item)
		{
			Registry.ItemRegister itemRegister = this.ItemRegistry.Find((Registry.ItemRegister x) => x.Definition == item);
			if (itemRegister != null)
			{
				this.ItemRegistry.Remove(itemRegister);
				this.RemoveItemFromDictionary(itemRegister);
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0003A524 File Offset: 0x00038724
		[Button]
		public void LogOrderedUnlocks()
		{
			List<ItemDefinition> list = new List<ItemDefinition>();
			for (int i = 0; i < this.ItemRegistry.Count; i++)
			{
				if ((this.ItemRegistry[i].Definition as StorableItemDefinition).RequiresLevelToPurchase)
				{
					list.Add(this.ItemRegistry[i].Definition);
				}
			}
			list.Sort((ItemDefinition x, ItemDefinition y) => (x as StorableItemDefinition).RequiredRank.CompareTo((y as StorableItemDefinition).RequiredRank));
			Console.Log("Ordered Unlocks:", null);
			foreach (ItemDefinition itemDefinition in list)
			{
				string id = itemDefinition.ID;
				string str = " - ";
				FullRank requiredRank = (itemDefinition as StorableItemDefinition).RequiredRank;
				Console.Log(id + str + requiredRank.ToString(), null);
			}
		}

		// Token: 0x04000D79 RID: 3449
		[SerializeField]
		private List<Registry.ObjectRegister> ObjectRegistry = new List<Registry.ObjectRegister>();

		// Token: 0x04000D7A RID: 3450
		[SerializeField]
		private List<Registry.ItemRegister> ItemRegistry = new List<Registry.ItemRegister>();

		// Token: 0x04000D7B RID: 3451
		[SerializeField]
		private List<Registry.ItemRegister> ItemsAddedAtRuntime = new List<Registry.ItemRegister>();

		// Token: 0x04000D7C RID: 3452
		private Dictionary<int, Registry.ItemRegister> ItemDictionary = new Dictionary<int, Registry.ItemRegister>();

		// Token: 0x04000D7D RID: 3453
		private Dictionary<string, string> itemIDAliases = new Dictionary<string, string>
		{
			{
				"viagra",
				"viagor"
			}
		};

		// Token: 0x04000D7E RID: 3454
		public List<SeedDefinition> Seeds = new List<SeedDefinition>();

		// Token: 0x02000273 RID: 627
		[Serializable]
		public class ObjectRegister
		{
			// Token: 0x04000D7F RID: 3455
			public string ID;

			// Token: 0x04000D80 RID: 3456
			public string AssetPath;

			// Token: 0x04000D81 RID: 3457
			public NetworkObject Prefab;
		}

		// Token: 0x02000274 RID: 628
		[Serializable]
		public class ItemRegister
		{
			// Token: 0x04000D82 RID: 3458
			public string name;

			// Token: 0x04000D83 RID: 3459
			public string ID;

			// Token: 0x04000D84 RID: 3460
			public string AssetPath;

			// Token: 0x04000D85 RID: 3461
			public ItemDefinition Definition;
		}
	}
}
