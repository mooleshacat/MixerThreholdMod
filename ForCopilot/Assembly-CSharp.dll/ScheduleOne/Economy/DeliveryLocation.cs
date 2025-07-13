using System;
using System.Collections.Generic;
using ScheduleOne.Map;
using ScheduleOne.Quests;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x020006B1 RID: 1713
	public class DeliveryLocation : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002EE6 RID: 12006 RVA: 0x000C4AD1 File Offset: 0x000C2CD1
		// (set) Token: 0x06002EE7 RID: 12007 RVA: 0x000C4AD9 File Offset: 0x000C2CD9
		public Guid GUID { get; protected set; }

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000C4AE2 File Offset: 0x000C2CE2
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000C4AF4 File Offset: 0x000C2CF4
		private void Awake()
		{
			this.PoI.gameObject.SetActive(false);
			if (!GUIDManager.IsGUIDValid(this.StaticGUID) || GUIDManager.IsGUIDAlreadyRegistered(new Guid(this.StaticGUID)))
			{
				Console.LogError("Delivery location Static GUID is not valid.", null);
				return;
			}
			((IGUIDRegisterable)this).SetGUID(this.StaticGUID);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000C4B49 File Offset: 0x000C2D49
		private void OnValidate()
		{
			base.gameObject.name = this.LocationName;
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000C4B5C File Offset: 0x000C2D5C
		public virtual string GetDescription()
		{
			return this.LocationDescription;
		}

		// Token: 0x040020FF RID: 8447
		public string LocationName = string.Empty;

		// Token: 0x04002100 RID: 8448
		public string LocationDescription = string.Empty;

		// Token: 0x04002101 RID: 8449
		public Transform CustomerStandPoint;

		// Token: 0x04002102 RID: 8450
		public Transform TeleportPoint;

		// Token: 0x04002103 RID: 8451
		public POI PoI;

		// Token: 0x04002104 RID: 8452
		public string StaticGUID = string.Empty;

		// Token: 0x04002105 RID: 8453
		public List<Contract> ScheduledContracts = new List<Contract>();
	}
}
