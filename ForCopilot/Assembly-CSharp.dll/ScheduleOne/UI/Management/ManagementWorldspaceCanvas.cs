using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI.Input;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B3F RID: 2879
	public class ManagementWorldspaceCanvas : Singleton<ManagementWorldspaceCanvas>
	{
		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x00142FB1 File Offset: 0x001411B1
		// (set) Token: 0x06004CB9 RID: 19641 RVA: 0x00142FB9 File Offset: 0x001411B9
		public bool IsOpen { get; protected set; }

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x00142FC2 File Offset: 0x001411C2
		public Property CurrentProperty
		{
			get
			{
				return Player.Local.CurrentProperty;
			}
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x00142FD0 File Offset: 0x001411D0
		public void Open()
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
			for (int i = 0; i < this.SelectedConfigurables.Count; i++)
			{
				this.SelectedConfigurables[i].Selected();
				this.SelectedConfigurables[i].ShowOutline(this.SelectedOutlineColor);
			}
			this.UpdateInputPrompt();
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x00143034 File Offset: 0x00141234
		public void Close(bool preserveSelection = false)
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			if (this.OutlinedConfigurable != null && !this.SelectedConfigurables.Contains(this.OutlinedConfigurable))
			{
				this.OutlinedConfigurable.Deselected();
				this.OutlinedConfigurable.HideOutline();
				this.OutlinedConfigurable = null;
			}
			if (this.HoveredConfigurable != null)
			{
				this.HoveredConfigurable.HideOutline();
				this.HoveredConfigurable = null;
			}
			if (!preserveSelection)
			{
				this.ClearSelection();
			}
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x001430B0 File Offset: 0x001412B0
		private void Update()
		{
			if (Player.Local == null)
			{
				return;
			}
			this.UpdateUIs();
			if (this.IsOpen)
			{
				IConfigurable hoveredConfigurable = this.GetHoveredConfigurable();
				if (hoveredConfigurable != null && !hoveredConfigurable.IsBeingConfiguredByOtherPlayer)
				{
					this.HoveredConfigurable = hoveredConfigurable;
				}
				else
				{
					this.HoveredConfigurable = null;
				}
				this.UpdateSelection();
			}
			else if (this.HoveredConfigurable != null)
			{
				this.HoveredConfigurable.Deselected();
				this.HoveredConfigurable.HideOutline();
				this.HoveredConfigurable = null;
			}
			this.UpdateInputPrompt();
			if (Player.Local.CurrentProperty == null)
			{
				this.ClearSelection();
			}
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x00143148 File Offset: 0x00141348
		private void UpdateInputPrompt()
		{
			List<IConfigurable> list = new List<IConfigurable>();
			if (this.HoveredConfigurable != null && !this.SelectedConfigurables.Contains(this.HoveredConfigurable))
			{
				list.Add(this.HoveredConfigurable);
			}
			list.AddRange(this.SelectedConfigurables);
			if (list.Count == 0)
			{
				this.HideCrosshairPrompt();
				return;
			}
			bool flag = true;
			if (list.Count > 1)
			{
				for (int i = 0; i < list.Count - 1; i++)
				{
					if (list[i].ConfigurableType != list[i + 1].ConfigurableType)
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				this.HideCrosshairPrompt();
				return;
			}
			string typeName = ConfigurableType.GetTypeName(list[0].ConfigurableType);
			if (list.Count > 1)
			{
				this.ShowCrosshairPrompt("Manage " + list.Count.ToString() + "x " + typeName);
				return;
			}
			this.ShowCrosshairPrompt("Manage " + typeName);
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x00143238 File Offset: 0x00141438
		private void UpdateUIs()
		{
			foreach (Property property in Property.OwnedProperties)
			{
				float num = Vector3.Distance(property.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
				property.WorldspaceUIContainer.gameObject.SetActive(this.IsOpen && num < 50f);
			}
			List<IConfigurable> configurablesToShow = this.GetConfigurablesToShow();
			this.RemoveNullConfigurables();
			for (int i = 0; i < this.ShownConfigurables.Count; i++)
			{
				if (!configurablesToShow.Contains(this.ShownConfigurables[i]) && this.ShownConfigurables[i].WorldspaceUI.IsEnabled)
				{
					IConfigurable config = this.ShownConfigurables[i];
					this.ShownConfigurables[i].WorldspaceUI.Hide(delegate
					{
						this.ShownConfigurables.Remove(config);
					});
				}
			}
			for (int j = 0; j < configurablesToShow.Count; j++)
			{
				if (!this.ShownConfigurables.Contains(configurablesToShow[j]))
				{
					configurablesToShow[j].WorldspaceUI.Show();
					if (!this.ShownConfigurables.Contains(configurablesToShow[j]))
					{
						this.ShownConfigurables.Add(configurablesToShow[j]);
					}
				}
			}
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x001433C0 File Offset: 0x001415C0
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.RemoveNullConfigurables();
			this.ShownConfigurables.Sort((IConfigurable a, IConfigurable b) => Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, a.UIPoint.position).CompareTo(Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, b.UIPoint.position)));
			for (int i = 0; i < this.ShownConfigurables.Count; i++)
			{
				this.ShownConfigurables[i].WorldspaceUI.SetInternalScale(this.ScaleCurve.Evaluate(Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.ShownConfigurables[i].UIPoint.position) / 5f));
				this.ShownConfigurables[i].WorldspaceUI.UpdatePosition(this.ShownConfigurables[i].UIPoint.position);
				this.ShownConfigurables[i].WorldspaceUI.transform.SetAsFirstSibling();
			}
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x001434BC File Offset: 0x001416BC
		private void UpdateSelection()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				this.ClearSelection();
			}
			if (this.HoveredConfigurable == null)
			{
				if (this.OutlinedConfigurable != null && !this.SelectedConfigurables.Contains(this.OutlinedConfigurable))
				{
					this.OutlinedConfigurable.Deselected();
					this.OutlinedConfigurable.HideOutline();
					this.OutlinedConfigurable = null;
				}
				return;
			}
			if (this.HoveredConfigurable != null && this.HoveredConfigurable.IsBeingConfiguredByOtherPlayer)
			{
				this.HoveredConfigurable.Deselected();
				this.HoveredConfigurable.HideOutline();
				this.HoveredConfigurable = null;
				return;
			}
			for (int i = 0; i < this.SelectedConfigurables.Count; i++)
			{
				if (this.SelectedConfigurables[i].IsBeingConfiguredByOtherPlayer)
				{
					this.RemoveFromSelection(this.SelectedConfigurables[i]);
					i--;
				}
			}
			if (!this.SelectedConfigurables.Contains(this.HoveredConfigurable) && this.OutlinedConfigurable != this.HoveredConfigurable)
			{
				if (this.OutlinedConfigurable != null && !this.SelectedConfigurables.Contains(this.OutlinedConfigurable))
				{
					this.OutlinedConfigurable.Deselected();
					this.OutlinedConfigurable.HideOutline();
					this.OutlinedConfigurable = null;
				}
				this.HoveredConfigurable.Selected();
				this.HoveredConfigurable.ShowOutline(this.HoveredOutlineColor);
				this.OutlinedConfigurable = this.HoveredConfigurable;
			}
			if (this.HoveredConfigurable == null || !this.HoveredConfigurable.CanBeSelected)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
			{
				if (this.SelectedConfigurables.Contains(this.HoveredConfigurable))
				{
					this.RemoveFromSelection(this.HoveredConfigurable);
					return;
				}
				this.AddToSelection(this.HoveredConfigurable);
			}
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x00143655 File Offset: 0x00141855
		private void AddToSelection(IConfigurable config)
		{
			if (this.SelectedConfigurables.Contains(config))
			{
				return;
			}
			config.ShowOutline(this.SelectedOutlineColor);
			config.Selected();
			this.SelectedConfigurables.Add(config);
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x00143684 File Offset: 0x00141884
		private void RemoveFromSelection(IConfigurable config)
		{
			if (this.HoveredConfigurable != config)
			{
				config.Deselected();
				config.HideOutline();
			}
			else
			{
				config.ShowOutline(this.HoveredOutlineColor);
			}
			if (this.SelectedConfigurables.Contains(config))
			{
				this.SelectedConfigurables.Remove(config);
			}
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x001436C4 File Offset: 0x001418C4
		private void ClearSelection()
		{
			IConfigurable[] array = this.SelectedConfigurables.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				this.RemoveFromSelection(array[i]);
			}
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x001436F4 File Offset: 0x001418F4
		private void RemoveNullConfigurables()
		{
			for (int i = 0; i < this.ShownConfigurables.Count; i++)
			{
				if (this.ShownConfigurables[i].IsDestroyed)
				{
					this.ShownConfigurables.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0014373C File Offset: 0x0014193C
		private IConfigurable GetHoveredConfigurable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, this.ObjectSelectionLayerMask, true, 0f))
			{
				IConfigurable componentInParent = raycastHit.collider.GetComponentInParent<IConfigurable>();
				if (componentInParent != null)
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0014377C File Offset: 0x0014197C
		private List<IConfigurable> GetConfigurablesToShow()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return new List<IConfigurable>();
			}
			List<IConfigurable> list = new List<IConfigurable>();
			if (this.CurrentProperty != null && this.CurrentProperty.IsOwned)
			{
				for (int i = 0; i < this.CurrentProperty.Configurables.Count; i++)
				{
					if (this.CurrentProperty.Configurables[i] != null && !this.CurrentProperty.Configurables[i].IsDestroyed && Vector3.Distance(this.CurrentProperty.Configurables[i].Transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) <= 5f)
					{
						list.Add(this.CurrentProperty.Configurables[i]);
					}
				}
			}
			for (int j = 0; j < this.SelectedConfigurables.Count; j++)
			{
				if (!list.Contains(this.SelectedConfigurables[j]))
				{
					list.Add(this.SelectedConfigurables[j]);
				}
			}
			if (!list.Contains(this.HoveredConfigurable) && this.HoveredConfigurable != null)
			{
				list.Add(this.HoveredConfigurable);
			}
			return list;
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x001438B2 File Offset: 0x00141AB2
		public void ShowCrosshairPrompt(string message)
		{
			this.CrosshairPrompt.SetLabel(message);
			this.CrosshairPrompt.gameObject.SetActive(true);
			this.CrosshairPrompt.transform.SetAsLastSibling();
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x001438E1 File Offset: 0x00141AE1
		public void HideCrosshairPrompt()
		{
			this.CrosshairPrompt.gameObject.SetActive(false);
		}

		// Token: 0x04003933 RID: 14643
		public const float VISIBILITY_RANGE = 5f;

		// Token: 0x04003934 RID: 14644
		public const float PROPERTY_CANVAS_RANGE = 50f;

		// Token: 0x04003936 RID: 14646
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003937 RID: 14647
		public AnimationCurve ScaleCurve;

		// Token: 0x04003938 RID: 14648
		public TransitLineVisuals TransitRouteVisualsPrefab;

		// Token: 0x04003939 RID: 14649
		public InputPrompt CrosshairPrompt;

		// Token: 0x0400393A RID: 14650
		[Header("Settings")]
		public LayerMask ObjectSelectionLayerMask;

		// Token: 0x0400393B RID: 14651
		public Color HoveredOutlineColor = Color.white;

		// Token: 0x0400393C RID: 14652
		public Color SelectedOutlineColor = Color.white;

		// Token: 0x0400393D RID: 14653
		private List<IConfigurable> ShownConfigurables = new List<IConfigurable>();

		// Token: 0x0400393E RID: 14654
		public IConfigurable HoveredConfigurable;

		// Token: 0x0400393F RID: 14655
		private IConfigurable OutlinedConfigurable;

		// Token: 0x04003940 RID: 14656
		public List<IConfigurable> SelectedConfigurables = new List<IConfigurable>();
	}
}
