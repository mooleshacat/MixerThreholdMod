using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DD RID: 221
	public class PostProcessingContext
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00014B9B File Offset: 0x00012D9B
		// (set) Token: 0x06000399 RID: 921 RVA: 0x00014BA3 File Offset: 0x00012DA3
		public bool interrupted { get; private set; }

		// Token: 0x0600039A RID: 922 RVA: 0x00014BAC File Offset: 0x00012DAC
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00014BB5 File Offset: 0x00012DB5
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00014BDB File Offset: 0x00012DDB
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00014BEB File Offset: 0x00012DEB
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00014BF8 File Offset: 0x00012DF8
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00014C05 File Offset: 0x00012E05
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00014C12 File Offset: 0x00012E12
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x04000479 RID: 1145
		public PostProcessingProfile profile;

		// Token: 0x0400047A RID: 1146
		public Camera camera;

		// Token: 0x0400047B RID: 1147
		public MaterialFactory materialFactory;

		// Token: 0x0400047C RID: 1148
		public RenderTextureFactory renderTextureFactory;
	}
}
