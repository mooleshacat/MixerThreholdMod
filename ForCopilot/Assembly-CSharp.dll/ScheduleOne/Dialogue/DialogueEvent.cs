using System;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006EB RID: 1771
	[Serializable]
	public class DialogueEvent
	{
		// Token: 0x04002215 RID: 8725
		public DialogueContainer Dialogue;

		// Token: 0x04002216 RID: 8726
		public UnityEvent onDialogueEnded;

		// Token: 0x04002217 RID: 8727
		public DialogueNodeEvent[] NodeEvents;
	}
}
