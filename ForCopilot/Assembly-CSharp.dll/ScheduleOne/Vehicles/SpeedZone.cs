using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x0200080A RID: 2058
	[RequireComponent(typeof(BoxCollider))]
	public class SpeedZone : MonoBehaviour
	{
		// Token: 0x0600380F RID: 14351 RVA: 0x000EC6C4 File Offset: 0x000EA8C4
		public virtual void Awake()
		{
			SpeedZone.speedZones.Add(this);
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x000EC6D4 File Offset: 0x000EA8D4
		public static List<SpeedZone> GetSpeedZones(Vector3 point)
		{
			List<SpeedZone> list = new List<SpeedZone>();
			for (int i = 0; i < SpeedZone.speedZones.Count; i++)
			{
				if (SpeedZone.speedZones[i].col.bounds.Contains(point))
				{
					list.Add(SpeedZone.speedZones[i]);
				}
			}
			return list;
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmosSelected()
		{
		}

		// Token: 0x040027DB RID: 10203
		public static List<SpeedZone> speedZones = new List<SpeedZone>();

		// Token: 0x040027DC RID: 10204
		public BoxCollider col;

		// Token: 0x040027DD RID: 10205
		public float speed = 20f;
	}
}
