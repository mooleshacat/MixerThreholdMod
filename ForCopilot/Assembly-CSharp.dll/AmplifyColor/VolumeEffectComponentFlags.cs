using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000CAA RID: 3242
	[Serializable]
	public class VolumeEffectComponentFlags
	{
		// Token: 0x06005AF5 RID: 23285 RVA: 0x0017F6C5 File Offset: 0x0017D8C5
		public VolumeEffectComponentFlags(string name)
		{
			this.componentName = name;
			this.componentFields = new List<VolumeEffectFieldFlags>();
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x0017F6E0 File Offset: 0x0017D8E0
		public VolumeEffectComponentFlags(VolumeEffectComponent comp) : this(comp.componentName)
		{
			this.blendFlag = true;
			foreach (VolumeEffectField volumeEffectField in comp.fields)
			{
				if (VolumeEffectField.IsValidType(volumeEffectField.fieldType))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(volumeEffectField));
				}
			}
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x0017F760 File Offset: 0x0017D960
		public VolumeEffectComponentFlags(Component c)
		{
			Type type = c.GetType();
			this..ctor(((type != null) ? type.ToString() : null) ?? "");
			foreach (FieldInfo fieldInfo in c.GetType().GetFields())
			{
				if (VolumeEffectField.IsValidType(fieldInfo.FieldType.FullName))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(fieldInfo));
				}
			}
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x0017F7D0 File Offset: 0x0017D9D0
		public void UpdateComponentFlags(VolumeEffectComponent comp)
		{
			using (List<VolumeEffectField>.Enumerator enumerator = comp.fields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectField field = enumerator.Current;
					if (this.componentFields.Find((VolumeEffectFieldFlags s) => s.fieldName == field.fieldName) == null && VolumeEffectField.IsValidType(field.fieldType))
					{
						this.componentFields.Add(new VolumeEffectFieldFlags(field));
					}
				}
			}
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x0017F868 File Offset: 0x0017DA68
		public void UpdateComponentFlags(Component c)
		{
			FieldInfo[] fields = c.GetType().GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo pi = fields[i];
				if (!this.componentFields.Exists((VolumeEffectFieldFlags s) => s.fieldName == pi.Name) && VolumeEffectField.IsValidType(pi.FieldType.FullName))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(pi));
				}
			}
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x0017F8E4 File Offset: 0x0017DAE4
		public string[] GetFieldNames()
		{
			return (from r in this.componentFields
			where r.blendFlag
			select r.fieldName).ToArray<string>();
		}

		// Token: 0x040042C6 RID: 17094
		public string componentName;

		// Token: 0x040042C7 RID: 17095
		public List<VolumeEffectFieldFlags> componentFields;

		// Token: 0x040042C8 RID: 17096
		public bool blendFlag;
	}
}
