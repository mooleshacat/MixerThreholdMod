using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using ScheduleOne.UI.Stations.Drying_rack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000AAD RID: 2733
	public class DryingRackCanvas : Singleton<DryingRackCanvas>
	{
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x0600495F RID: 18783 RVA: 0x001341D1 File Offset: 0x001323D1
		// (set) Token: 0x06004960 RID: 18784 RVA: 0x001341D9 File Offset: 0x001323D9
		public bool isOpen { get; protected set; }

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06004961 RID: 18785 RVA: 0x001341E2 File Offset: 0x001323E2
		// (set) Token: 0x06004962 RID: 18786 RVA: 0x001341EA File Offset: 0x001323EA
		public DryingRack Rack { get; protected set; }

		// Token: 0x06004963 RID: 18787 RVA: 0x001341F3 File Offset: 0x001323F3
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x001341FB File Offset: 0x001323FB
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x00134231 File Offset: 0x00132431
		private void MinPass()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.UpdateDryingOperations();
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00134242 File Offset: 0x00132442
		protected virtual void Update()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.UpdateUI();
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x00134254 File Offset: 0x00132454
		private void UpdateUI()
		{
			this.InsertButton.interactable = this.Rack.CanStartOperation();
			this.CapacityLabel.text = this.Rack.GetTotalDryingItems().ToString() + " / " + this.Rack.ItemCapacity.ToString();
			this.CapacityLabel.color = ((this.Rack.GetTotalDryingItems() >= this.Rack.ItemCapacity) ? new Color32(byte.MaxValue, 50, 50, byte.MaxValue) : Color.white);
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x001342F4 File Offset: 0x001324F4
		private void UpdateDryingOperations()
		{
			foreach (DryingOperationUI dryingOperationUI in this.operationUIs)
			{
				RectTransform alignment = null;
				DryingOperation assignedOperation = dryingOperationUI.AssignedOperation;
				if (assignedOperation.StartQuality == EQuality.Trash)
				{
					alignment = this.IndicatorAlignments[0];
				}
				else if (assignedOperation.StartQuality == EQuality.Poor)
				{
					alignment = this.IndicatorAlignments[1];
				}
				else if (assignedOperation.StartQuality == EQuality.Standard)
				{
					alignment = this.IndicatorAlignments[2];
				}
				else if (assignedOperation.StartQuality == EQuality.Premium)
				{
					alignment = this.IndicatorAlignments[3];
				}
				else
				{
					Console.LogWarning("Alignment not found for quality: " + assignedOperation.StartQuality.ToString(), null);
				}
				dryingOperationUI.SetAlignment(alignment);
			}
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x001343C4 File Offset: 0x001325C4
		private void UpdateQuantities()
		{
			foreach (DryingOperationUI dryingOperationUI in this.operationUIs)
			{
				dryingOperationUI.RefreshQuantity();
			}
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x00134414 File Offset: 0x00132614
		public void SetIsOpen(DryingRack rack, bool open)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			}
			if (open)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				this.InputSlotUI.AssignSlot(rack.InputSlot);
				this.OutputSlotUI.AssignSlot(rack.OutputSlot);
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				for (int i = 0; i < rack.DryingOperations.Count; i++)
				{
					this.CreateOperationUI(rack.DryingOperations[i]);
				}
				rack.onOperationStart = (Action<DryingOperation>)Delegate.Combine(rack.onOperationStart, new Action<DryingOperation>(this.CreateOperationUI));
				rack.onOperationComplete = (Action<DryingOperation>)Delegate.Combine(rack.onOperationComplete, new Action<DryingOperation>(this.DestroyOperationUI));
				rack.onOperationsChanged = (Action)Delegate.Combine(rack.onOperationsChanged, new Action(this.UpdateQuantities));
			}
			else
			{
				this.InputSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				if (this.Rack != null)
				{
					DryingRack rack2 = this.Rack;
					rack2.onOperationStart = (Action<DryingOperation>)Delegate.Remove(rack2.onOperationStart, new Action<DryingOperation>(this.CreateOperationUI));
					DryingRack rack3 = this.Rack;
					rack3.onOperationComplete = (Action<DryingOperation>)Delegate.Remove(rack3.onOperationComplete, new Action<DryingOperation>(this.DestroyOperationUI));
					DryingRack rack4 = this.Rack;
					rack4.onOperationsChanged = (Action)Delegate.Remove(rack4.onOperationsChanged, new Action(this.UpdateQuantities));
				}
				foreach (DryingOperationUI dryingOperationUI in this.operationUIs)
				{
					UnityEngine.Object.Destroy(dryingOperationUI.gameObject);
				}
				this.operationUIs.Clear();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				List<ItemSlot> list = new List<ItemSlot>();
				list.AddRange(rack.InputSlots);
				list.Add(rack.OutputSlot);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			}
			this.Rack = rack;
			if (open)
			{
				this.UpdateUI();
				this.MinPass();
			}
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x0013469C File Offset: 0x0013289C
		private void CreateOperationUI(DryingOperation operation)
		{
			DryingOperationUI dryingOperationUI = UnityEngine.Object.Instantiate<DryingOperationUI>(this.IndicatorPrefab, this.IndicatorContainer);
			dryingOperationUI.SetOperation(operation);
			this.operationUIs.Add(dryingOperationUI);
			this.UpdateDryingOperations();
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x001346D4 File Offset: 0x001328D4
		private void DestroyOperationUI(DryingOperation operation)
		{
			DryingOperationUI dryingOperationUI = this.operationUIs.FirstOrDefault((DryingOperationUI x) => x.AssignedOperation == operation);
			if (dryingOperationUI != null)
			{
				this.operationUIs.Remove(dryingOperationUI);
				UnityEngine.Object.Destroy(dryingOperationUI.gameObject);
			}
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x00134727 File Offset: 0x00132927
		public void Insert()
		{
			this.Rack.StartOperation();
		}

		// Token: 0x04003604 RID: 13828
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003605 RID: 13829
		public RectTransform Container;

		// Token: 0x04003606 RID: 13830
		public ItemSlotUI InputSlotUI;

		// Token: 0x04003607 RID: 13831
		public ItemSlotUI OutputSlotUI;

		// Token: 0x04003608 RID: 13832
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x04003609 RID: 13833
		public TextMeshProUGUI CapacityLabel;

		// Token: 0x0400360A RID: 13834
		public Button InsertButton;

		// Token: 0x0400360B RID: 13835
		public RectTransform IndicatorContainer;

		// Token: 0x0400360C RID: 13836
		public RectTransform[] IndicatorAlignments;

		// Token: 0x0400360D RID: 13837
		[Header("Prefabs")]
		public DryingOperationUI IndicatorPrefab;

		// Token: 0x0400360E RID: 13838
		private List<DryingOperationUI> operationUIs = new List<DryingOperationUI>();
	}
}
