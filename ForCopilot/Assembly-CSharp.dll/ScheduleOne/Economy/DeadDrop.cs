using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.Storage;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x020006A4 RID: 1700
	public class DeadDrop : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002E23 RID: 11811 RVA: 0x000C052B File Offset: 0x000BE72B
		// (set) Token: 0x06002E24 RID: 11812 RVA: 0x000C0533 File Offset: 0x000BE733
		public Guid GUID { get; protected set; }

		// Token: 0x06002E25 RID: 11813 RVA: 0x000C053C File Offset: 0x000BE73C
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000C0562 File Offset: 0x000BE762
		protected virtual void Awake()
		{
			DeadDrop.DeadDrops.Add(this);
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000C0586 File Offset: 0x000BE786
		private void OnValidate()
		{
			base.gameObject.name = this.DeadDropName;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000C059C File Offset: 0x000BE79C
		protected virtual void Start()
		{
			base.GetComponent<StorageEntity>().StorageEntitySubtitle = this.DeadDropName;
			this.PoI.SetMainText("Dead Drop\n(" + this.DeadDropName + ")");
			this.UpdateDeadDrop();
			this.Storage.onContentsChanged.AddListener(new UnityAction(this.UpdateDeadDrop));
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000C05FC File Offset: 0x000BE7FC
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000C060B File Offset: 0x000BE80B
		public void OnDestroy()
		{
			DeadDrop.DeadDrops.Remove(this);
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000C061C File Offset: 0x000BE81C
		public static DeadDrop GetRandomEmptyDrop(Vector3 origin)
		{
			List<DeadDrop> list = (from drop in DeadDrop.DeadDrops
			where drop.Storage.ItemCount == 0
			select drop).ToList<DeadDrop>();
			list = (from drop in list
			orderby Vector3.Distance(drop.transform.position, origin)
			select drop).ToList<DeadDrop>();
			list.RemoveAt(0);
			list.RemoveRange(list.Count / 2, list.Count / 2);
			if (list.Count == 0)
			{
				return null;
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000C06B8 File Offset: 0x000BE8B8
		private void UpdateDeadDrop()
		{
			this.PoI.enabled = false;
			this.Light.Enabled = (this.Storage.ItemCount > 0);
			if (this.ItemCountVariable != string.Empty)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.ItemCountVariable, this.Storage.ItemCount.ToString(), true);
			}
		}

		// Token: 0x0400209C RID: 8348
		public static List<DeadDrop> DeadDrops = new List<DeadDrop>();

		// Token: 0x0400209D RID: 8349
		public string DeadDropName;

		// Token: 0x0400209E RID: 8350
		public string DeadDropDescription;

		// Token: 0x0400209F RID: 8351
		public StorageEntity Storage;

		// Token: 0x040020A0 RID: 8352
		public POI PoI;

		// Token: 0x040020A1 RID: 8353
		public OptimizedLight Light;

		// Token: 0x040020A2 RID: 8354
		public string ItemCountVariable = string.Empty;

		// Token: 0x040020A4 RID: 8356
		[SerializeField]
		protected string BakedGUID = string.Empty;
	}
}
