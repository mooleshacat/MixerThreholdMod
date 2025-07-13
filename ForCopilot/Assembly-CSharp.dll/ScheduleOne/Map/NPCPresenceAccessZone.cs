using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C7F RID: 3199
	public class NPCPresenceAccessZone : AccessZone
	{
		// Token: 0x060059D5 RID: 22997 RVA: 0x0017B229 File Offset: 0x00179429
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x0017B231 File Offset: 0x00179431
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x0017B25C File Offset: 0x0017945C
		protected virtual void MinPass()
		{
			if (this.TargetNPC == null)
			{
				return;
			}
			this.SetIsOpen(this.DetectionZone.bounds.Contains(this.TargetNPC.Avatar.CenterPoint));
		}

		// Token: 0x040041E8 RID: 16872
		public const float CooldownTime = 0.5f;

		// Token: 0x040041E9 RID: 16873
		public Collider DetectionZone;

		// Token: 0x040041EA RID: 16874
		public NPC TargetNPC;

		// Token: 0x040041EB RID: 16875
		private float timeSinceNPCSensed = float.MaxValue;
	}
}
