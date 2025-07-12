using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B49 RID: 2889
	public class TransitEntitySelector : MonoBehaviour
	{
		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06004CFF RID: 19711 RVA: 0x0014481F File Offset: 0x00142A1F
		// (set) Token: 0x06004D00 RID: 19712 RVA: 0x00144827 File Offset: 0x00142A27
		public bool IsOpen { get; protected set; }

		// Token: 0x06004D01 RID: 19713 RVA: 0x00144830 File Offset: 0x00142A30
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 12);
			Singleton<ManagementClipboard>.Instance.onClipboardUnequipped.AddListener(new UnityAction(this.ClipboardClosed));
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x00144860 File Offset: 0x00142A60
		public virtual void Open(string _selectionTitle, string instruction, int _maxSelectedObjects, List<ITransitEntity> _selectedObjects, List<Type> _typeRequirements, TransitEntitySelector.ObjectFilter _objectFilter, Action<List<ITransitEntity>> _callback, List<Transform> transitLineSources = null, bool selectingDestination = true)
		{
			this.IsOpen = true;
			this.changesMade = false;
			this.selectDestination = selectingDestination;
			this.selectionTitle = _selectionTitle;
			if (instruction != string.Empty)
			{
				Singleton<HUD>.Instance.ShowTopScreenText(instruction);
			}
			this.maxSelectedObjects = _maxSelectedObjects;
			this.selectedObjects = new List<ITransitEntity>();
			this.selectedObjects.AddRange(_selectedObjects);
			for (int i = 0; i < this.selectedObjects.Count; i++)
			{
				this.SetSelectionOutline(this.selectedObjects[i], true);
			}
			this.objectFilter = _objectFilter;
			this.typeRequirements = _typeRequirements;
			this.callback = _callback;
			this.UpdateInstructions();
			Singleton<ManagementInterface>.Instance.EquippedClipboard.OverrideClipboardText(this.selectionTitle);
			Singleton<ManagementClipboard>.Instance.Close(true);
			if (this.maxSelectedObjects == 1)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("objectselector");
			}
			else
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("objectselector_multi");
			}
			if (transitLineSources != null)
			{
				this.transitSources.Clear();
				this.transitSources.AddRange(transitLineSources);
				for (int j = 0; j < this.transitSources.Count; j++)
				{
					TransitLineVisuals item = UnityEngine.Object.Instantiate<TransitLineVisuals>(Singleton<ManagementWorldspaceCanvas>.Instance.TransitRouteVisualsPrefab, NetworkSingleton<GameManager>.Instance.Temp);
					this.transitLines.Add(item);
				}
				this.UpdateTransitLines();
			}
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x001449B0 File Offset: 0x00142BB0
		private void UpdateTransitLines()
		{
			float num = 1.5f;
			Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * num;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(num, out raycastHit, this.DetectionMask, false, 0f))
			{
				vector = raycastHit.point;
			}
			for (int i = 0; i < this.transitSources.Count; i++)
			{
				if (this.selectDestination)
				{
					this.transitLines[i].SetSourcePosition(this.transitSources[i].position);
					this.transitLines[i].SetDestinationPosition(vector);
				}
				else
				{
					this.transitLines[i].SetSourcePosition(vector);
					this.transitLines[i].SetDestinationPosition(this.transitSources[i].position);
				}
			}
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x00144A9C File Offset: 0x00142C9C
		public virtual void Close(bool returnToClipboard, bool pushChanges)
		{
			this.IsOpen = false;
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "npcselector" || Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "objectselector_multi")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			for (int i = 0; i < this.selectedObjects.Count; i++)
			{
				this.SetSelectionOutline(this.selectedObjects[i], false);
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			if (returnToClipboard)
			{
				Singleton<ManagementInterface>.Instance.EquippedClipboard.EndOverride();
				Singleton<ManagementClipboard>.Instance.Open(Singleton<ManagementInterface>.Instance.Configurables, Singleton<ManagementInterface>.Instance.EquippedClipboard);
			}
			for (int j = 0; j < this.transitLines.Count; j++)
			{
				UnityEngine.Object.Destroy(this.transitLines[j].gameObject);
			}
			this.transitLines.Clear();
			this.transitSources.Clear();
			this.objectFilter = null;
			if (pushChanges)
			{
				this.callback(this.selectedObjects);
			}
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x00144BA8 File Offset: 0x00142DA8
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.hoveredObj = this.GetHoveredObject();
			string empty = string.Empty;
			if (this.hoveredObj != null && this.IsObjectTypeValid(this.hoveredObj, out empty))
			{
				if (this.hoveredObj != this.highlightedObj && !this.selectedObjects.Contains(this.hoveredObj))
				{
					if (this.highlightedObj != null)
					{
						if (this.selectedObjects.Contains(this.highlightedObj))
						{
							this.highlightedObj.ShowOutline(this.SelectOutlineColor);
						}
						else
						{
							this.highlightedObj.HideOutline();
						}
						this.highlightedObj = null;
					}
					this.highlightedObj = this.hoveredObj;
					this.hoveredObj.ShowOutline(this.HoverOutlineColor);
				}
			}
			else
			{
				Singleton<HUD>.Instance.CrosshairText.Show(empty, new Color32(byte.MaxValue, 125, 125, byte.MaxValue));
				if (this.highlightedObj != null)
				{
					if (this.selectedObjects.Contains(this.highlightedObj))
					{
						this.highlightedObj.ShowOutline(this.SelectOutlineColor);
					}
					else
					{
						this.highlightedObj.HideOutline();
					}
					this.highlightedObj = null;
				}
			}
			this.UpdateInstructions();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.hoveredObj != null && this.IsObjectTypeValid(this.hoveredObj, out empty))
			{
				this.ObjectClicked(this.hoveredObj);
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Submit) && this.maxSelectedObjects > 1)
			{
				this.Close(true, true);
			}
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x00144D28 File Offset: 0x00142F28
		private void LateUpdate()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.UpdateTransitLines();
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x00144D3C File Offset: 0x00142F3C
		private void UpdateInstructions()
		{
			string text = this.selectionTitle;
			if (this.maxSelectedObjects > 1)
			{
				text = string.Concat(new string[]
				{
					text,
					" (",
					this.selectedObjects.Count.ToString(),
					"/",
					this.maxSelectedObjects.ToString(),
					")"
				});
			}
			Singleton<HUD>.Instance.ShowTopScreenText(text);
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x00144DB0 File Offset: 0x00142FB0
		private ITransitEntity GetHoveredObject()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, this.DetectionMask, false, 0.1f))
			{
				return raycastHit.collider.GetComponentInParent<ITransitEntity>();
			}
			return null;
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x00144DEC File Offset: 0x00142FEC
		public bool IsObjectTypeValid(ITransitEntity obj, out string reason)
		{
			reason = string.Empty;
			if (this.typeRequirements.Count > 0 && !this.typeRequirements.Contains(obj.GetType()))
			{
				bool flag = false;
				for (int i = 0; i < this.typeRequirements.Count; i++)
				{
					if (obj.GetType().IsAssignableFrom(this.typeRequirements[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					reason = "Does not match type requirement";
					return false;
				}
			}
			string text;
			if (this.objectFilter != null && !this.objectFilter(obj, out text))
			{
				reason = text;
				return false;
			}
			return true;
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x00144E80 File Offset: 0x00143080
		public void ObjectClicked(ITransitEntity obj)
		{
			string text;
			if (!this.IsObjectTypeValid(obj, out text))
			{
				return;
			}
			this.changesMade = true;
			if (!this.selectedObjects.Contains(obj))
			{
				if (this.selectedObjects.Count < this.maxSelectedObjects)
				{
					this.selectedObjects.Add(obj);
					this.SetSelectionOutline(obj, true);
				}
			}
			else if (this.maxSelectedObjects > 1)
			{
				this.selectedObjects.Remove(obj);
				this.SetSelectionOutline(obj, false);
			}
			if (this.maxSelectedObjects == 1 || !GameInput.GetButton(GameInput.ButtonCode.Sprint))
			{
				this.Close(true, true);
				return;
			}
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x00144F10 File Offset: 0x00143110
		private void SetSelectionOutline(ITransitEntity obj, bool on)
		{
			if (obj == null)
			{
				return;
			}
			if (on)
			{
				obj.ShowOutline(this.SelectOutlineColor);
				return;
			}
			obj.HideOutline();
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x00144F2C File Offset: 0x0014312C
		private void ClipboardClosed()
		{
			this.Close(false, false);
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x00144F36 File Offset: 0x00143136
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
				this.Close(true, this.changesMade);
			}
		}

		// Token: 0x04003970 RID: 14704
		public const float SELECTION_RANGE = 5f;

		// Token: 0x04003972 RID: 14706
		[Header("Settings")]
		public LayerMask DetectionMask;

		// Token: 0x04003973 RID: 14707
		public Color HoverOutlineColor;

		// Token: 0x04003974 RID: 14708
		public Color SelectOutlineColor;

		// Token: 0x04003975 RID: 14709
		private int maxSelectedObjects;

		// Token: 0x04003976 RID: 14710
		private List<ITransitEntity> selectedObjects = new List<ITransitEntity>();

		// Token: 0x04003977 RID: 14711
		private List<Type> typeRequirements = new List<Type>();

		// Token: 0x04003978 RID: 14712
		private TransitEntitySelector.ObjectFilter objectFilter;

		// Token: 0x04003979 RID: 14713
		private Action<List<ITransitEntity>> callback;

		// Token: 0x0400397A RID: 14714
		private ITransitEntity hoveredObj;

		// Token: 0x0400397B RID: 14715
		private ITransitEntity highlightedObj;

		// Token: 0x0400397C RID: 14716
		private string selectionTitle = "";

		// Token: 0x0400397D RID: 14717
		private bool changesMade;

		// Token: 0x0400397E RID: 14718
		private List<Transform> transitSources = new List<Transform>();

		// Token: 0x0400397F RID: 14719
		private List<TransitLineVisuals> transitLines = new List<TransitLineVisuals>();

		// Token: 0x04003980 RID: 14720
		private bool selectDestination = true;

		// Token: 0x02000B4A RID: 2890
		// (Invoke) Token: 0x06004D10 RID: 19728
		public delegate bool ObjectFilter(ITransitEntity obj, out string reason);
	}
}
