using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A90 RID: 2704
	public class TextInputScreen : Singleton<TextInputScreen>
	{
		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060048A8 RID: 18600 RVA: 0x00130E88 File Offset: 0x0012F088
		public bool IsOpen
		{
			get
			{
				return this.Canvas.enabled;
			}
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x00130E95 File Offset: 0x0012F095
		protected override void Awake()
		{
			base.Awake();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 2);
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x00130EAF File Offset: 0x0012F0AF
		public void Submit()
		{
			this.Close(true);
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00130EB8 File Offset: 0x0012F0B8
		public void Cancel()
		{
			this.Close(false);
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x00130EC1 File Offset: 0x0012F0C1
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Return))
			{
				this.Submit();
			}
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x00130EDB File Offset: 0x0012F0DB
		public void Exit(ExitAction action)
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
				this.Close(false);
			}
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x00130F08 File Offset: 0x0012F108
		public void Open(string header, string text, TextInputScreen.OnSubmit _onSubmit, int maxChars = 10000)
		{
			this.HeaderLabel.text = header;
			this.InputField.SetTextWithoutNotify(text);
			this.Canvas.enabled = true;
			this.InputField.characterLimit = maxChars;
			this.InputField.ActivateInputField();
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			this.onSubmit = _onSubmit;
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x00130F94 File Offset: 0x0012F194
		private void Close(bool submit)
		{
			this.Canvas.enabled = false;
			this.InputField.DeactivateInputField(false);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			if (submit)
			{
				string text = this.InputField.text;
				if (this.onSubmit != null)
				{
					this.onSubmit(text);
				}
			}
		}

		// Token: 0x04003552 RID: 13650
		public Canvas Canvas;

		// Token: 0x04003553 RID: 13651
		public TextMeshProUGUI HeaderLabel;

		// Token: 0x04003554 RID: 13652
		public TMP_InputField InputField;

		// Token: 0x04003555 RID: 13653
		private TextInputScreen.OnSubmit onSubmit;

		// Token: 0x02000A91 RID: 2705
		// (Invoke) Token: 0x060048B2 RID: 18610
		public delegate void OnSubmit(string text);
	}
}
