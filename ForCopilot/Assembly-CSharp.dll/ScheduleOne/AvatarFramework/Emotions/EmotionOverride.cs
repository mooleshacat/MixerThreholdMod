using System;

namespace ScheduleOne.AvatarFramework.Emotions
{
	// Token: 0x020009C5 RID: 2501
	public class EmotionOverride
	{
		// Token: 0x060043A8 RID: 17320 RVA: 0x0011C225 File Offset: 0x0011A425
		public EmotionOverride(string emotion, string label, int priority)
		{
			this.Emotion = emotion;
			this.Label = label;
			this.Priority = priority;
		}

		// Token: 0x04003081 RID: 12417
		public string Emotion;

		// Token: 0x04003082 RID: 12418
		public string Label;

		// Token: 0x04003083 RID: 12419
		public int Priority;
	}
}
