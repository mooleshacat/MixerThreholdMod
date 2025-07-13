using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.NPCs
{
	// Token: 0x020004A0 RID: 1184
	public class NPCPathCache
	{
		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x0006E09A File Offset: 0x0006C29A
		// (set) Token: 0x060018E9 RID: 6377 RVA: 0x0006E0A2 File Offset: 0x0006C2A2
		public List<NPCPathCache.PathCache> Paths { get; private set; } = new List<NPCPathCache.PathCache>();

		// Token: 0x060018EA RID: 6378 RVA: 0x0006E0AC File Offset: 0x0006C2AC
		public NavMeshPath GetPath(Vector3 start, Vector3 end, float sqrMaxDistance)
		{
			foreach (NPCPathCache.PathCache pathCache in this.Paths)
			{
				if ((pathCache.Start - start).sqrMagnitude < sqrMaxDistance && (pathCache.End - end).sqrMagnitude < sqrMaxDistance)
				{
					return pathCache.Path;
				}
			}
			return null;
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0006E134 File Offset: 0x0006C334
		public void AddPath(Vector3 start, Vector3 end, NavMeshPath path)
		{
			this.Paths.Add(new NPCPathCache.PathCache(start, end, path));
		}

		// Token: 0x020004A1 RID: 1185
		[Serializable]
		public class PathCache
		{
			// Token: 0x060018ED RID: 6381 RVA: 0x0006E15C File Offset: 0x0006C35C
			public PathCache(Vector3 start, Vector3 end, NavMeshPath path)
			{
				this.Start = start;
				this.End = end;
				this.Path = path;
			}

			// Token: 0x04001606 RID: 5638
			public Vector3 Start;

			// Token: 0x04001607 RID: 5639
			public Vector3 End;

			// Token: 0x04001608 RID: 5640
			public NavMeshPath Path;
		}
	}
}
