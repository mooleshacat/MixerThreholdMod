using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000AA5 RID: 2725
	public class BrickPressCanvas : Singleton<BrickPressCanvas>
	{
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06004928 RID: 18728 RVA: 0x00132EB1 File Offset: 0x001310B1
		// (set) Token: 0x06004929 RID: 18729 RVA: 0x00132EB9 File Offset: 0x001310B9
		public bool isOpen { get; protected set; }

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x0600492A RID: 18730 RVA: 0x00132EC2 File Offset: 0x001310C2
		// (set) Token: 0x0600492B RID: 18731 RVA: 0x00132ECA File Offset: 0x001310CA
		public BrickPress Press { get; protected set; }

		// Token: 0x0600492C RID: 18732 RVA: 0x00132ED3 File Offset: 0x001310D3
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x00132EF7 File Offset: 0x001310F7
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x00132F08 File Offset: 0x00131108
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				PackagingStation.EState state = this.Press.GetState();
				if (state == PackagingStation.EState.CanBegin)
				{
					this.InstructionLabel.enabled = false;
					this.BeginButton.interactable = true;
					return;
				}
				if (state == PackagingStation.EState.InsufficentProduct)
				{
					this.InstructionLabel.text = "Drag 20x product into input slots";
				}
				else if (state == PackagingStation.EState.OutputSlotFull)
				{
					this.InstructionLabel.text = "Output slot is full!";
				}
				else if (state == PackagingStation.EState.Mismatch)
				{
					this.InstructionLabel.text = "Output slot is full!";
				}
				this.InstructionLabel.enabled = true;
				this.BeginButton.interactable = false;
			}
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x00132FC0 File Offset: 0x001311C0
		public void SetIsOpen(BrickPress press, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.Press = press;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (press != null)
			{
				for (int i = 0; i < this.ProductSlotUIs.Length; i++)
				{
					this.ProductSlotUIs[i].AssignSlot(press.InputSlots[i]);
				}
				this.OutputSlotUI.AssignSlot(press.OutputSlot);
			}
			else
			{
				for (int j = 0; j < this.ProductSlotUIs.Length; j++)
				{
					this.ProductSlotUIs[j].ClearSlot();
				}
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				this.Update();
			}
			else if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				List<ItemSlot> list = new List<ItemSlot>();
				list.AddRange(press.InputSlots);
				list.Add(press.OutputSlot);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			}
			if (this.isOpen)
			{
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x00133128 File Offset: 0x00131328
		public void BeginButtonPressed()
		{
			if (this.Press.GetState() != PackagingStation.EState.CanBegin)
			{
				return;
			}
			ProductItemInstance product;
			if (!this.Press.HasSufficientProduct(out product))
			{
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			new UseBrickPress(this.Press, product);
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x040035D0 RID: 13776
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040035D1 RID: 13777
		public RectTransform Container;

		// Token: 0x040035D2 RID: 13778
		public ItemSlotUI[] ProductSlotUIs;

		// Token: 0x040035D3 RID: 13779
		public ItemSlotUI OutputSlotUI;

		// Token: 0x040035D4 RID: 13780
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x040035D5 RID: 13781
		public Button BeginButton;
	}
}
