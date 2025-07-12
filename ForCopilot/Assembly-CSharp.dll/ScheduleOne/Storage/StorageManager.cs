using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Storage
{
	// Token: 0x020008EB RID: 2283
	public class StorageManager : NetworkSingleton<StorageManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x00105034 File Offset: 0x00103234
		public string SaveFolderName
		{
			get
			{
				return "WorldStorageEntities";
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x00105034 File Offset: 0x00103234
		public string SaveFileName
		{
			get
			{
				return "WorldStorageEntities";
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x0010503B File Offset: 0x0010323B
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x00105043 File Offset: 0x00103243
		// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x0010504B File Offset: 0x0010324B
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x00105054 File Offset: 0x00103254
		// (set) Token: 0x06003DDA RID: 15834 RVA: 0x0010505C File Offset: 0x0010325C
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x00105065 File Offset: 0x00103265
		// (set) Token: 0x06003DDC RID: 15836 RVA: 0x0010506D File Offset: 0x0010326D
		public bool HasChanged { get; set; } = true;

		// Token: 0x06003DDD RID: 15837 RVA: 0x00105076 File Offset: 0x00103276
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.StorageManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0010508C File Offset: 0x0010328C
		public Pallet CreatePallet(Vector3 position, Quaternion rotation, string initialSlotGuid = "")
		{
			Pallet component = UnityEngine.Object.Instantiate<GameObject>(this.PalletPrefab).GetComponent<Pallet>();
			component.transform.position = position;
			component.transform.rotation = rotation;
			base.NetworkObject.Spawn(component.gameObject, null, default(Scene));
			if (GUIDManager.IsGUIDValid(initialSlotGuid))
			{
				PalletSlot @object = GUIDManager.GetObject<PalletSlot>(new Guid(initialSlotGuid));
				if (@object != null)
				{
					component.BindToSlot_Server(@object.GUID);
				}
			}
			return component;
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00105108 File Offset: 0x00103308
		public virtual string GetSaveString()
		{
			List<WorldStorageEntityData> list = new List<WorldStorageEntityData>();
			for (int i = 0; i < WorldStorageEntity.All.Count; i++)
			{
				if (WorldStorageEntity.All[i].ShouldSave())
				{
					list.Add(WorldStorageEntity.All[i].GetSaveData());
				}
			}
			return new WorldStorageEntitiesData(list.ToArray()).GetJson(true);
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x00105199 File Offset: 0x00103399
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x001051B2 File Offset: 0x001033B2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.StorageManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x001051CB File Offset: 0x001033CB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x001051D9 File Offset: 0x001033D9
		protected override void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04002C1D RID: 11293
		[Header("Prefabs")]
		public GameObject PalletPrefab;

		// Token: 0x04002C1E RID: 11294
		private StorageLoader loader = new StorageLoader();

		// Token: 0x04002C22 RID: 11298
		private bool dll_Excuted;

		// Token: 0x04002C23 RID: 11299
		private bool dll_Excuted;
	}
}
