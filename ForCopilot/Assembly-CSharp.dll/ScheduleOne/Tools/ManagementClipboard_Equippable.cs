using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Misc;
using ScheduleOne.UI;
using ScheduleOne.UI.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x0200087C RID: 2172
	public class ManagementClipboard_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06003B83 RID: 15235 RVA: 0x000FC0A8 File Offset: 0x000FA2A8
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			Singleton<ManagementWorldspaceCanvas>.Instance.Open();
			this.Clipboard.transform.position = this.LoweredPosition.position;
			this.OverrideText.gameObject.SetActive(false);
			this.SelectionInfo.gameObject.SetActive(true);
			Singleton<ManagementClipboard>.Instance.IsEquipped = true;
			Singleton<ManagementClipboard>.Instance.onOpened.AddListener(new UnityAction(this.FullscreenEnter));
			Singleton<ManagementClipboard>.Instance.onClosed.AddListener(new UnityAction(this.FullscreenExit));
			Singleton<InputPromptsCanvas>.Instance.LoadModule("clipboard");
			if (Singleton<ManagementClipboard>.Instance.onClipboardEquipped != null)
			{
				Singleton<ManagementClipboard>.Instance.onClipboardEquipped.Invoke();
			}
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x000FC170 File Offset: 0x000FA370
		public override void Unequip()
		{
			base.Unequip();
			if (Singleton<ManagementClipboard>.Instance.IsOpen)
			{
				Singleton<ManagementClipboard>.Instance.Close(false);
			}
			Singleton<ManagementWorldspaceCanvas>.Instance.Close(false);
			Singleton<ManagementClipboard>.Instance.IsEquipped = false;
			if (Singleton<ManagementClipboard>.Instance.onClipboardUnequipped != null)
			{
				Singleton<ManagementClipboard>.Instance.onClipboardUnequipped.Invoke();
			}
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "clipboard")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x000FC1EC File Offset: 0x000FA3EC
		protected override void Update()
		{
			base.Update();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact) && !GameInput.IsTyping && Singleton<InteractionManager>.Instance.hoveredValidInteractableObject == null)
			{
				if (Singleton<ManagementClipboard>.Instance.IsOpen)
				{
					Singleton<ManagementClipboard>.Instance.Close(false);
					return;
				}
				List<IConfigurable> list = new List<IConfigurable>();
				list.AddRange(Singleton<ManagementWorldspaceCanvas>.Instance.SelectedConfigurables);
				if (Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable != null && !list.Contains(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable))
				{
					list.Add(Singleton<ManagementWorldspaceCanvas>.Instance.HoveredConfigurable);
				}
				Singleton<ManagementClipboard>.Instance.Open(list, this);
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x000FC28C File Offset: 0x000FA48C
		private void FullscreenEnter()
		{
			Singleton<ManagementWorldspaceCanvas>.Instance.Close(true);
			this.Clipboard.gameObject.SetActive(false);
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "clipboard")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x000FC2CC File Offset: 0x000FA4CC
		private void FullscreenExit()
		{
			this.Clipboard.gameObject.SetActive(true);
			if (!Singleton<ManagementClipboard>.Instance.IsOpen && !Singleton<ManagementClipboard>.Instance.StatePreserved)
			{
				Singleton<ManagementWorldspaceCanvas>.Instance.Open();
				Singleton<InputPromptsCanvas>.Instance.LoadModule("clipboard");
			}
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x000FC31B File Offset: 0x000FA51B
		public void OverrideClipboardText(string overriddenText)
		{
			this.OverrideText.text = overriddenText;
			this.OverrideText.gameObject.SetActive(true);
			this.SelectionInfo.gameObject.SetActive(false);
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x000FC34B File Offset: 0x000FA54B
		public void EndOverride()
		{
			this.OverrideText.gameObject.SetActive(false);
			this.SelectionInfo.gameObject.SetActive(true);
		}

		// Token: 0x04002A80 RID: 10880
		[Header("References")]
		public Transform Clipboard;

		// Token: 0x04002A81 RID: 10881
		public Transform LoweredPosition;

		// Token: 0x04002A82 RID: 10882
		public Transform RaisedPosition;

		// Token: 0x04002A83 RID: 10883
		public ToggleableLight Light;

		// Token: 0x04002A84 RID: 10884
		public SelectionInfoUI SelectionInfo;

		// Token: 0x04002A85 RID: 10885
		public TextMeshProUGUI OverrideText;

		// Token: 0x04002A86 RID: 10886
		private Coroutine moveRoutine;
	}
}
