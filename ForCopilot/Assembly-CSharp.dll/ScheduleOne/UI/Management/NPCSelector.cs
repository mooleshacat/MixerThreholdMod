using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B42 RID: 2882
	public class NPCSelector : MonoBehaviour
	{
		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x001439A0 File Offset: 0x00141BA0
		// (set) Token: 0x06004CD1 RID: 19665 RVA: 0x001439A8 File Offset: 0x00141BA8
		public bool IsOpen { get; protected set; }

		// Token: 0x06004CD2 RID: 19666 RVA: 0x001439B1 File Offset: 0x00141BB1
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 12);
			Singleton<ManagementClipboard>.Instance.onClipboardUnequipped.AddListener(new UnityAction(this.ClipboardClosed));
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x001439E4 File Offset: 0x00141BE4
		public virtual void Open(string selectionTitle, Type typeRequirement, Action<NPC> _callback)
		{
			this.IsOpen = true;
			this.TypeRequirement = typeRequirement;
			this.callback = _callback;
			Singleton<HUD>.Instance.ShowTopScreenText(selectionTitle);
			Singleton<ManagementInterface>.Instance.EquippedClipboard.OverrideClipboardText(selectionTitle);
			Singleton<ManagementClipboard>.Instance.Close(true);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("npcselector");
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x00143A3C File Offset: 0x00141C3C
		public virtual void Close(bool returnToClipboard)
		{
			this.IsOpen = false;
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "npcselector")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			if (this.highlightedNPC != null)
			{
				this.highlightedNPC.HideOutline();
				this.highlightedNPC = null;
			}
			if (returnToClipboard)
			{
				Singleton<ManagementInterface>.Instance.EquippedClipboard.EndOverride();
				Singleton<ManagementClipboard>.Instance.Open(Singleton<ManagementInterface>.Instance.Configurables, Singleton<ManagementInterface>.Instance.EquippedClipboard);
			}
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x00143ACC File Offset: 0x00141CCC
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.hoveredNPC = this.GetHoveredNPC();
			if (this.hoveredNPC != null && this.IsNPCTypeValid(this.hoveredNPC))
			{
				if (this.hoveredNPC != this.highlightedNPC)
				{
					if (this.highlightedNPC != null)
					{
						this.highlightedNPC.HideOutline();
						this.highlightedNPC = null;
					}
					this.highlightedNPC = this.hoveredNPC;
					this.highlightedNPC.ShowOutline(this.HoverOutlineColor);
				}
			}
			else if (this.highlightedNPC != null)
			{
				this.highlightedNPC.HideOutline();
				this.highlightedNPC = null;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.hoveredNPC != null && this.IsNPCTypeValid(this.hoveredNPC))
			{
				this.NPCClicked(this.hoveredNPC);
			}
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x00143BAC File Offset: 0x00141DAC
		private NPC GetHoveredNPC()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, this.DetectionMask, false, 0.1f))
			{
				return raycastHit.collider.GetComponentInParent<NPC>();
			}
			return null;
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x00143BE6 File Offset: 0x00141DE6
		public bool IsNPCTypeValid(NPC npc)
		{
			return this.TypeRequirement == null || this.TypeRequirement.IsAssignableFrom(npc.GetType());
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x00143C09 File Offset: 0x00141E09
		public void NPCClicked(NPC npc)
		{
			if (!this.IsNPCTypeValid(npc))
			{
				return;
			}
			Action<NPC> action = this.callback;
			if (action != null)
			{
				action(this.hoveredNPC);
			}
			this.Close(true);
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x00143C33 File Offset: 0x00141E33
		private void ClipboardClosed()
		{
			this.Close(false);
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x00143C3C File Offset: 0x00141E3C
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
			if (exitAction.exitType == ExitType.Escape)
			{
				exitAction.Used = true;
				this.Close(true);
			}
		}

		// Token: 0x04003945 RID: 14661
		public const float SELECTION_RANGE = 5f;

		// Token: 0x04003947 RID: 14663
		[Header("Settings")]
		public LayerMask DetectionMask;

		// Token: 0x04003948 RID: 14664
		public Color HoverOutlineColor;

		// Token: 0x04003949 RID: 14665
		private Type TypeRequirement;

		// Token: 0x0400394A RID: 14666
		private Action<NPC> callback;

		// Token: 0x0400394B RID: 14667
		private NPC hoveredNPC;

		// Token: 0x0400394C RID: 14668
		private NPC highlightedNPC;
	}
}
