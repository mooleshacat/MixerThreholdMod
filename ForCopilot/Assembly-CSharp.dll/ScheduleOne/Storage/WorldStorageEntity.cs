using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008F5 RID: 2293
	public class WorldStorageEntity : StorageEntity, IGUIDRegisterable, ISaveable
	{
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06003E26 RID: 15910 RVA: 0x001063A0 File Offset: 0x001045A0
		// (set) Token: 0x06003E27 RID: 15911 RVA: 0x001063A8 File Offset: 0x001045A8
		public Guid GUID { get; protected set; }

		// Token: 0x06003E28 RID: 15912 RVA: 0x001063B4 File Offset: 0x001045B4
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06003E29 RID: 15913 RVA: 0x001063DC File Offset: 0x001045DC
		public string SaveFolderName
		{
			get
			{
				return "Entity_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06003E2A RID: 15914 RVA: 0x00106410 File Offset: 0x00104610
		public string SaveFileName
		{
			get
			{
				return "Entity_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06003E2B RID: 15915 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003E2C RID: 15916 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003E2D RID: 15917 RVA: 0x00106442 File Offset: 0x00104642
		// (set) Token: 0x06003E2E RID: 15918 RVA: 0x0010644A File Offset: 0x0010464A
		public List<string> LocalExtraFiles { get; set; } = new List<string>
		{
			"Contents"
		};

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003E2F RID: 15919 RVA: 0x00106453 File Offset: 0x00104653
		// (set) Token: 0x06003E30 RID: 15920 RVA: 0x0010645B File Offset: 0x0010465B
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x00106464 File Offset: 0x00104664
		// (set) Token: 0x06003E32 RID: 15922 RVA: 0x0010646C File Offset: 0x0010466C
		public bool HasChanged { get; set; }

		// Token: 0x06003E33 RID: 15923 RVA: 0x00106478 File Offset: 0x00104678
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.WorldStorageEntity_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x00106497 File Offset: 0x00104697
		protected override void OnDestroy()
		{
			WorldStorageEntity.All.Remove(this);
			base.OnDestroy();
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x001064AB File Offset: 0x001046AB
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x001064BA File Offset: 0x001046BA
		public virtual bool ShouldSave()
		{
			return base.ItemCount > 0;
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x001064C5 File Offset: 0x001046C5
		public WorldStorageEntityData GetSaveData()
		{
			return new WorldStorageEntityData(this.GUID, new ItemSet(base.ItemSlots));
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x001064DD File Offset: 0x001046DD
		public virtual string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x001064EB File Offset: 0x001046EB
		public virtual void Load(WorldStorageEntityData data)
		{
			data.Contents.LoadTo(base.ItemSlots);
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x001064FE File Offset: 0x001046FE
		protected override void ContentsChanged()
		{
			base.ContentsChanged();
			this.HasChanged = true;
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x0010654D File Offset: 0x0010474D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x00106566 File Offset: 0x00104766
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.WorldStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x0010657F File Offset: 0x0010477F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x00106590 File Offset: 0x00104790
		protected override void dll()
		{
			base.Awake();
			WorldStorageEntity.All.Add(this);
			if (!GUIDManager.IsGUIDValid(this.BakedGUID))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is not valid! Bad.", null);
			}
			if (GUIDManager.IsGUIDAlreadyRegistered(new Guid(this.BakedGUID)))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is already registered! Bad.", this);
			}
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
			this.InitializeSaveable();
		}

		// Token: 0x04002C4B RID: 11339
		public static List<WorldStorageEntity> All = new List<WorldStorageEntity>();

		// Token: 0x04002C4D RID: 11341
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04002C51 RID: 11345
		private bool dll_Excuted;

		// Token: 0x04002C52 RID: 11346
		private bool dll_Excuted;
	}
}
