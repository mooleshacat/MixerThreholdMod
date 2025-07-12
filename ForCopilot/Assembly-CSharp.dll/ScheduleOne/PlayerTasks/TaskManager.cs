using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000359 RID: 857
	public class TaskManager : Singleton<TaskManager>
	{
		// Token: 0x0600133E RID: 4926 RVA: 0x00053C3A File Offset: 0x00051E3A
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00053C54 File Offset: 0x00051E54
		protected virtual void Update()
		{
			if (this.currentTask != null)
			{
				this.currentTask.Update();
			}
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00053C69 File Offset: 0x00051E69
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (action.exitType == ExitType.Escape && this.currentTask != null)
			{
				action.Used = true;
				this.currentTask.Outcome = Task.EOutcome.Cancelled;
				this.currentTask.StopTask();
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00053CA3 File Offset: 0x00051EA3
		protected virtual void LateUpdate()
		{
			if (this.currentTask != null)
			{
				this.currentTask.LateUpdate();
			}
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00053CB8 File Offset: 0x00051EB8
		protected virtual void FixedUpdate()
		{
			if (this.currentTask != null)
			{
				this.currentTask.FixedUpdate();
			}
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00053CCD File Offset: 0x00051ECD
		public void PlayTaskCompleteSound()
		{
			this.TaskCompleteSound.Play();
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00053CDA File Offset: 0x00051EDA
		public void StartTask(Task task)
		{
			this.currentTask = task;
			if (this.OnTaskStarted != null)
			{
				this.OnTaskStarted(task);
			}
		}

		// Token: 0x04001274 RID: 4724
		public Task currentTask;

		// Token: 0x04001275 RID: 4725
		public AudioSourceController TaskCompleteSound;

		// Token: 0x04001276 RID: 4726
		public Action<Task> OnTaskStarted;
	}
}
