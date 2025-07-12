using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A89 RID: 2697
	public class StorageMenu : Singleton<StorageMenu>
	{
		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x0600488A RID: 18570 RVA: 0x00130651 File Offset: 0x0012E851
		// (set) Token: 0x0600488B RID: 18571 RVA: 0x00130659 File Offset: 0x0012E859
		public bool IsOpen { get; protected set; }

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x0600488C RID: 18572 RVA: 0x00130662 File Offset: 0x0012E862
		// (set) Token: 0x0600488D RID: 18573 RVA: 0x0013066A File Offset: 0x0012E86A
		public StorageEntity OpenedStorageEntity { get; protected set; }

		// Token: 0x0600488E RID: 18574 RVA: 0x00130673 File Offset: 0x0012E873
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x001306AA File Offset: 0x0012E8AA
		public virtual void Open(IItemSlotOwner owner, string title, string subtitle)
		{
			this.IsOpen = true;
			this.OpenedStorageEntity = null;
			this.SlotGridLayout.constraintCount = 1;
			this.Open(title, subtitle, owner);
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x001306CF File Offset: 0x0012E8CF
		public virtual void Open(StorageEntity entity)
		{
			this.IsOpen = true;
			this.OpenedStorageEntity = entity;
			this.SlotGridLayout.constraintCount = entity.DisplayRowCount;
			this.Open(entity.StorageEntityName, entity.StorageEntitySubtitle, entity);
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x00130704 File Offset: 0x0012E904
		private void Open(string title, string subtitle, IItemSlotOwner owner)
		{
			this.IsOpen = true;
			this.TitleLabel.text = title;
			this.SubtitleLabel.text = subtitle;
			for (int i = 0; i < this.SlotsUIs.Length; i++)
			{
				if (owner.ItemSlots.Count > i)
				{
					this.SlotsUIs[i].gameObject.SetActive(true);
					this.SlotsUIs[i].AssignSlot(owner.ItemSlots[i]);
				}
				else
				{
					this.SlotsUIs[i].ClearSlot();
					this.SlotsUIs[i].gameObject.SetActive(false);
				}
			}
			int constraintCount = this.SlotGridLayout.constraintCount;
			this.CloseButton.anchoredPosition = new Vector2(0f, (float)constraintCount * -this.SlotGridLayout.cellSize.y - 60f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.06f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), owner.ItemSlots.ToList<ItemSlot>());
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x00130887 File Offset: 0x0012EA87
		public void Close()
		{
			if (this.OpenedStorageEntity != null)
			{
				this.OpenedStorageEntity.Close();
				return;
			}
			this.CloseMenu();
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x001308AC File Offset: 0x0012EAAC
		public virtual void CloseMenu()
		{
			this.IsOpen = false;
			this.OpenedStorageEntity = null;
			for (int i = 0; i < this.SlotsUIs.Length; i++)
			{
				this.SlotsUIs[i].ClearSlot();
				this.SlotsUIs[i].gameObject.SetActive(false);
			}
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.06f);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			if (this.onClosed != null)
			{
				this.onClosed.Invoke();
			}
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x0013098C File Offset: 0x0012EB8C
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
				if (this.OpenedStorageEntity != null)
				{
					this.OpenedStorageEntity.Close();
					return;
				}
				this.CloseMenu();
			}
		}

		// Token: 0x04003530 RID: 13616
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003531 RID: 13617
		public RectTransform Container;

		// Token: 0x04003532 RID: 13618
		public TextMeshProUGUI TitleLabel;

		// Token: 0x04003533 RID: 13619
		public TextMeshProUGUI SubtitleLabel;

		// Token: 0x04003534 RID: 13620
		public RectTransform SlotContainer;

		// Token: 0x04003535 RID: 13621
		public ItemSlotUI[] SlotsUIs;

		// Token: 0x04003536 RID: 13622
		public GridLayoutGroup SlotGridLayout;

		// Token: 0x04003537 RID: 13623
		public RectTransform CloseButton;

		// Token: 0x04003538 RID: 13624
		public UnityEvent onClosed;
	}
}
