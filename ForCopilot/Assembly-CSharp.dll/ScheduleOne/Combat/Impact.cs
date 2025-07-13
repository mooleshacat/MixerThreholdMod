using System;
using FishNet.Object;
using FishNet.Serializing.Helping;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000777 RID: 1911
	[Serializable]
	public class Impact
	{
		// Token: 0x06003368 RID: 13160 RVA: 0x000D5F78 File Offset: 0x000D4178
		public Impact(RaycastHit hit, Vector3 hitPoint, Vector3 impactForceDirection, float impactForce, float impactDamage, EImpactType impactType, Player impactSource, int impactID)
		{
			this.Hit = hit;
			this.HitPoint = hitPoint;
			this.ImpactForceDirection = impactForceDirection;
			this.ImpactForce = impactForce;
			this.ImpactDamage = impactDamage;
			this.ImpactType = impactType;
			if (impactSource != null)
			{
				this.ImpactSource = impactSource.NetworkObject;
			}
			this.ImpactID = impactID;
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x0000494F File Offset: 0x00002B4F
		public Impact()
		{
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x000D5FD7 File Offset: 0x000D41D7
		public static bool IsLethal(EImpactType impactType)
		{
			return impactType == EImpactType.SharpMetal || impactType == EImpactType.Bullet || impactType == EImpactType.Explosion;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x000D5FE8 File Offset: 0x000D41E8
		public bool IsPlayerImpact(out Player player)
		{
			if (this.ImpactSource == null)
			{
				player = null;
				return false;
			}
			player = this.ImpactSource.GetComponent<Player>();
			return player != null;
		}

		// Token: 0x04002445 RID: 9285
		[CodegenExclude]
		public RaycastHit Hit;

		// Token: 0x04002446 RID: 9286
		public Vector3 HitPoint;

		// Token: 0x04002447 RID: 9287
		public Vector3 ImpactForceDirection;

		// Token: 0x04002448 RID: 9288
		public float ImpactForce;

		// Token: 0x04002449 RID: 9289
		public float ImpactDamage;

		// Token: 0x0400244A RID: 9290
		public EImpactType ImpactType;

		// Token: 0x0400244B RID: 9291
		public NetworkObject ImpactSource;

		// Token: 0x0400244C RID: 9292
		public int ImpactID;
	}
}
