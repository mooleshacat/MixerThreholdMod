using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B37 RID: 2871
	public class RouteEntryUI : MonoBehaviour
	{
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06004C87 RID: 19591 RVA: 0x00142334 File Offset: 0x00140534
		// (set) Token: 0x06004C88 RID: 19592 RVA: 0x0014233C File Offset: 0x0014053C
		public AdvancedTransitRoute AssignedRoute { get; private set; }

		// Token: 0x06004C89 RID: 19593 RVA: 0x00142345 File Offset: 0x00140545
		public void AssignRoute(AdvancedTransitRoute route)
		{
			this.AssignedRoute = route;
			this.RefreshUI();
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x00142354 File Offset: 0x00140554
		public void ClearRoute()
		{
			this.AssignedRoute = null;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x00142360 File Offset: 0x00140560
		public void RefreshUI()
		{
			if (this.AssignedRoute != null && this.AssignedRoute.Source != null)
			{
				this.SourceLabel.text = this.AssignedRoute.Source.Name;
			}
			else
			{
				this.SourceLabel.text = "None";
			}
			if (this.AssignedRoute != null && this.AssignedRoute.Destination != null)
			{
				this.DestinationLabel.text = this.AssignedRoute.Destination.Name;
				return;
			}
			this.DestinationLabel.text = "None";
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x001423F0 File Offset: 0x001405F0
		public void SourceClicked()
		{
			this.settingSource = true;
			this.settingDestination = false;
			List<ITransitEntity> selectedObjects = new List<ITransitEntity>();
			List<Transform> list = new List<Transform>();
			if (this.AssignedRoute.Destination != null)
			{
				list.Add(this.AssignedRoute.Destination.LinkOrigin);
			}
			Singleton<ManagementInterface>.Instance.TransitEntitySelector.Open("Select source", "Click an entity to set it as the route source", 1, selectedObjects, new List<Type>(), new TransitEntitySelector.ObjectFilter(this.ObjectValid), new Action<List<ITransitEntity>>(this.ObjectsSelected), list, false);
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x00142474 File Offset: 0x00140674
		public void DestinationClicked()
		{
			this.settingDestination = true;
			this.settingSource = false;
			List<ITransitEntity> selectedObjects = new List<ITransitEntity>();
			List<Transform> list = new List<Transform>();
			if (this.AssignedRoute.Source != null)
			{
				list.Add(this.AssignedRoute.Source.LinkOrigin);
			}
			Singleton<ManagementInterface>.Instance.TransitEntitySelector.Open("Select destination", "Click an entity to set it as the route destination", 1, selectedObjects, new List<Type>(), new TransitEntitySelector.ObjectFilter(this.ObjectValid), new Action<List<ITransitEntity>>(this.ObjectsSelected), list, true);
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x000045B1 File Offset: 0x000027B1
		public void FilterClicked()
		{
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x001424F8 File Offset: 0x001406F8
		public void DeleteClicked()
		{
			if (this.onDeleteClicked != null)
			{
				this.onDeleteClicked.Invoke();
			}
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x00142510 File Offset: 0x00140710
		private bool ObjectValid(ITransitEntity obj, out string reason)
		{
			reason = string.Empty;
			if (this.AssignedRoute == null)
			{
				return false;
			}
			if (obj == null)
			{
				return false;
			}
			if (this.settingDestination && obj == this.AssignedRoute.Source)
			{
				reason = "Destination cannot be the same as the source";
				return false;
			}
			if (this.settingSource && obj == this.AssignedRoute.Destination)
			{
				reason = "Source cannot be the same as the destination";
				return false;
			}
			return true;
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x00142574 File Offset: 0x00140774
		public void ObjectsSelected(List<ITransitEntity> objs)
		{
			if (objs.Count > 1)
			{
				objs.RemoveAt(0);
			}
			if (this.settingSource)
			{
				this.AssignedRoute.SetSource((objs.Count > 0) ? objs[0] : null);
			}
			if (this.settingDestination)
			{
				this.AssignedRoute.SetDestination((objs.Count > 0) ? objs[0] : null);
			}
		}

		// Token: 0x04003910 RID: 14608
		[Header("References")]
		public TextMeshProUGUI SourceLabel;

		// Token: 0x04003911 RID: 14609
		public TextMeshProUGUI DestinationLabel;

		// Token: 0x04003912 RID: 14610
		public Image FilterIcon;

		// Token: 0x04003913 RID: 14611
		public UnityEvent onDeleteClicked = new UnityEvent();

		// Token: 0x04003914 RID: 14612
		private bool settingSource;

		// Token: 0x04003915 RID: 14613
		private bool settingDestination;
	}
}
