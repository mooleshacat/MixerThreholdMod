using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200093F RID: 2367
	[CreateAssetMenu(fileName = "PropertyItemDefinition", menuName = "ScriptableObjects/PropertyItemDefinition", order = 1)]
	[Serializable]
	public class PropertyItemDefinition : StorableItemDefinition
	{
		// Token: 0x0600401E RID: 16414 RVA: 0x0010F323 File Offset: 0x0010D523
		public virtual void Initialize(List<Property> properties)
		{
			this.Properties.AddRange(properties);
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x0010F331 File Offset: 0x0010D531
		public bool HasProperty(Property property)
		{
			return this.Properties.Contains(property);
		}

		// Token: 0x04002D94 RID: 11668
		[Header("Properties")]
		public List<Property> Properties = new List<Property>();
	}
}
