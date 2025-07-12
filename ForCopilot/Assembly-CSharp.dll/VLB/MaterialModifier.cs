using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000133 RID: 307
	public static class MaterialModifier
	{
		// Token: 0x02000134 RID: 308
		public interface Interface
		{
			// Token: 0x0600053A RID: 1338
			void SetMaterialProp(int nameID, float value);

			// Token: 0x0600053B RID: 1339
			void SetMaterialProp(int nameID, Vector4 value);

			// Token: 0x0600053C RID: 1340
			void SetMaterialProp(int nameID, Color value);

			// Token: 0x0600053D RID: 1341
			void SetMaterialProp(int nameID, Matrix4x4 value);

			// Token: 0x0600053E RID: 1342
			void SetMaterialProp(int nameID, Texture value);
		}

		// Token: 0x02000135 RID: 309
		// (Invoke) Token: 0x06000540 RID: 1344
		public delegate void Callback(MaterialModifier.Interface owner);
	}
}
