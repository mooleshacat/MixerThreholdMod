using System;
using System.Collections.Generic;
using ScheduleOne.Property;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A78 RID: 2680
	public class PropertyDropdown : MonoBehaviour
	{
		// Token: 0x06004811 RID: 18449 RVA: 0x0012EF14 File Offset: 0x0012D114
		protected virtual void Awake()
		{
			List<string> list = new List<string>();
			list.Add("None");
			this.TMP_dropdown = base.GetComponent<TMP_Dropdown>();
			if (this.TMP_dropdown != null)
			{
				this.TMP_dropdown.onValueChanged.AddListener(new UnityAction<int>(this.ValueChanged));
				this.TMP_dropdown.AddOptions(list);
			}
			this.dropdown = base.GetComponent<Dropdown>();
			if (this.dropdown != null)
			{
				this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.ValueChanged));
				this.dropdown.AddOptions(list);
			}
			this.intToProperty.Add(0, null);
			Property.onPropertyAcquired = (Property.PropertyChange)Delegate.Combine(Property.onPropertyAcquired, new Property.PropertyChange(this.PropertyAcquired));
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0012EFE4 File Offset: 0x0012D1E4
		private void PropertyAcquired(Property p)
		{
			List<string> list = new List<string>();
			list.Add(p.PropertyName);
			if (this.dropdown != null)
			{
				this.intToProperty.Add(this.dropdown.options.Count, p);
				this.dropdown.AddOptions(list);
			}
			if (this.TMP_dropdown != null)
			{
				this.intToProperty.Add(this.TMP_dropdown.options.Count, p);
				this.TMP_dropdown.AddOptions(list);
			}
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x0012F06F File Offset: 0x0012D26F
		private void ValueChanged(int newVal)
		{
			this.selectedProperty = this.intToProperty[newVal];
			if (this.onSelectionChanged != null)
			{
				this.onSelectionChanged();
			}
		}

		// Token: 0x040034D8 RID: 13528
		public Property selectedProperty;

		// Token: 0x040034D9 RID: 13529
		private TMP_Dropdown TMP_dropdown;

		// Token: 0x040034DA RID: 13530
		private Dropdown dropdown;

		// Token: 0x040034DB RID: 13531
		private Dictionary<int, Property> intToProperty = new Dictionary<int, Property>();

		// Token: 0x040034DC RID: 13532
		public Action onSelectionChanged;
	}
}
