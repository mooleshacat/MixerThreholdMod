using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.EntityFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x02000727 RID: 1831
	public class ObjectSelector : Singleton<ObjectSelector>
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x000CEA20 File Offset: 0x000CCC20
		// (set) Token: 0x06003193 RID: 12691 RVA: 0x000CEA28 File Offset: 0x000CCC28
		public bool isSelecting { get; protected set; }

		// Token: 0x06003194 RID: 12692 RVA: 0x000CEA31 File Offset: 0x000CCC31
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000CEA4C File Offset: 0x000CCC4C
		protected virtual void Update()
		{
			if (this.isSelecting)
			{
				this.hoveredBuildable = this.GetHoveredBuildable();
				this.hoveredConstructable = this.GetHoveredConstructable();
				if (this.hoveredBuildable != null || this.hoveredConstructable != null)
				{
					Singleton<HUD>.Instance.ShowRadialIndicator(1f);
				}
				RaycastHit raycastHit;
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, true, 0f))
				{
					if (raycastHit.collider.GetComponentInParent<BuildableItem>())
					{
						BuildableItem componentInParent = raycastHit.collider.GetComponentInParent<BuildableItem>();
						if (this.allowedTypes.Contains(componentInParent.GetType()))
						{
							if (this.selectedObjects.Contains(componentInParent))
							{
								Console.Log("Deselected: " + componentInParent.ItemInstance.Name, null);
								this.selectedObjects.Remove(componentInParent);
								return;
							}
							if (this.selectedObjects.Count + this.selectedConstructables.Count < this.selectionLimit)
							{
								Console.Log("Selected: " + componentInParent.ItemInstance.Name, null);
								this.selectedObjects.Add(componentInParent);
								if (this.selectedObjects.Count + this.selectedConstructables.Count >= this.selectionLimit && this.exitOnSelectionLimit)
								{
									this.StopSelecting();
									return;
								}
							}
						}
					}
					else if (raycastHit.collider.GetComponentInParent<Constructable>())
					{
						Constructable componentInParent2 = raycastHit.collider.GetComponentInParent<Constructable>();
						if (this.allowedTypes.Contains(componentInParent2.GetType()))
						{
							if (this.selectedConstructables.Contains(componentInParent2))
							{
								Console.Log("Deselected: " + componentInParent2.ConstructableName, null);
								this.selectedConstructables.Remove(componentInParent2);
								return;
							}
							if (this.selectedObjects.Count + this.selectedConstructables.Count < this.selectionLimit)
							{
								Console.Log("Selected: " + componentInParent2.ConstructableName, null);
								this.selectedConstructables.Add(componentInParent2);
								if (this.selectedObjects.Count + this.selectedConstructables.Count >= this.selectionLimit && this.exitOnSelectionLimit)
								{
									this.StopSelecting();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000CEC9C File Offset: 0x000CCE9C
		protected virtual void LateUpdate()
		{
			if (this.isSelecting)
			{
				for (int i = 0; i < this.outlinedObjects.Count; i++)
				{
					this.outlinedObjects[i].HideOutline();
				}
				for (int j = 0; j < this.outlinedConstructables.Count; j++)
				{
					this.outlinedConstructables[j].HideOutline();
				}
				this.outlinedObjects.Clear();
				this.outlinedConstructables.Clear();
				for (int k = 0; k < this.selectedConstructables.Count; k++)
				{
					this.selectedConstructables[k].ShowOutline(BuildableItem.EOutlineColor.Blue);
					this.outlinedConstructables.Add(this.selectedConstructables[k]);
				}
				for (int l = 0; l < this.selectedObjects.Count; l++)
				{
					this.selectedObjects[l].ShowOutline(BuildableItem.EOutlineColor.Blue);
					this.outlinedObjects.Add(this.selectedObjects[l]);
				}
				if (this.hoveredBuildable != null)
				{
					if (this.selectedObjects.Contains(this.hoveredBuildable))
					{
						this.hoveredBuildable.ShowOutline(BuildableItem.EOutlineColor.LightBlue);
					}
					else
					{
						this.hoveredBuildable.ShowOutline(BuildableItem.EOutlineColor.White);
						this.outlinedObjects.Add(this.hoveredBuildable);
					}
				}
				if (this.hoveredConstructable != null)
				{
					if (this.selectedConstructables.Contains(this.hoveredConstructable))
					{
						this.hoveredConstructable.ShowOutline(BuildableItem.EOutlineColor.LightBlue);
						return;
					}
					this.hoveredConstructable.ShowOutline(BuildableItem.EOutlineColor.White);
					this.outlinedConstructables.Add(this.hoveredConstructable);
				}
			}
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000CEE30 File Offset: 0x000CD030
		private BuildableItem GetHoveredBuildable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0.1f))
			{
				BuildableItem componentInParent = raycastHit.collider.GetComponentInParent<BuildableItem>();
				if (componentInParent != null && this.allowedTypes.Contains(componentInParent.GetType()))
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x000CEE8C File Offset: 0x000CD08C
		private Constructable GetHoveredConstructable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0.1f))
			{
				Constructable componentInParent = raycastHit.collider.GetComponentInParent<Constructable>();
				if (componentInParent != null && this.allowedTypes.Contains(componentInParent.GetType()))
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000CEEE5 File Offset: 0x000CD0E5
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (action.exitType == ExitType.Escape && this.isSelecting)
			{
				action.Used = true;
				this.StopSelecting();
			}
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x000CEF10 File Offset: 0x000CD110
		public void StartSelecting(string selectionTitle, List<Type> _typeRestriction, ref List<BuildableItem> initialSelection_Objects, ref List<Constructable> initalSelection_Constructables, int _selectionLimit, bool _exitOnSelectionLimit)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.allowedTypes = _typeRestriction;
			this.selectedObjects = initialSelection_Objects;
			this.selectedConstructables = initalSelection_Constructables;
			for (int i = 0; i < this.selectedObjects.Count; i++)
			{
				this.selectedObjects[i].ShowOutline(BuildableItem.EOutlineColor.White);
				this.outlinedObjects.Add(this.selectedObjects[i]);
			}
			for (int j = 0; j < this.selectedConstructables.Count; j++)
			{
				this.selectedConstructables[j].ShowOutline(BuildableItem.EOutlineColor.White);
				this.outlinedConstructables.Add(this.selectedConstructables[j]);
			}
			this.selectionLimit = _selectionLimit;
			Singleton<HUD>.Instance.ShowTopScreenText(selectionTitle);
			this.isSelecting = true;
			this.exitOnSelectionLimit = _exitOnSelectionLimit;
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000CEFE8 File Offset: 0x000CD1E8
		public void StopSelecting()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.allowedTypes = null;
			for (int i = 0; i < this.outlinedObjects.Count; i++)
			{
				this.outlinedObjects[i].HideOutline();
			}
			for (int j = 0; j < this.outlinedConstructables.Count; j++)
			{
				this.outlinedConstructables[j].HideOutline();
			}
			this.outlinedObjects.Clear();
			this.outlinedConstructables.Clear();
			if (this.onClose != null)
			{
				this.onClose();
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			this.isSelecting = false;
		}

		// Token: 0x040022DD RID: 8925
		[Header("Settings")]
		[SerializeField]
		protected float detectionRange = 5f;

		// Token: 0x040022DE RID: 8926
		[SerializeField]
		protected LayerMask detectionMask;

		// Token: 0x040022E0 RID: 8928
		private List<Type> allowedTypes;

		// Token: 0x040022E1 RID: 8929
		private List<BuildableItem> selectedObjects = new List<BuildableItem>();

		// Token: 0x040022E2 RID: 8930
		private List<Constructable> selectedConstructables = new List<Constructable>();

		// Token: 0x040022E3 RID: 8931
		public Action onClose;

		// Token: 0x040022E4 RID: 8932
		private int selectionLimit;

		// Token: 0x040022E5 RID: 8933
		private bool exitOnSelectionLimit;

		// Token: 0x040022E6 RID: 8934
		private BuildableItem hoveredBuildable;

		// Token: 0x040022E7 RID: 8935
		private Constructable hoveredConstructable;

		// Token: 0x040022E8 RID: 8936
		private List<BuildableItem> outlinedObjects = new List<BuildableItem>();

		// Token: 0x040022E9 RID: 8937
		private List<Constructable> outlinedConstructables = new List<Constructable>();
	}
}
