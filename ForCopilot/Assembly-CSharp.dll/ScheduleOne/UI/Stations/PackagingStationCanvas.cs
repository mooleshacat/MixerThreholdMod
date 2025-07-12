using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000AB1 RID: 2737
	public class PackagingStationCanvas : Singleton<PackagingStationCanvas>
	{
		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06004998 RID: 18840 RVA: 0x00135B13 File Offset: 0x00133D13
		// (set) Token: 0x06004999 RID: 18841 RVA: 0x00135B1B File Offset: 0x00133D1B
		public bool isOpen { get; protected set; }

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x0600499A RID: 18842 RVA: 0x00135B24 File Offset: 0x00133D24
		// (set) Token: 0x0600499B RID: 18843 RVA: 0x00135B2C File Offset: 0x00133D2C
		public PackagingStation PackagingStation { get; protected set; }

		// Token: 0x0600499C RID: 18844 RVA: 0x00135B35 File Offset: 0x00133D35
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00135B59 File Offset: 0x00133D59
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00135B6C File Offset: 0x00133D6C
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.CurrentMode == PackagingStation.EMode.Package)
				{
					this.ButtonLabel.text = "PACK";
				}
				else
				{
					this.ButtonLabel.text = "UNPACK";
				}
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				PackagingStation.EState state = this.PackagingStation.GetState(this.CurrentMode);
				if (state == PackagingStation.EState.CanBegin)
				{
					this.InstructionLabel.enabled = false;
					this.InstructionShadow.enabled = false;
					this.BeginButton.interactable = true;
					return;
				}
				if (this.CurrentMode == PackagingStation.EMode.Package)
				{
					if (state == PackagingStation.EState.MissingItems)
					{
						this.InstructionLabel.text = "Drag product + packaging into slots";
						this.InstructionLabel.color = Color.white;
					}
					else if (state == PackagingStation.EState.InsufficentProduct)
					{
						this.InstructionLabel.text = "This packaging type requires <color=#FFC73D>" + (this.PackagingStation.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity.ToString() + "x</color> product";
						this.InstructionLabel.color = Color.white;
					}
					else if (state == PackagingStation.EState.OutputSlotFull)
					{
						this.InstructionLabel.text = "Output slot is full!";
						this.InstructionLabel.color = this.InstructionWarningColor;
					}
					else if (state == PackagingStation.EState.Mismatch)
					{
						this.InstructionLabel.text = "Output slot is full!";
						this.InstructionLabel.color = this.InstructionWarningColor;
					}
				}
				else if (state == PackagingStation.EState.MissingItems)
				{
					this.InstructionLabel.text = "Drag packaged product into output";
					this.InstructionLabel.color = Color.white;
				}
				else if (state == PackagingStation.EState.PackageSlotFull)
				{
					this.InstructionLabel.text = "Unpackaged items won't fit!";
					this.InstructionLabel.color = this.InstructionWarningColor;
				}
				else if (state == PackagingStation.EState.ProductSlotFull)
				{
					this.InstructionLabel.text = "Unpackaged items won't fit!";
					this.InstructionLabel.color = this.InstructionWarningColor;
				}
				this.InstructionLabel.enabled = true;
				this.InstructionShadow.enabled = true;
				this.BeginButton.interactable = false;
			}
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x00135D7C File Offset: 0x00133F7C
		public void SetIsOpen(PackagingStation station, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.PackagingStation = station;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (station != null)
			{
				this.PackagingSlotUI.AssignSlot(station.PackagingSlot);
				this.ProductSlotUI.AssignSlot(station.ProductSlot);
				this.OutputSlotUI.AssignSlot(station.OutputSlot);
			}
			else
			{
				this.PackagingSlotUI.ClearSlot();
				this.ProductSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				if (this.ShowShiftClickHint && station.OutputSlot.Quantity > 0)
				{
					Singleton<HintDisplay>.Instance.ShowHint_20s("<Input_QuickMove><h1> + click</h> an item to quickly move it");
				}
			}
			else if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				if (this.CurrentMode == PackagingStation.EMode.Package)
				{
					Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
					{
						station.ProductSlot,
						station.PackagingSlot,
						station.OutputSlot
					});
				}
				else
				{
					Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
					{
						station.OutputSlot,
						station.PackagingSlot,
						station.ProductSlot
					});
				}
			}
			if (this.isOpen)
			{
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00135F44 File Offset: 0x00134144
		public void BeginButtonPressed()
		{
			if (this.PackagingStation == null)
			{
				return;
			}
			if (this.PackagingStation.GetState(this.CurrentMode) != PackagingStation.EState.CanBegin)
			{
				return;
			}
			if (this.CurrentMode == PackagingStation.EMode.Unpackage)
			{
				this.PackagingStation.Unpack();
				Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.PackagingStation.StartTask();
			if (this.ShowHintOnOpen)
			{
				Singleton<HintDisplay>.Instance.ShowHint_20s("When performing tasks at stations, click and drag items to move them.");
			}
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00135FCC File Offset: 0x001341CC
		private void UpdateSlotPositions()
		{
			if (this.PackagingStation != null)
			{
				this.PackagingSlotUI.Rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.PackagingStation.PackagingSlotPosition.position);
				this.ProductSlotUI.Rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.PackagingStation.ProductSlotPosition.position);
				this.OutputSlotUI.Rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.PackagingStation.OutputSlotPosition.position);
			}
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00136077 File Offset: 0x00134277
		public void ToggleMode()
		{
			this.SetMode((this.CurrentMode == PackagingStation.EMode.Package) ? PackagingStation.EMode.Unpackage : PackagingStation.EMode.Package);
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x0013608C File Offset: 0x0013428C
		public void SetMode(PackagingStation.EMode mode)
		{
			this.CurrentMode = mode;
			if (mode == PackagingStation.EMode.Package)
			{
				this.ModeAnimation.Play("Packaging station switch to package");
			}
			else
			{
				this.ModeAnimation.Play("Packaging station switch to unpackage");
			}
			if (this.CurrentMode == PackagingStation.EMode.Package)
			{
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
				{
					this.PackagingStation.ProductSlot,
					this.PackagingStation.PackagingSlot,
					this.PackagingStation.OutputSlot
				});
				return;
			}
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
			{
				this.PackagingStation.OutputSlot,
				this.PackagingStation.PackagingSlot,
				this.PackagingStation.ProductSlot
			});
		}

		// Token: 0x04003636 RID: 13878
		public bool ShowHintOnOpen;

		// Token: 0x04003637 RID: 13879
		public bool ShowShiftClickHint;

		// Token: 0x04003638 RID: 13880
		public PackagingStation.EMode CurrentMode;

		// Token: 0x04003639 RID: 13881
		public Color InstructionWarningColor;

		// Token: 0x0400363A RID: 13882
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400363B RID: 13883
		public GameObject Container;

		// Token: 0x0400363C RID: 13884
		public ItemSlotUI PackagingSlotUI;

		// Token: 0x0400363D RID: 13885
		public ItemSlotUI ProductSlotUI;

		// Token: 0x0400363E RID: 13886
		public ItemSlotUI OutputSlotUI;

		// Token: 0x0400363F RID: 13887
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x04003640 RID: 13888
		public Image InstructionShadow;

		// Token: 0x04003641 RID: 13889
		public Button BeginButton;

		// Token: 0x04003642 RID: 13890
		public Animation ModeAnimation;

		// Token: 0x04003643 RID: 13891
		public TextMeshProUGUI ButtonLabel;
	}
}
