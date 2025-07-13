using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000877 RID: 2167
	public class TrashSpawnVolume : MonoBehaviour
	{
		// Token: 0x06003B69 RID: 15209 RVA: 0x000FB9AF File Offset: 0x000F9BAF
		public void Awake()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.SleepStart));
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x000FB9CC File Offset: 0x000F9BCC
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.SleepStart));
			}
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x000FB9F0 File Offset: 0x000F9BF0
		public void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (UnityEngine.Random.value > this.TrashSpawnChance)
			{
				return;
			}
			Collider[] array = Physics.OverlapBox(this.DetectionVolume.transform.TransformPoint(this.DetectionVolume.center), Vector3.Scale(this.DetectionVolume.size, this.DetectionVolume.transform.lossyScale) * 0.5f, this.DetectionVolume.transform.rotation, 1 << LayerMask.NameToLayer("Trash"), 2);
			int num = 0;
			foreach (Collider collider in array)
			{
				if (num >= this.TrashLimit)
				{
					break;
				}
				if (collider.GetComponentInParent<TrashItem>() != null)
				{
					num++;
				}
			}
			num = Mathf.Max(UnityEngine.Random.Range(0, this.TrashLimit - num), 0);
			for (int j = num; j < this.TrashLimit; j++)
			{
				TrashItem randomGeneratableTrashPrefab = NetworkSingleton<TrashManager>.Instance.GetRandomGeneratableTrashPrefab();
				Vector3 posiiton = new Vector3(UnityEngine.Random.Range(this.CreatonVolume.bounds.min.x, this.CreatonVolume.bounds.max.x), UnityEngine.Random.Range(this.CreatonVolume.bounds.min.y, this.CreatonVolume.bounds.max.y), UnityEngine.Random.Range(this.CreatonVolume.bounds.min.z, this.CreatonVolume.bounds.max.z));
				NetworkSingleton<TrashManager>.Instance.CreateTrashItem(randomGeneratableTrashPrefab.ID, posiiton, UnityEngine.Random.rotation, default(Vector3), "", false).SetContinuousCollisionDetection();
			}
		}

		// Token: 0x04002A65 RID: 10853
		public BoxCollider CreatonVolume;

		// Token: 0x04002A66 RID: 10854
		public BoxCollider DetectionVolume;

		// Token: 0x04002A67 RID: 10855
		public int TrashLimit = 10;

		// Token: 0x04002A68 RID: 10856
		public float TrashSpawnChance = 1f;
	}
}
