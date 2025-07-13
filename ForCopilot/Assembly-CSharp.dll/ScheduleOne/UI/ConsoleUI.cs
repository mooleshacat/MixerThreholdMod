using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ScheduleOne.UI
{
	// Token: 0x02000A0B RID: 2571
	public class ConsoleUI : MonoBehaviour
	{
		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06004518 RID: 17688 RVA: 0x00121E45 File Offset: 0x00120045
		public bool IS_CONSOLE_ENABLED
		{
			get
			{
				return (NetworkSingleton<GameManager>.Instance.Settings.ConsoleEnabled && InstanceFinder.IsServer) || Application.isEditor;
			}
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x00121E68 File Offset: 0x00120068
		private void Awake()
		{
			this.InputField.onSubmit.AddListener(new UnityAction<string>(this.Submit));
			this.Container.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x00121EB4 File Offset: 0x001200B4
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.BackQuote) && !Singleton<PauseMenu>.Instance.IsPaused && this.IS_CONSOLE_ENABLED)
			{
				this.SetIsOpen(!this.canvas.enabled);
			}
			if (!this.canvas.enabled)
			{
				return;
			}
			if (!Player.Local.Health.IsAlive)
			{
				this.SetIsOpen(false);
			}
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x00121F18 File Offset: 0x00120118
		private void Exit(ExitAction exitAction)
		{
			if (this.canvas == null)
			{
				return;
			}
			if (!this.canvas.enabled)
			{
				return;
			}
			if (exitAction.Used)
			{
				return;
			}
			if (exitAction.exitType == ExitType.Escape)
			{
				exitAction.Used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x00121F58 File Offset: 0x00120158
		public void SetIsOpen(bool open)
		{
			if (!InstanceFinder.IsHost && InstanceFinder.NetworkManager != null && !Application.isEditor && !Debug.isDebugBuild)
			{
				return;
			}
			this.canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.InputField.SetTextWithoutNotify("");
			if (open)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				GameInput.IsTyping = true;
				base.StartCoroutine(this.<SetIsOpen>g__Routine|8_0());
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			GameInput.IsTyping = false;
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x00121FF2 File Offset: 0x001201F2
		public void Submit(string val)
		{
			if (!this.canvas.enabled)
			{
				return;
			}
			Console.SubmitCommand(val);
			this.SetIsOpen(false);
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x0012200F File Offset: 0x0012020F
		[CompilerGenerated]
		private IEnumerator <SetIsOpen>g__Routine|8_0()
		{
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(this.InputField.gameObject);
			yield break;
		}

		// Token: 0x040031ED RID: 12781
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040031EE RID: 12782
		public TMP_InputField InputField;

		// Token: 0x040031EF RID: 12783
		public GameObject Container;
	}
}
