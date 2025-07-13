using System;
using ScheduleOne.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ScriptableObjects
{
	// Token: 0x020007B9 RID: 1977
	[CreateAssetMenu(fileName = "PhoneCallData", menuName = "ScriptableObjects/PhoneCallData", order = 1)]
	[Serializable]
	public class PhoneCallData : ScriptableObject
	{
		// Token: 0x060035C5 RID: 13765 RVA: 0x000E114A File Offset: 0x000DF34A
		public void Completed()
		{
			if (this.onCallCompleted != null)
			{
				this.onCallCompleted.Invoke();
			}
		}

		// Token: 0x0400261E RID: 9758
		public CallerID CallerID;

		// Token: 0x0400261F RID: 9759
		public PhoneCallData.Stage[] Stages;

		// Token: 0x04002620 RID: 9760
		public UnityEvent onCallCompleted;

		// Token: 0x020007BA RID: 1978
		[Serializable]
		public class Stage
		{
			// Token: 0x060035C7 RID: 13767 RVA: 0x000E1160 File Offset: 0x000DF360
			public void OnStageStart()
			{
				if (this.OnStartTriggers != null)
				{
					for (int i = 0; i < this.OnStartTriggers.Length; i++)
					{
						this.OnStartTriggers[i].Trigger();
					}
				}
			}

			// Token: 0x060035C8 RID: 13768 RVA: 0x000E1198 File Offset: 0x000DF398
			public void OnStageEnd()
			{
				if (this.OnDoneTriggers != null)
				{
					for (int i = 0; i < this.OnDoneTriggers.Length; i++)
					{
						this.OnDoneTriggers[i].Trigger();
					}
				}
			}

			// Token: 0x04002621 RID: 9761
			[TextArea(3, 10)]
			public string Text;

			// Token: 0x04002622 RID: 9762
			public SystemTrigger[] OnStartTriggers;

			// Token: 0x04002623 RID: 9763
			public SystemTrigger[] OnDoneTriggers;
		}
	}
}
