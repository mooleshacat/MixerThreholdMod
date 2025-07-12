using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall.Utilities
{
	// Token: 0x02000239 RID: 569
	[Serializable]
	public class MaterialOverrides
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x00037DAC File Offset: 0x00035FAC
		public void OverrideMaterial(Material material)
		{
			foreach (MaterialOverrides.TextureProperty textureProperty in this.textureOverrides)
			{
				material.SetTexture(textureProperty.propertyName, textureProperty.propertyValue);
			}
			foreach (MaterialOverrides.FloatProperty floatProperty in this.floatOverrides)
			{
				material.SetFloat(floatProperty.propertyName, floatProperty.propertyValue);
			}
			foreach (MaterialOverrides.IntProperty intProperty in this.intOverrides)
			{
				material.SetInt(intProperty.propertyName, intProperty.propertyValue);
			}
			foreach (MaterialOverrides.VectorProperty vectorProperty in this.vectorOverrides)
			{
				material.SetVector(vectorProperty.propertyName, vectorProperty.propertyValue);
			}
			foreach (MaterialOverrides.ColorProperty colorProperty in this.colorOverrides)
			{
				material.SetColor(colorProperty.propertyName, colorProperty.propertyValue);
			}
		}

		// Token: 0x04000D5E RID: 3422
		public List<MaterialOverrides.TextureProperty> textureOverrides;

		// Token: 0x04000D5F RID: 3423
		public List<MaterialOverrides.FloatProperty> floatOverrides;

		// Token: 0x04000D60 RID: 3424
		public List<MaterialOverrides.IntProperty> intOverrides;

		// Token: 0x04000D61 RID: 3425
		public List<MaterialOverrides.VectorProperty> vectorOverrides;

		// Token: 0x04000D62 RID: 3426
		public List<MaterialOverrides.ColorProperty> colorOverrides;

		// Token: 0x0200023A RID: 570
		[Serializable]
		public class TextureProperty
		{
			// Token: 0x04000D63 RID: 3427
			public string propertyName;

			// Token: 0x04000D64 RID: 3428
			public Texture2D propertyValue;
		}

		// Token: 0x0200023B RID: 571
		[Serializable]
		public class FloatProperty
		{
			// Token: 0x04000D65 RID: 3429
			public string propertyName;

			// Token: 0x04000D66 RID: 3430
			public float propertyValue;
		}

		// Token: 0x0200023C RID: 572
		[Serializable]
		public class IntProperty
		{
			// Token: 0x04000D67 RID: 3431
			public string propertyName;

			// Token: 0x04000D68 RID: 3432
			public int propertyValue;
		}

		// Token: 0x0200023D RID: 573
		[Serializable]
		public class VectorProperty
		{
			// Token: 0x04000D69 RID: 3433
			public string propertyName;

			// Token: 0x04000D6A RID: 3434
			public Vector4 propertyValue;
		}

		// Token: 0x0200023E RID: 574
		[Serializable]
		public class ColorProperty
		{
			// Token: 0x04000D6B RID: 3435
			public string propertyName;

			// Token: 0x04000D6C RID: 3436
			public Color propertyValue;
		}
	}
}
