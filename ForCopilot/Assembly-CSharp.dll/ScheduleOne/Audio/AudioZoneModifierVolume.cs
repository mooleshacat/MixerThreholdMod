using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007E1 RID: 2017
	public class AudioZoneModifierVolume : MonoBehaviour
	{
		// Token: 0x0600369D RID: 13981 RVA: 0x000E60E6 File Offset: 0x000E42E6
		private void Start()
		{
			base.InvokeRepeating("Refresh", 0f, 0.25f);
			this.colliders = base.GetComponentsInChildren<BoxCollider>();
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000E6120 File Offset: 0x000E4320
		private void Refresh()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			BoxCollider[] array = this.colliders;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].bounds.Contains(PlayerSingleton<PlayerCamera>.Instance.transform.position))
				{
					foreach (AudioZone audioZone in this.Zones)
					{
						audioZone.AddModifier(this, this.VolumeMultiplier);
					}
					return;
				}
			}
			foreach (AudioZone audioZone2 in this.Zones)
			{
				audioZone2.RemoveModifier(this);
			}
		}

		// Token: 0x040026DE RID: 9950
		public List<AudioZone> Zones = new List<AudioZone>();

		// Token: 0x040026DF RID: 9951
		public float VolumeMultiplier = 0.5f;

		// Token: 0x040026E0 RID: 9952
		private BoxCollider[] colliders;
	}
}
