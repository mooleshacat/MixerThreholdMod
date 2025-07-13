using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x02000371 RID: 881
	public class PourOntoTargetTask : PourIntoPotTask
	{
		// Token: 0x060013EC RID: 5100 RVA: 0x0005818D File Offset: 0x0005638D
		public PourOntoTargetTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab) : base(_pot, _itemInstance, _pourablePrefab)
		{
			this.Target = _pot.Target;
			_pot.RandomizeTarget();
			_pot.SetTargetActive(true);
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x000581C8 File Offset: 0x000563C8
		public override void Update()
		{
			base.Update();
			Vector3 vector = this.pourable.PourPoint.position - this.Target.position;
			vector.y = 0f;
			if (vector.magnitude < this.SUCCESS_THRESHOLD)
			{
				this.timeOverTarget += Time.deltaTime * this.pourable.NormalizedPourRate;
				if (this.timeOverTarget >= this.SUCCESS_TIME)
				{
					this.TargetReached();
					return;
				}
			}
			else
			{
				this.timeOverTarget = 0f;
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00058255 File Offset: 0x00056455
		public override void StopTask()
		{
			this.pot.SetTargetActive(false);
			base.StopTask();
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00058269 File Offset: 0x00056469
		public virtual void TargetReached()
		{
			this.pot.RandomizeTarget();
			this.timeOverTarget = 0f;
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
		}

		// Token: 0x040012ED RID: 4845
		public Transform Target;

		// Token: 0x040012EE RID: 4846
		public float SUCCESS_THRESHOLD = 0.12f;

		// Token: 0x040012EF RID: 4847
		public float SUCCESS_TIME = 0.4f;

		// Token: 0x040012F0 RID: 4848
		private float timeOverTarget;
	}
}
