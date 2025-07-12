using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Phone;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A57 RID: 2647
	public abstract class App<T> : PlayerSingleton<T> where T : PlayerSingleton<T>
	{
		// Token: 0x06004726 RID: 18214 RVA: 0x0012AEC5 File Offset: 0x001290C5
		public static App<T> GetApp(int index)
		{
			if (index < 0 || index >= App<T>.Apps.Count)
			{
				return null;
			}
			return App<T>.Apps[index];
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06004727 RID: 18215 RVA: 0x0012AEE5 File Offset: 0x001290E5
		// (set) Token: 0x06004728 RID: 18216 RVA: 0x0012AEED File Offset: 0x001290ED
		public bool isOpen { get; protected set; }

		// Token: 0x06004729 RID: 18217 RVA: 0x0012AEF8 File Offset: 0x001290F8
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				return;
			}
			if (!this.AvailableInTutorial && NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.appContainer.gameObject.SetActive(false);
				return;
			}
			this.GenerateHomeScreenIcon();
			App<T>.Apps.Add(this);
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0012AF48 File Offset: 0x00129148
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			Phone instance = PlayerSingleton<Phone>.Instance;
			instance.closeApps = (Action)Delegate.Combine(instance.closeApps, new Action(this.Close));
			Phone instance2 = PlayerSingleton<Phone>.Instance;
			instance2.onPhoneOpened = (Action)Delegate.Combine(instance2.onPhoneOpened, new Action(this.OnPhoneOpened));
			this.SetOpen(false);
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x0012AFC2 File Offset: 0x001291C2
		private void Close()
		{
			if (this.isOpen)
			{
				this.SetOpen(false);
			}
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x0012AFD3 File Offset: 0x001291D3
		protected virtual void Update()
		{
			if (this.isOpen && PlayerSingleton<Phone>.Instance.IsOpen && this.IsHoveringButton() && GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
			{
				this.SetOpen(false);
			}
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0012B000 File Offset: 0x00129200
		private bool IsHoveringButton()
		{
			RaycastHit raycastHit;
			return Physics.Raycast(Singleton<GameplayMenu>.Instance.OverlayCamera.ScreenPointToRay(Input.mousePosition), ref raycastHit, 2f, 1 << LayerMask.NameToLayer("Overlay")) && raycastHit.collider.gameObject.name == "Button";
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x0012B060 File Offset: 0x00129260
		private void GenerateHomeScreenIcon()
		{
			this.appIconButton = PlayerSingleton<HomeScreen>.Instance.GenerateAppIcon<T>(this);
			this.appIconButton.onClick.AddListener(new UnityAction(this.ShortcutClicked));
			this.notificationContainer = this.appIconButton.transform.Find("Notifications").GetComponent<RectTransform>();
			this.notificationText = this.notificationContainer.Find("Text").GetComponent<Text>();
			this.notificationContainer.gameObject.SetActive(false);
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0012B0E6 File Offset: 0x001292E6
		public void SetNotificationCount(int amount)
		{
			this.notificationText.text = amount.ToString();
			this.notificationContainer.gameObject.SetActive(amount > 0);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0012B10E File Offset: 0x0012930E
		protected virtual void OnPhoneOpened()
		{
			if (this.isOpen)
			{
				if (this.Orientation == App<T>.EOrientation.Horizontal)
				{
					PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(0.6f);
					return;
				}
				PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(1f);
			}
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0012B13F File Offset: 0x0012933F
		private void ShortcutClicked()
		{
			this.SetOpen(!this.isOpen);
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0012B150 File Offset: 0x00129350
		public virtual void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (this.isOpen && PlayerSingleton<Phone>.InstanceExists && PlayerSingleton<Phone>.Instance.IsOpen)
			{
				exit.Used = true;
				this.SetOpen(false);
			}
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x0012B184 File Offset: 0x00129384
		public virtual void SetOpen(bool open)
		{
			if (open && Phone.ActiveApp != null)
			{
				Console.LogWarning(Phone.ActiveApp.name + " is already open", null);
			}
			this.isOpen = open;
			PlayerSingleton<AppsCanvas>.Instance.SetIsOpen(open);
			PlayerSingleton<HomeScreen>.Instance.SetIsOpen(!open);
			if (this.isOpen)
			{
				if (this.Orientation == App<T>.EOrientation.Horizontal)
				{
					PlayerSingleton<Phone>.Instance.SetIsHorizontal(true);
					PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(0.6f);
				}
				else
				{
					PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(1f);
				}
				Phone.ActiveApp = base.gameObject;
			}
			else
			{
				if (Phone.ActiveApp == base.gameObject)
				{
					Phone.ActiveApp = null;
				}
				PlayerSingleton<Phone>.Instance.SetIsHorizontal(false);
				PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(1f);
				Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
			}
			this.appContainer.gameObject.SetActive(open);
		}

		// Token: 0x040033FA RID: 13306
		public static List<App<T>> Apps = new List<App<T>>();

		// Token: 0x040033FB RID: 13307
		[Header("Settings")]
		public string AppName;

		// Token: 0x040033FC RID: 13308
		public string IconLabel;

		// Token: 0x040033FD RID: 13309
		public Sprite AppIcon;

		// Token: 0x040033FE RID: 13310
		public App<T>.EOrientation Orientation;

		// Token: 0x040033FF RID: 13311
		public bool AvailableInTutorial = true;

		// Token: 0x04003400 RID: 13312
		[Header("References")]
		[SerializeField]
		protected RectTransform appContainer;

		// Token: 0x04003401 RID: 13313
		protected RectTransform notificationContainer;

		// Token: 0x04003402 RID: 13314
		protected Text notificationText;

		// Token: 0x04003404 RID: 13316
		protected Button appIconButton;

		// Token: 0x02000A58 RID: 2648
		public enum EOrientation
		{
			// Token: 0x04003406 RID: 13318
			Horizontal,
			// Token: 0x04003407 RID: 13319
			Vertical
		}
	}
}
