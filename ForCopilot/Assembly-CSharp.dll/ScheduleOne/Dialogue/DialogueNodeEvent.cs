using System;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006EC RID: 1772
	[Serializable]
	public class DialogueNodeEvent
	{
		// Token: 0x04002218 RID: 8728
		public string NodeLabel;

		// Token: 0x04002219 RID: 8729
		public UnityEvent onNodeDisplayed;
	}
}
