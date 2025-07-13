using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Compass;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.Modification;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A96 RID: 2710
	public class VehicleModMenu : Singleton<VehicleModMenu>
	{
		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060048C8 RID: 18632 RVA: 0x00131511 File Offset: 0x0012F711
		// (set) Token: 0x060048C9 RID: 18633 RVA: 0x00131519 File Offset: 0x0012F719
		public bool IsOpen { get; private set; }

		// Token: 0x060048CA RID: 18634 RVA: 0x00131522 File Offset: 0x0012F722
		protected override void Awake()
		{
			base.Awake();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x0013153C File Offset: 0x0012F73C
		protected override void Start()
		{
			base.Start();
			this.confirmText_Online.text = "Confirm (" + MoneyManager.FormatAmount(VehicleModMenu.repaintCost, false, true) + ")";
			for (int i = 0; i < Singleton<VehicleColors>.Instance.colorLibrary.Count; i++)
			{
				RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, this.buttonContainer).GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2((0.5f + (float)this.colorButtons.Count) * component.sizeDelta.x, component.anchoredPosition.y);
				component.Find("Image").GetComponent<Image>().color = Singleton<VehicleColors>.Instance.colorLibrary[i].UIColor;
				EVehicleColor c = Singleton<VehicleColors>.Instance.colorLibrary[i].color;
				this.colorButtons.Add(component);
				this.colorToButton.Add(c, component);
				component.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ColorClicked(c);
				});
			}
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x00131671 File Offset: 0x0012F871
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
			if (this.openCloseRoutine != null)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x001316A4 File Offset: 0x0012F8A4
		protected virtual void Update()
		{
			if (this.IsOpen)
			{
				this.UpdateConfirmButton();
			}
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x001316B4 File Offset: 0x0012F8B4
		public void Open(LandVehicle vehicle)
		{
			this.currentVehicle = vehicle;
			this.selectedColor = vehicle.OwnedColor;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.openCloseRoutine = base.StartCoroutine(this.<Open>g__Close|24_0());
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x00131701 File Offset: 0x0012F901
		public void Close()
		{
			if (this.currentVehicle != null)
			{
				this.currentVehicle.ApplyOwnedColor();
			}
			this.openCloseRoutine = base.StartCoroutine(this.<Close>g__Close|25_0());
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x0013172E File Offset: 0x0012F92E
		public void ColorClicked(EVehicleColor col)
		{
			this.selectedColor = col;
			this.currentVehicle.ApplyColor(col);
			this.RefreshSelectionIndicator();
			this.UpdateConfirmButton();
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x00131750 File Offset: 0x0012F950
		private void UpdateConfirmButton()
		{
			bool flag = NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= VehicleModMenu.repaintCost;
			this.confirmButton_Online.interactable = (flag && this.selectedColor != this.currentVehicle.OwnedColor);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0013179C File Offset: 0x0012F99C
		private void RefreshSelectionIndicator()
		{
			this.tempIndicator.position = this.colorToButton[this.selectedColor].position;
			this.permIndicator.position = this.colorToButton[this.currentVehicle.OwnedColor].position;
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x001317F0 File Offset: 0x0012F9F0
		public void ConfirmButtonClicked()
		{
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Vehicle repaint", -VehicleModMenu.repaintCost, 1f, string.Empty);
			NetworkSingleton<MoneyManager>.Instance.CashSound.Play();
			this.currentVehicle.SendOwnedColor(this.selectedColor);
			this.RefreshSelectionIndicator();
			if (this.onPaintPurchased != null)
			{
				this.onPaintPurchased.Invoke();
			}
			this.Close();
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0013188C File Offset: 0x0012FA8C
		[CompilerGenerated]
		private IEnumerator <Open>g__Close|24_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			this.IsOpen = true;
			this.canvas.enabled = true;
			this.currentVehicle.AlignTo(this.VehiclePosition, EParkingAlignment.RearToKerb, true);
			this.RefreshSelectionIndicator();
			this.UpdateConfirmButton();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x0013189B File Offset: 0x0012FA9B
		[CompilerGenerated]
		private IEnumerator <Close>g__Close|25_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.currentVehicle = null;
			this.IsOpen = false;
			this.canvas.enabled = false;
			Singleton<CompassManager>.Instance.SetVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x0400356C RID: 13676
		public static float repaintCost = 100f;

		// Token: 0x0400356E RID: 13678
		[Header("UI References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x0400356F RID: 13679
		[SerializeField]
		protected RectTransform buttonContainer;

		// Token: 0x04003570 RID: 13680
		[SerializeField]
		protected RectTransform tempIndicator;

		// Token: 0x04003571 RID: 13681
		[SerializeField]
		protected RectTransform permIndicator;

		// Token: 0x04003572 RID: 13682
		[SerializeField]
		protected Button confirmButton_Online;

		// Token: 0x04003573 RID: 13683
		[SerializeField]
		protected TextMeshProUGUI confirmText_Online;

		// Token: 0x04003574 RID: 13684
		[Header("References")]
		public Transform CameraPosition;

		// Token: 0x04003575 RID: 13685
		public Transform VehiclePosition;

		// Token: 0x04003576 RID: 13686
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject buttonPrefab;

		// Token: 0x04003577 RID: 13687
		public UnityEvent onPaintPurchased;

		// Token: 0x04003578 RID: 13688
		protected LandVehicle currentVehicle;

		// Token: 0x04003579 RID: 13689
		protected List<RectTransform> colorButtons = new List<RectTransform>();

		// Token: 0x0400357A RID: 13690
		protected Dictionary<EVehicleColor, RectTransform> colorToButton = new Dictionary<EVehicleColor, RectTransform>();

		// Token: 0x0400357B RID: 13691
		protected EVehicleColor selectedColor = EVehicleColor.White;

		// Token: 0x0400357C RID: 13692
		private Coroutine openCloseRoutine;
	}
}
