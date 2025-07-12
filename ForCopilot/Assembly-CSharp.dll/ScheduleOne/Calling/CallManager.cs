using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI.Phone;
using UnityEngine;

namespace ScheduleOne.Calling
{
	// Token: 0x020007B6 RID: 1974
	public class CallManager : Singleton<CallManager>
	{
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060035B5 RID: 13749 RVA: 0x000E0EB3 File Offset: 0x000DF0B3
		// (set) Token: 0x060035B6 RID: 13750 RVA: 0x000E0EBB File Offset: 0x000DF0BB
		public PhoneCallData QueuedCallData { get; private set; }

		// Token: 0x060035B7 RID: 13751 RVA: 0x000E0EC4 File Offset: 0x000DF0C4
		protected override void Start()
		{
			base.Start();
			if (Singleton<CallInterface>.Instance == null)
			{
				Debug.LogError("CallInterface instance is null. CallManager cannot function without it.");
				return;
			}
			CallInterface instance = Singleton<CallInterface>.Instance;
			instance.CallCompleted = (Action<PhoneCallData>)Delegate.Combine(instance.CallCompleted, new Action<PhoneCallData>(this.CallCompleted));
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000E0F15 File Offset: 0x000DF115
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (Singleton<CallInterface>.Instance != null)
			{
				CallInterface instance = Singleton<CallInterface>.Instance;
				instance.CallCompleted = (Action<PhoneCallData>)Delegate.Remove(instance.CallCompleted, new Action<PhoneCallData>(this.CallCompleted));
			}
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000E0F50 File Offset: 0x000DF150
		public void QueueCall(PhoneCallData data)
		{
			this.QueuedCallData = data;
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000E0F59 File Offset: 0x000DF159
		public void ClearQueuedCall()
		{
			this.QueuedCallData = null;
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000E0F62 File Offset: 0x000DF162
		private void CallCompleted(PhoneCallData call)
		{
			if (call == this.QueuedCallData)
			{
				this.ClearQueuedCall();
			}
		}
	}
}
