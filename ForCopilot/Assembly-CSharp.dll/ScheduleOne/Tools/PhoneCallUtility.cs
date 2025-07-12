using System;
using ScheduleOne.Calling;
using ScheduleOne.DevUtilities;
using ScheduleOne.ScriptableObjects;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A7 RID: 2215
	public class PhoneCallUtility : MonoBehaviour
	{
		// Token: 0x06003C1E RID: 15390 RVA: 0x000FD767 File Offset: 0x000FB967
		public void PromptCall(PhoneCallData callData)
		{
			Singleton<CallManager>.Instance.QueueCall(callData);
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x000FD767 File Offset: 0x000FB967
		public void StartCall(PhoneCallData callData)
		{
			Singleton<CallManager>.Instance.QueueCall(callData);
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x000FD767 File Offset: 0x000FB967
		public void SetQueuedCall(PhoneCallData callData)
		{
			Singleton<CallManager>.Instance.QueueCall(callData);
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x000FD774 File Offset: 0x000FB974
		public void ClearCall()
		{
			Singleton<CallManager>.Instance.ClearQueuedCall();
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x000045B1 File Offset: 0x000027B1
		public void SetPhoneOpenable(bool openable)
		{
		}
	}
}
