using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A79 RID: 2681
	public class PropertySelector : MonoBehaviour
	{
		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06004815 RID: 18453 RVA: 0x0012F0A9 File Offset: 0x0012D2A9
		public bool isOpen
		{
			get
			{
				return this.container.activeSelf;
			}
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x0012F0B6 File Offset: 0x0012D2B6
		protected virtual void Awake()
		{
			Property.onPropertyAcquired = (Property.PropertyChange)Delegate.Combine(Property.onPropertyAcquired, new Property.PropertyChange(this.PropertyAcquired));
			this.container.SetActive(false);
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x0012F0E4 File Offset: 0x0012D2E4
		protected virtual void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0012F0F9 File Offset: 0x0012D2F9
		public virtual void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (exit.exitType == ExitType.RightClick)
			{
				return;
			}
			if (this.container.activeSelf)
			{
				exit.Used = true;
				this.Close(true);
			}
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0012F128 File Offset: 0x0012D328
		public void OpenSelector(PropertySelector.PropertySelected p)
		{
			this.pCallback = p;
			this.container.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x000045B1 File Offset: 0x000027B1
		private void PropertyAcquired(Property p)
		{
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0012F18E File Offset: 0x0012D38E
		private void SelectProperty(Property p)
		{
			this.pCallback(p);
			this.Close(false);
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x0012F1A4 File Offset: 0x0012D3A4
		private void Close(bool reenableShit)
		{
			this.container.SetActive(false);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (reenableShit)
			{
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<HUD>.Instance.SetCrosshairVisible(true);
			}
		}

		// Token: 0x040034DD RID: 13533
		[Header("References")]
		[SerializeField]
		protected GameObject container;

		// Token: 0x040034DE RID: 13534
		[SerializeField]
		protected RectTransform buttonContainer;

		// Token: 0x040034DF RID: 13535
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject buttonPrefab;

		// Token: 0x040034E0 RID: 13536
		private PropertySelector.PropertySelected pCallback;

		// Token: 0x02000A7A RID: 2682
		// (Invoke) Token: 0x0600481F RID: 18463
		public delegate void PropertySelected(Property p);
	}
}
