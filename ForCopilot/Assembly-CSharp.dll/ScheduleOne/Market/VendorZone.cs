using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Market
{
	// Token: 0x02000599 RID: 1433
	public class VendorZone : MonoBehaviour
	{
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x0008EDB2 File Offset: 0x0008CFB2
		public bool isOpen
		{
			get
			{
				return NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.openTime, this.closeTime);
			}
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x0008EDCA File Offset: 0x0008CFCA
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPassed));
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x0008EDF2 File Offset: 0x0008CFF2
		private void MinPassed()
		{
			if (this.isOpen)
			{
				this.SetDoorsActive(false);
				return;
			}
			if (!this.IsPlayerWithinVendorZone())
			{
				this.SetDoorsActive(true);
			}
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x0008EE14 File Offset: 0x0008D014
		private bool IsPlayerWithinVendorZone()
		{
			return this.zoneCollider.bounds.Contains(PlayerSingleton<PlayerMovement>.Instance.transform.position);
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x0008EE44 File Offset: 0x0008D044
		private void SetDoorsActive(bool a)
		{
			for (int i = 0; i < this.doors.Count; i++)
			{
				this.doors[i].SetActive(a);
			}
		}

		// Token: 0x04001A52 RID: 6738
		[Header("References")]
		[SerializeField]
		protected BoxCollider zoneCollider;

		// Token: 0x04001A53 RID: 6739
		[SerializeField]
		protected List<GameObject> doors = new List<GameObject>();

		// Token: 0x04001A54 RID: 6740
		[Header("Settings")]
		[SerializeField]
		protected int openTime = 600;

		// Token: 0x04001A55 RID: 6741
		[SerializeField]
		protected int closeTime = 1800;
	}
}
