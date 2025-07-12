using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B32 RID: 2866
	public class ObjectFieldUI : MonoBehaviour
	{
		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06004C67 RID: 19559 RVA: 0x00141895 File Offset: 0x0013FA95
		// (set) Token: 0x06004C68 RID: 19560 RVA: 0x0014189D File Offset: 0x0013FA9D
		public List<ObjectField> Fields { get; protected set; } = new List<ObjectField>();

		// Token: 0x06004C69 RID: 19561 RVA: 0x001418A8 File Offset: 0x0013FAA8
		public void Bind(List<ObjectField> field)
		{
			this.Fields = new List<ObjectField>();
			this.Fields.AddRange(field);
			this.Fields[this.Fields.Count - 1].onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.Refresh));
			this.Refresh(this.Fields[0].SelectedObject);
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x00141914 File Offset: 0x0013FB14
		private void Refresh(BuildableItem newVal)
		{
			this.IconImg.gameObject.SetActive(false);
			this.NoneSelected.gameObject.SetActive(false);
			this.MultipleSelected.gameObject.SetActive(false);
			if (this.AreFieldsUniform())
			{
				if (newVal != null)
				{
					this.IconImg.sprite = newVal.ItemInstance.Icon;
					this.SelectionLabel.text = newVal.ItemInstance.Name;
					this.IconImg.gameObject.SetActive(true);
				}
				else
				{
					this.NoneSelected.SetActive(true);
					this.SelectionLabel.text = "None";
				}
			}
			else
			{
				this.MultipleSelected.SetActive(true);
				this.SelectionLabel.text = "Mixed";
			}
			ObjectField objectField = this.Fields.FirstOrDefault((ObjectField x) => x.SelectedObject != null);
			this.ClearButton.gameObject.SetActive(objectField != null);
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x00141A20 File Offset: 0x0013FC20
		private bool AreFieldsUniform()
		{
			for (int i = 0; i < this.Fields.Count - 1; i++)
			{
				if (this.Fields[i].SelectedObject != this.Fields[i + 1].SelectedObject)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x00141A74 File Offset: 0x0013FC74
		public void Clicked()
		{
			BuildableItem buildableItem = null;
			if (this.AreFieldsUniform())
			{
				buildableItem = this.Fields[0].SelectedObject;
			}
			List<BuildableItem> list = new List<BuildableItem>();
			if (buildableItem != null)
			{
				list.Add(buildableItem);
			}
			List<Transform> list2 = new List<Transform>();
			for (int i = 0; i < this.Fields.Count; i++)
			{
				if (this.Fields[i].DrawTransitLine)
				{
					list2.Add(this.Fields[i].ParentConfig.Configurable.UIPoint);
				}
			}
			Singleton<ManagementInterface>.Instance.ObjectSelector.Open(this.InstructionText, this.ExtendedInstructionText, 1, list, this.Fields[0].TypeRequirements, this.Fields[0].ParentConfig.Configurable.ParentProperty, new ObjectSelector.ObjectFilter(this.ObjectValid), new Action<List<BuildableItem>>(this.ObjectsSelected), list2);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x00141B68 File Offset: 0x0013FD68
		private bool ObjectValid(BuildableItem obj, out string reason)
		{
			string text = string.Empty;
			for (int i = 0; i < this.Fields.Count; i++)
			{
				if (this.Fields[i].objectFilter == null || this.Fields[i].objectFilter(obj, out reason))
				{
					reason = string.Empty;
					return true;
				}
				text = reason;
			}
			reason = text;
			return false;
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x00141BCE File Offset: 0x0013FDCE
		public void ObjectsSelected(List<BuildableItem> objs)
		{
			this.ObjectSelected((objs.Count > 0) ? objs[objs.Count - 1] : null);
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x00141BF0 File Offset: 0x0013FDF0
		private void ObjectSelected(BuildableItem obj)
		{
			if (obj != null && this.Fields[0].TypeRequirements.Count > 0 && !this.Fields[0].TypeRequirements.Contains(obj.GetType()))
			{
				Console.LogError("Wrong Object type selection", null);
				return;
			}
			foreach (ObjectField objectField in this.Fields)
			{
				objectField.SetObject(obj, true);
			}
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x00141C90 File Offset: 0x0013FE90
		public void ClearClicked()
		{
			this.ObjectSelected(null);
		}

		// Token: 0x040038F5 RID: 14581
		[Header("References")]
		public string InstructionText = "Select <ObjectType>";

		// Token: 0x040038F6 RID: 14582
		public string ExtendedInstructionText = string.Empty;

		// Token: 0x040038F7 RID: 14583
		public TextMeshProUGUI FieldLabel;

		// Token: 0x040038F8 RID: 14584
		public Image IconImg;

		// Token: 0x040038F9 RID: 14585
		public TextMeshProUGUI SelectionLabel;

		// Token: 0x040038FA RID: 14586
		public GameObject NoneSelected;

		// Token: 0x040038FB RID: 14587
		public GameObject MultipleSelected;

		// Token: 0x040038FC RID: 14588
		public RectTransform ClearButton;
	}
}
