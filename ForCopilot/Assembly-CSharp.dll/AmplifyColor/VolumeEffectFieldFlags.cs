using System;
using System.Reflection;

namespace AmplifyColor
{
	// Token: 0x02000CA9 RID: 3241
	[Serializable]
	public class VolumeEffectFieldFlags
	{
		// Token: 0x06005AF3 RID: 23283 RVA: 0x0017F679 File Offset: 0x0017D879
		public VolumeEffectFieldFlags(FieldInfo pi)
		{
			this.fieldName = pi.Name;
			this.fieldType = pi.FieldType.FullName;
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x0017F69E File Offset: 0x0017D89E
		public VolumeEffectFieldFlags(VolumeEffectField field)
		{
			this.fieldName = field.fieldName;
			this.fieldType = field.fieldType;
			this.blendFlag = true;
		}

		// Token: 0x040042C3 RID: 17091
		public string fieldName;

		// Token: 0x040042C4 RID: 17092
		public string fieldType;

		// Token: 0x040042C5 RID: 17093
		public bool blendFlag;
	}
}
