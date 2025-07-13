using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Quests
{
	// Token: 0x0200030D RID: 781
	public class StateMachine : MonoBehaviour
	{
		// Token: 0x06001160 RID: 4448 RVA: 0x0004D11D File Offset: 0x0004B31D
		private void Start()
		{
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0004D13A File Offset: 0x0004B33A
		private void Update()
		{
			if (StateMachine.stateChanged)
			{
				Action onStateChange = StateMachine.OnStateChange;
				if (onStateChange != null)
				{
					onStateChange();
				}
				StateMachine.stateChanged = false;
			}
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0004D159 File Offset: 0x0004B359
		private void Clean()
		{
			Debug.Log("Clearing state change...");
			StateMachine.OnStateChange = null;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0004D16B File Offset: 0x0004B36B
		public static void ChangeState()
		{
			StateMachine.stateChanged = true;
		}

		// Token: 0x04001150 RID: 4432
		public static Action OnStateChange;

		// Token: 0x04001151 RID: 4433
		private static bool stateChanged;
	}
}
