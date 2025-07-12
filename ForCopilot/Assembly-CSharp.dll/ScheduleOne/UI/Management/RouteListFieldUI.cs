using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B38 RID: 2872
	public class RouteListFieldUI : MonoBehaviour
	{
		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06004C93 RID: 19603 RVA: 0x001425F0 File Offset: 0x001407F0
		// (set) Token: 0x06004C94 RID: 19604 RVA: 0x001425F8 File Offset: 0x001407F8
		public List<RouteListField> Fields { get; protected set; } = new List<RouteListField>();

		// Token: 0x06004C95 RID: 19605 RVA: 0x00142604 File Offset: 0x00140804
		private void Start()
		{
			this.FieldLabel.text = this.FieldText;
			for (int i = 0; i < this.RouteEntries.Length; i++)
			{
				RouteEntryUI entry = this.RouteEntries[i];
				this.RouteEntries[i].onDeleteClicked.AddListener(delegate()
				{
					this.EntryDeleteClicked(entry);
				});
			}
			this.AddButton.onClick.AddListener(new UnityAction(this.AddClicked));
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x0014268C File Offset: 0x0014088C
		public void Bind(List<RouteListField> field)
		{
			this.Fields = new List<RouteListField>();
			this.Fields.AddRange(field);
			this.Refresh(this.Fields[0].Routes);
			this.Fields[0].onListChanged.AddListener(new UnityAction<List<AdvancedTransitRoute>>(this.Refresh));
			this.MultiEditBlocker.gameObject.SetActive(this.Fields.Count > 1);
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x00142708 File Offset: 0x00140908
		private void Refresh(List<AdvancedTransitRoute> newVal)
		{
			int num = 0;
			for (int i = 0; i < this.RouteEntries.Length; i++)
			{
				if (newVal.Count > i)
				{
					num++;
					this.RouteEntries[i].AssignRoute(newVal[i]);
					this.RouteEntries[i].gameObject.SetActive(true);
				}
				else
				{
					this.RouteEntries[i].ClearRoute();
					this.RouteEntries[i].gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < newVal.Count; j++)
			{
				AdvancedTransitRoute advancedTransitRoute = newVal[j];
				advancedTransitRoute.onSourceChange = (Action<ITransitEntity>)Delegate.Remove(advancedTransitRoute.onSourceChange, new Action<ITransitEntity>(this.RouteChanged));
				AdvancedTransitRoute advancedTransitRoute2 = newVal[j];
				advancedTransitRoute2.onDestinationChange = (Action<ITransitEntity>)Delegate.Remove(advancedTransitRoute2.onDestinationChange, new Action<ITransitEntity>(this.RouteChanged));
				AdvancedTransitRoute advancedTransitRoute3 = newVal[j];
				advancedTransitRoute3.onSourceChange = (Action<ITransitEntity>)Delegate.Combine(advancedTransitRoute3.onSourceChange, new Action<ITransitEntity>(this.RouteChanged));
				AdvancedTransitRoute advancedTransitRoute4 = newVal[j];
				advancedTransitRoute4.onDestinationChange = (Action<ITransitEntity>)Delegate.Combine(advancedTransitRoute4.onDestinationChange, new Action<ITransitEntity>(this.RouteChanged));
			}
			this.AddButton.gameObject.SetActive(num < this.Fields[0].MaxRoutes);
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x0014285B File Offset: 0x00140A5B
		private void EntryDeleteClicked(RouteEntryUI entry)
		{
			this.Fields[0].RemoveItem(entry.AssignedRoute);
			entry.ClearRoute();
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x0014287A File Offset: 0x00140A7A
		private void AddClicked()
		{
			this.Fields[0].AddItem(new AdvancedTransitRoute(null, null));
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x00142894 File Offset: 0x00140A94
		private void RouteChanged(ITransitEntity newEntity)
		{
			this.Fields[0].Replicate();
		}

		// Token: 0x04003917 RID: 14615
		[Header("References")]
		public string FieldText = "Routes";

		// Token: 0x04003918 RID: 14616
		public TextMeshProUGUI FieldLabel;

		// Token: 0x04003919 RID: 14617
		public RouteEntryUI[] RouteEntries;

		// Token: 0x0400391A RID: 14618
		public RectTransform MultiEditBlocker;

		// Token: 0x0400391B RID: 14619
		public Button AddButton;
	}
}
