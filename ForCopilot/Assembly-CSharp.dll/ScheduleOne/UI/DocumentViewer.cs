using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A1D RID: 2589
	public class DocumentViewer : Singleton<DocumentViewer>
	{
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060045B2 RID: 17842 RVA: 0x00124CFF File Offset: 0x00122EFF
		// (set) Token: 0x060045B3 RID: 17843 RVA: 0x00124D07 File Offset: 0x00122F07
		public bool IsOpen { get; protected set; }

		// Token: 0x060045B4 RID: 17844 RVA: 0x00124D10 File Offset: 0x00122F10
		protected override void Start()
		{
			base.Start();
			this.IsOpen = false;
			this.Canvas.enabled = false;
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 15);
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00124D3E File Offset: 0x00122F3E
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x00124D68 File Offset: 0x00122F68
		public void Open(string documentName)
		{
			this.IsOpen = true;
			for (int i = 0; i < this.Documents.Length; i++)
			{
				this.Documents[i].gameObject.SetActive(this.Documents[i].name == documentName);
			}
			this.Canvas.enabled = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			Singleton<HUD>.Instance.canvas.enabled = false;
			if (this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x00124E40 File Offset: 0x00123040
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Singleton<HUD>.Instance.canvas.enabled = true;
		}

		// Token: 0x04003276 RID: 12918
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003277 RID: 12919
		public RectTransform[] Documents;

		// Token: 0x04003278 RID: 12920
		public UnityEvent onOpen;
	}
}
