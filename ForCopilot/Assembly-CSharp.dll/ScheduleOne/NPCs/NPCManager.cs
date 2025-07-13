using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000496 RID: 1174
	public class NPCManager : NetworkSingleton<NPCManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x0006BBA2 File Offset: 0x00069DA2
		public string SaveFolderName
		{
			get
			{
				return "NPCs";
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x0006BBA2 File Offset: 0x00069DA2
		public string SaveFileName
		{
			get
			{
				return "NPCs";
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x0006BBA9 File Offset: 0x00069DA9
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001864 RID: 6244 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x0006BBB1 File Offset: 0x00069DB1
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x0006BBB9 File Offset: 0x00069DB9
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001867 RID: 6247 RVA: 0x0006BBC2 File Offset: 0x00069DC2
		// (set) Token: 0x06001868 RID: 6248 RVA: 0x0006BBCA File Offset: 0x00069DCA
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001869 RID: 6249 RVA: 0x0006BBD3 File Offset: 0x00069DD3
		// (set) Token: 0x0600186A RID: 6250 RVA: 0x0006BBDB File Offset: 0x00069DDB
		public bool HasChanged { get; set; }

		// Token: 0x0600186B RID: 6251 RVA: 0x0006BBE4 File Offset: 0x00069DE4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0006BBF8 File Offset: 0x00069DF8
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(delegate()
			{
				NPCManager.NPCRegistry.Clear();
			});
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x000045B1 File Offset: 0x000027B1
		public void Update()
		{
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0006BC30 File Offset: 0x00069E30
		public static NPC GetNPC(string id)
		{
			foreach (NPC npc in NPCManager.NPCRegistry)
			{
				if (npc.ID.ToLower() == id.ToLower())
				{
					return npc;
				}
			}
			return null;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0006BC9C File Offset: 0x00069E9C
		public static List<NPC> GetNPCsInRegion(EMapRegion region)
		{
			List<NPC> list = new List<NPC>();
			foreach (NPC npc in NPCManager.NPCRegistry)
			{
				if (!(npc == null) && npc.Region == region)
				{
					list.Add(npc);
				}
			}
			return list;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x0006BD08 File Offset: 0x00069F08
		public virtual string GetSaveString()
		{
			List<DynamicSaveData> list = new List<DynamicSaveData>();
			for (int i = 0; i < NPCManager.NPCRegistry.Count; i++)
			{
				if (NPCManager.NPCRegistry[i].ShouldSave())
				{
					list.Add(NPCManager.NPCRegistry[i].GetSaveData());
				}
			}
			return new NPCCollectionData(list.ToArray()).GetJson(true);
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0006BD6C File Offset: 0x00069F6C
		public List<Transform> GetOrderedDistanceWarpPoints(Vector3 origin)
		{
			return (from x in new List<Transform>(this.NPCWarpPoints)
			orderby Vector3.SqrMagnitude(x.position - origin)
			select x).ToList<Transform>();
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0006BDA8 File Offset: 0x00069FA8
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < NPCManager.NPCRegistry.Count; i++)
			{
				if (NPCManager.NPCRegistry[i].ShouldSave())
				{
					new SaveRequest(NPCManager.NPCRegistry[i], containerFolder);
					list.Add(NPCManager.NPCRegistry[i].SaveFolderName);
				}
			}
			return list;
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0006BE48 File Offset: 0x0006A048
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0006BE61 File Offset: 0x0006A061
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0006BE7A File Offset: 0x0006A07A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0006BE88 File Offset: 0x0006A088
		protected override void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x0400159E RID: 5534
		public static List<NPC> NPCRegistry = new List<NPC>();

		// Token: 0x0400159F RID: 5535
		public Transform[] NPCWarpPoints;

		// Token: 0x040015A0 RID: 5536
		public Transform NPCContainer;

		// Token: 0x040015A1 RID: 5537
		[Header("Employee Prefabs")]
		public GameObject BotanistPrefab;

		// Token: 0x040015A2 RID: 5538
		public GameObject PackagerPrefab;

		// Token: 0x040015A3 RID: 5539
		[Header("Prefabs")]
		public NPCPoI NPCPoIPrefab;

		// Token: 0x040015A4 RID: 5540
		public NPCPoI PotentialCustomerPoIPrefab;

		// Token: 0x040015A5 RID: 5541
		public NPCPoI PotentialDealerPoIPrefab;

		// Token: 0x040015A6 RID: 5542
		private NPCsLoader loader = new NPCsLoader();

		// Token: 0x040015AA RID: 5546
		private bool dll_Excuted;

		// Token: 0x040015AB RID: 5547
		private bool dll_Excuted;
	}
}
