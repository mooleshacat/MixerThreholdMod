using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000876 RID: 2166
	[RequireComponent(typeof(BoxCollider))]
	public class TrashRemovalVolume : MonoBehaviour
	{
		// Token: 0x06003B64 RID: 15204 RVA: 0x000FB869 File Offset: 0x000F9A69
		public void Awake()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.SleepStart));
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x000FB886 File Offset: 0x000F9A86
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.SleepStart));
			}
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x000FB8AC File Offset: 0x000F9AAC
		private void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (UnityEngine.Random.value > this.RemovalChance)
			{
				return;
			}
			TrashItem[] trash = this.GetTrash();
			for (int i = 0; i < trash.Length; i++)
			{
				trash[i].DestroyTrash();
			}
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x000FB8EC File Offset: 0x000F9AEC
		private TrashItem[] GetTrash()
		{
			List<TrashItem> list = new List<TrashItem>();
			Vector3 vector = this.Collider.transform.TransformPoint(this.Collider.center);
			Vector3 vector2 = Vector3.Scale(this.Collider.size, this.Collider.transform.lossyScale) * 0.5f;
			Collider[] array = Physics.OverlapBox(vector, vector2, this.Collider.transform.rotation, 1 << LayerMask.NameToLayer("Trash"), 2);
			for (int i = 0; i < array.Length; i++)
			{
				TrashItem componentInParent = array[i].GetComponentInParent<TrashItem>();
				if (componentInParent != null)
				{
					list.Add(componentInParent);
				}
			}
			return list.ToArray();
		}

		// Token: 0x04002A63 RID: 10851
		public BoxCollider Collider;

		// Token: 0x04002A64 RID: 10852
		public float RemovalChance = 1f;
	}
}
