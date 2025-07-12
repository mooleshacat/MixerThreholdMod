using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.Management;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000879 RID: 2169
	public class ManagementClipboard : Singleton<ManagementClipboard>
	{
		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003B6E RID: 15214 RVA: 0x000FBBDD File Offset: 0x000F9DDD
		// (set) Token: 0x06003B6F RID: 15215 RVA: 0x000FBBE5 File Offset: 0x000F9DE5
		public bool IsOpen { get; protected set; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003B70 RID: 15216 RVA: 0x000FBBEE File Offset: 0x000F9DEE
		// (set) Token: 0x06003B71 RID: 15217 RVA: 0x000FBBF6 File Offset: 0x000F9DF6
		public bool StatePreserved { get; protected set; }

		// Token: 0x06003B72 RID: 15218 RVA: 0x000FBC00 File Offset: 0x000F9E00
		protected override void Awake()
		{
			base.Awake();
			this.ClipboardTransform.gameObject.SetActive(false);
			this.ClipboardTransform.localPosition = new Vector3(this.ClipboardTransform.localPosition.x, this.ClosedOffset, this.ClipboardTransform.localPosition.z);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 10);
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x000FBC70 File Offset: 0x000F9E70
		private void Update()
		{
			for (int i = 0; i < this.CurrentConfigurables.Count; i++)
			{
				if (this.CurrentConfigurables[i].IsBeingConfiguredByOtherPlayer)
				{
					this.Close(false);
				}
			}
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x000FBCAD File Offset: 0x000F9EAD
		private void Exit(ExitAction exitAction)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exitAction.Used)
			{
				return;
			}
			this.Close(false);
			exitAction.Used = true;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x000FBCD0 File Offset: 0x000F9ED0
		public void Open(List<IConfigurable> selection, ManagementClipboard_Equippable equippable)
		{
			this.IsOpen = true;
			this.OverlayCamera.enabled = true;
			this.OverlayLight.enabled = true;
			this.ClipboardTransform.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.06f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.06f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			this.SelectionInfo.Set(selection);
			this.LerpToVerticalPosition(true, null);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Singleton<ManagementInterface>.Instance.Open(selection, equippable);
			this.CurrentConfigurables.AddRange(selection);
			for (int i = 0; i < this.CurrentConfigurables.Count; i++)
			{
				this.CurrentConfigurables[i].SetConfigurer(Player.Local.NetworkObject);
			}
			if (this.onOpened != null)
			{
				this.onOpened.Invoke();
			}
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x000FBDD0 File Offset: 0x000F9FD0
		public void Close(bool preserveState = false)
		{
			this.IsOpen = false;
			this.StatePreserved = preserveState;
			this.OverlayLight.enabled = false;
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.06f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			Singleton<ManagementInterface>.Instance.Close(preserveState);
			if (this.onClosed != null)
			{
				this.onClosed.Invoke();
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			for (int i = 0; i < this.CurrentConfigurables.Count; i++)
			{
				if (this.CurrentConfigurables[i].CurrentPlayerConfigurer == Player.Local.NetworkObject)
				{
					this.CurrentConfigurables[i].SetConfigurer(null);
				}
			}
			this.CurrentConfigurables.Clear();
			this.LerpToVerticalPosition(false, delegate
			{
				this.<Close>g__Done|25_1();
			});
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x000FBEC0 File Offset: 0x000FA0C0
		private void LerpToVerticalPosition(bool open, Action callback)
		{
			ManagementClipboard.<>c__DisplayClass26_0 CS$<>8__locals1 = new ManagementClipboard.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.callback = callback;
			CS$<>8__locals1.endPos = new Vector3(this.ClipboardTransform.localPosition.x, open ? 0f : this.ClosedOffset, this.ClipboardTransform.localPosition.z);
			CS$<>8__locals1.startPos = this.ClipboardTransform.localPosition;
			if (this.lerpRoutine != null)
			{
				base.StopCoroutine(this.lerpRoutine);
			}
			this.lerpRoutine = base.StartCoroutine(CS$<>8__locals1.<LerpToVerticalPosition>g__Lerp|0());
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x000FBF79 File Offset: 0x000FA179
		[CompilerGenerated]
		private void <Close>g__Done|25_1()
		{
			if (!Singleton<GameplayMenu>.Instance.IsOpen)
			{
				this.ClipboardTransform.gameObject.SetActive(false);
				this.OverlayCamera.enabled = false;
			}
		}

		// Token: 0x04002A69 RID: 10857
		public bool IsEquipped;

		// Token: 0x04002A6C RID: 10860
		public const float OpenTime = 0.06f;

		// Token: 0x04002A6D RID: 10861
		[Header("References")]
		public Transform ClipboardTransform;

		// Token: 0x04002A6E RID: 10862
		public Camera OverlayCamera;

		// Token: 0x04002A6F RID: 10863
		public Light OverlayLight;

		// Token: 0x04002A70 RID: 10864
		public SelectionInfoUI SelectionInfo;

		// Token: 0x04002A71 RID: 10865
		[Header("Settings")]
		public float ClosedOffset = -0.2f;

		// Token: 0x04002A72 RID: 10866
		public UnityEvent onClipboardEquipped;

		// Token: 0x04002A73 RID: 10867
		public UnityEvent onClipboardUnequipped;

		// Token: 0x04002A74 RID: 10868
		public UnityEvent onOpened;

		// Token: 0x04002A75 RID: 10869
		public UnityEvent onClosed;

		// Token: 0x04002A76 RID: 10870
		private Coroutine lerpRoutine;

		// Token: 0x04002A77 RID: 10871
		private List<IConfigurable> CurrentConfigurables = new List<IConfigurable>();
	}
}
