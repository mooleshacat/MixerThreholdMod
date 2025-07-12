using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A8A RID: 2698
	public class TaskManagerUI : Singleton<TaskManagerUI>
	{
		// Token: 0x06004896 RID: 18582 RVA: 0x001309E3 File Offset: 0x0012EBE3
		protected virtual void Update()
		{
			this.UpdateInstructionLabel();
			this.canvas.enabled = (Singleton<TaskManager>.Instance.currentTask != null);
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x00130A03 File Offset: 0x0012EC03
		protected override void Start()
		{
			base.Start();
			TaskManager instance = Singleton<TaskManager>.Instance;
			instance.OnTaskStarted = (Action<Task>)Delegate.Combine(instance.OnTaskStarted, new Action<Task>(this.TaskStarted));
			this.multiGrabIndicator.gameObject.SetActive(false);
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x00130A44 File Offset: 0x0012EC44
		protected virtual void UpdateInstructionLabel()
		{
			if (Singleton<TaskManager>.Instance.currentTask != null && Singleton<TaskManager>.Instance.currentTask.CurrentInstruction != string.Empty)
			{
				this.textShown = true;
				Singleton<HUD>.Instance.ShowTopScreenText(Singleton<TaskManager>.Instance.currentTask.CurrentInstruction);
				return;
			}
			if (this.textShown)
			{
				this.textShown = false;
				Singleton<HUD>.Instance.HideTopScreenText();
			}
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00130AB4 File Offset: 0x0012ECB4
		private void TaskStarted(Task task)
		{
			bool value = NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("InputHintsTutorialDone");
			this.multiGrabIndicator.gameObject.SetActive(false);
			if (GameManager.IS_TUTORIAL && !value && !Application.isEditor)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("InputHintsTutorialDone", true.ToString(), true);
				this.inputPromptUI.Open();
			}
		}

		// Token: 0x04003539 RID: 13625
		private bool textShown;

		// Token: 0x0400353A RID: 13626
		public GenericUIScreen inputPromptUI;

		// Token: 0x0400353B RID: 13627
		public Canvas canvas;

		// Token: 0x0400353C RID: 13628
		public RectTransform multiGrabIndicator;

		// Token: 0x0400353D RID: 13629
		public GenericUIScreen PackagingStationMK2TutorialDone;
	}
}
