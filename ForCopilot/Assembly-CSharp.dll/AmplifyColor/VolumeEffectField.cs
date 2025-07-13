using System;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000CA1 RID: 3233
	[Serializable]
	public class VolumeEffectField
	{
		// Token: 0x06005AC9 RID: 23241 RVA: 0x0017E739 File Offset: 0x0017C939
		public VolumeEffectField(string fieldName, string fieldType)
		{
			this.fieldName = fieldName;
			this.fieldType = fieldType;
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x0017E750 File Offset: 0x0017C950
		public VolumeEffectField(FieldInfo pi, Component c) : this(pi.Name, pi.FieldType.FullName)
		{
			object value = pi.GetValue(c);
			this.UpdateValue(value);
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x0017E784 File Offset: 0x0017C984
		public static bool IsValidType(string type)
		{
			return type == "System.Single" || type == "System.Boolean" || type == "UnityEngine.Color" || type == "UnityEngine.Vector2" || type == "UnityEngine.Vector3" || type == "UnityEngine.Vector4";
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x0017E7E4 File Offset: 0x0017C9E4
		public void UpdateValue(object val)
		{
			string a = this.fieldType;
			if (a == "System.Single")
			{
				this.valueSingle = (float)val;
				return;
			}
			if (a == "System.Boolean")
			{
				this.valueBoolean = (bool)val;
				return;
			}
			if (a == "UnityEngine.Color")
			{
				this.valueColor = (Color)val;
				return;
			}
			if (a == "UnityEngine.Vector2")
			{
				this.valueVector2 = (Vector2)val;
				return;
			}
			if (a == "UnityEngine.Vector3")
			{
				this.valueVector3 = (Vector3)val;
				return;
			}
			if (!(a == "UnityEngine.Vector4"))
			{
				return;
			}
			this.valueVector4 = (Vector4)val;
		}

		// Token: 0x040042AD RID: 17069
		public string fieldName;

		// Token: 0x040042AE RID: 17070
		public string fieldType;

		// Token: 0x040042AF RID: 17071
		public float valueSingle;

		// Token: 0x040042B0 RID: 17072
		public Color valueColor;

		// Token: 0x040042B1 RID: 17073
		public bool valueBoolean;

		// Token: 0x040042B2 RID: 17074
		public Vector2 valueVector2;

		// Token: 0x040042B3 RID: 17075
		public Vector3 valueVector3;

		// Token: 0x040042B4 RID: 17076
		public Vector4 valueVector4;
	}
}
