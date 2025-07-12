using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x0200086F RID: 2159
	[RequireComponent(typeof(BoxCollider))]
	public class TrashGenerator : MonoBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x000F9030 File Offset: 0x000F7230
		public string SaveFolderName
		{
			get
			{
				return "Generator_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x000F9064 File Offset: 0x000F7264
		public string SaveFileName
		{
			get
			{
				return "Generator_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x000F9096 File Offset: 0x000F7296
		// (set) Token: 0x06003ADA RID: 15066 RVA: 0x000F909E File Offset: 0x000F729E
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x000F90A7 File Offset: 0x000F72A7
		// (set) Token: 0x06003ADC RID: 15068 RVA: 0x000F90AF File Offset: 0x000F72AF
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x000F90B8 File Offset: 0x000F72B8
		// (set) Token: 0x06003ADE RID: 15070 RVA: 0x000F90C0 File Offset: 0x000F72C0
		public bool HasChanged { get; set; }

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000F90C9 File Offset: 0x000F72C9
		// (set) Token: 0x06003AE0 RID: 15072 RVA: 0x000F90D1 File Offset: 0x000F72D1
		public Guid GUID { get; protected set; }

		// Token: 0x06003AE1 RID: 15073 RVA: 0x000F90DA File Offset: 0x000F72DA
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x000F90E9 File Offset: 0x000F72E9
		private void Awake()
		{
			TrashGenerator.AllGenerators.Add(this);
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x000F90F8 File Offset: 0x000F72F8
		private void Start()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.SleepStart));
			this.boxCollider = base.GetComponent<BoxCollider>();
			this.boxCollider.isTrigger = true;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
			this.GUID = new Guid(this.StaticGUID);
			GUIDManager.RegisterObject(this);
			this.InitializeSaveable();
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x000F916A File Offset: 0x000F736A
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.StaticGUID))
			{
				this.RegenerateGUID();
			}
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x000F917F File Offset: 0x000F737F
		private void OnDestroy()
		{
			TrashGenerator.AllGenerators.Remove(this);
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x000F9190 File Offset: 0x000F7390
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			this.boxCollider = base.GetComponent<BoxCollider>();
			Gizmos.DrawWireCube(this.boxCollider.bounds.center, new Vector3(this.boxCollider.size.x * base.transform.localScale.x, this.boxCollider.size.y * base.transform.localScale.y, this.boxCollider.size.z * base.transform.localScale.z));
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x000F9234 File Offset: 0x000F7434
		public void AddGeneratedTrash(TrashItem item)
		{
			if (this.generatedTrash.Contains(item))
			{
				return;
			}
			item.onDestroyed = (Action<TrashItem>)Delegate.Combine(item.onDestroyed, new Action<TrashItem>(this.RemoveGeneratedTrash));
			this.generatedTrash.Add(item);
			this.HasChanged = true;
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x000F9285 File Offset: 0x000F7485
		public void RemoveGeneratedTrash(TrashItem item)
		{
			item.onDestroyed = (Action<TrashItem>)Delegate.Remove(item.onDestroyed, new Action<TrashItem>(this.RemoveGeneratedTrash));
			this.generatedTrash.Remove(item);
			this.HasChanged = true;
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x000F92C0 File Offset: 0x000F74C0
		[Button]
		private void RegenerateGUID()
		{
			this.StaticGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x000F92E8 File Offset: 0x000F74E8
		[Button]
		private void AutoCalculateTrashCount()
		{
			this.boxCollider = base.GetComponent<BoxCollider>();
			float num = this.boxCollider.size.x * base.transform.localScale.x * (this.boxCollider.size.z * base.transform.localScale.z);
			this.MaxTrashCount = Mathf.FloorToInt(num * 0.015f);
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x000F9357 File Offset: 0x000F7557
		[Button]
		private void GenerateMaxTrash()
		{
			this.GenerateTrash(this.MaxTrashCount - this.generatedTrash.Count);
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x000F9374 File Offset: 0x000F7574
		private void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			int num = Mathf.Min(this.MaxTrashCount - this.generatedTrash.Count, Mathf.FloorToInt((float)this.MaxTrashCount * 0.2f));
			if (num <= 0)
			{
				return;
			}
			this.GenerateTrash(num);
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x000F93C0 File Offset: 0x000F75C0
		private void GenerateTrash(int count)
		{
			Console.Log("Generating " + count.ToString() + " trash items", null);
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = new Vector3(UnityEngine.Random.Range(this.boxCollider.bounds.min.x, this.boxCollider.bounds.max.x), UnityEngine.Random.Range(this.boxCollider.bounds.min.y, this.boxCollider.bounds.max.y), UnityEngine.Random.Range(this.boxCollider.bounds.min.z, this.boxCollider.bounds.max.z));
				RaycastHit raycastHit;
				vector = (Physics.Raycast(vector, Vector3.down, ref raycastHit, 20f, this.GroundCheckMask) ? raycastHit.point : vector);
				int num = 0;
				NavMeshHit navMeshHit;
				while (!NavMeshUtility.SamplePosition(vector, out navMeshHit, 1.5f, -1, true))
				{
					if (num > 10)
					{
						Console.Log("Failed to find a valid position for trash item", null);
						break;
					}
					vector = new Vector3(UnityEngine.Random.Range(this.boxCollider.bounds.min.x, this.boxCollider.bounds.max.x), UnityEngine.Random.Range(this.boxCollider.bounds.min.y, this.boxCollider.bounds.max.y), UnityEngine.Random.Range(this.boxCollider.bounds.min.z, this.boxCollider.bounds.max.z));
					vector = (Physics.Raycast(vector, Vector3.down, ref raycastHit, 20f, this.GroundCheckMask) ? raycastHit.point : vector);
					num++;
				}
				vector += Vector3.up * 0.5f;
				TrashItem randomGeneratableTrashPrefab = NetworkSingleton<TrashManager>.Instance.GetRandomGeneratableTrashPrefab();
				TrashItem trashItem = NetworkSingleton<TrashManager>.Instance.CreateTrashItem(randomGeneratableTrashPrefab.ID, vector, UnityEngine.Random.rotation, default(Vector3), "", false);
				trashItem.SetContinuousCollisionDetection();
				this.AddGeneratedTrash(trashItem);
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x000F9635 File Offset: 0x000F7835
		public bool ShouldSave()
		{
			return this.generatedTrash.Count > 0;
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000F9648 File Offset: 0x000F7848
		public virtual TrashGeneratorData GetSaveData()
		{
			return new TrashGeneratorData(this.GUID.ToString(), this.generatedTrash.ConvertAll<string>((TrashItem x) => x.GUID.ToString()).ToArray());
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x000F969D File Offset: 0x000F789D
		public string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x04002A2E RID: 10798
		public const float TRASH_GENERATION_FRACTION = 0.2f;

		// Token: 0x04002A2F RID: 10799
		public const float DEFAULT_TRASH_PER_M2 = 0.015f;

		// Token: 0x04002A30 RID: 10800
		public static List<TrashGenerator> AllGenerators = new List<TrashGenerator>();

		// Token: 0x04002A31 RID: 10801
		[Range(1f, 200f)]
		[SerializeField]
		private int MaxTrashCount = 10;

		// Token: 0x04002A32 RID: 10802
		[SerializeField]
		private List<TrashItem> generatedTrash = new List<TrashItem>();

		// Token: 0x04002A33 RID: 10803
		[Header("Settings")]
		public LayerMask GroundCheckMask;

		// Token: 0x04002A34 RID: 10804
		private BoxCollider boxCollider;

		// Token: 0x04002A39 RID: 10809
		public string StaticGUID = string.Empty;
	}
}
