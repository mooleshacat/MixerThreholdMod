using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000377 RID: 887
	public class GenericSaveablesManager : Singleton<GenericSaveablesManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x00058FD7 File Offset: 0x000571D7
		public string SaveFolderName
		{
			get
			{
				return "GenericSaveables";
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x00058FD7 File Offset: 0x000571D7
		public string SaveFileName
		{
			get
			{
				return "GenericSaveables";
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x00058FDE File Offset: 0x000571DE
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00058FE6 File Offset: 0x000571E6
		// (set) Token: 0x06001413 RID: 5139 RVA: 0x00058FEE File Offset: 0x000571EE
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x00058FF7 File Offset: 0x000571F7
		// (set) Token: 0x06001415 RID: 5141 RVA: 0x00058FFF File Offset: 0x000571FF
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00059008 File Offset: 0x00057208
		// (set) Token: 0x06001417 RID: 5143 RVA: 0x00059010 File Offset: 0x00057210
		public bool HasChanged { get; set; } = true;

		// Token: 0x06001418 RID: 5144 RVA: 0x00059019 File Offset: 0x00057219
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00059027 File Offset: 0x00057227
		public void RegisterSaveable(IGenericSaveable saveable)
		{
			if (this.Saveables.Contains(saveable))
			{
				return;
			}
			this.Saveables.Add(saveable);
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00059044 File Offset: 0x00057244
		public virtual string GetSaveString()
		{
			List<GenericSaveData> list = new List<GenericSaveData>();
			for (int i = 0; i < this.Saveables.Count; i++)
			{
				if (this.Saveables[i] != null)
				{
					list.Add(this.Saveables[i].GetSaveData());
				}
			}
			return new GenericSaveablesData(list.ToArray()).GetJson(true);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x000590A4 File Offset: 0x000572A4
		public void LoadSaveable(GenericSaveData data)
		{
			if (!GUIDManager.IsGUIDValid(data.GUID))
			{
				Console.LogWarning("Invalid GUID found in generic save data: " + data.GUID, null);
				return;
			}
			Guid guid = new Guid(data.GUID);
			IGenericSaveable genericSaveable = this.Saveables.Find((IGenericSaveable x) => x.GUID == guid);
			if (genericSaveable == null)
			{
				string str = "No saveable found with GUID: ";
				Guid guid2 = guid;
				Console.LogWarning(str + guid2.ToString(), null);
				return;
			}
			genericSaveable.Load(data);
		}

		// Token: 0x04001301 RID: 4865
		protected List<IGenericSaveable> Saveables = new List<IGenericSaveable>();

		// Token: 0x04001302 RID: 4866
		private GenericSaveablesLoader loader = new GenericSaveablesLoader();
	}
}
