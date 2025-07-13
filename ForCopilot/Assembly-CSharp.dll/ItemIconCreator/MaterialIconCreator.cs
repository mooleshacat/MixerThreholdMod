using System;
using System.Collections;
using UnityEngine;

namespace ItemIconCreator
{
	// Token: 0x02000231 RID: 561
	[ExecuteInEditMode]
	public class MaterialIconCreator : IconCreator
	{
		// Token: 0x06000BF9 RID: 3065 RVA: 0x00037420 File Offset: 0x00035620
		public override void BuildIcons()
		{
			base.StartCoroutine(this.BuildIconsRotine());
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0003742F File Offset: 0x0003562F
		public override bool CheckConditions()
		{
			if (!base.CheckConditions())
			{
				return false;
			}
			if (this.materials.Length == 0)
			{
				Debug.LogError("There's no materials");
				return false;
			}
			if (this.targetRenderer == null)
			{
				Debug.LogError("There's no target renderer");
				return false;
			}
			return true;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0003746B File Offset: 0x0003566B
		private IEnumerator BuildIconsRotine()
		{
			base.Initialize();
			if (this.dynamicFov)
			{
				base.UpdateFOV(this.targetRenderer.gameObject);
			}
			if (this.lookAtObjectCenter)
			{
				base.LookAtTargetCenter(this.targetRenderer.gameObject);
			}
			this.currentObject = this.targetRenderer.transform;
			yield return base.CaptureFrame(this.targetRenderer.name, 0);
			int num;
			for (int i = 0; i < this.materials.Length; i = num + 1)
			{
				this.targetRenderer.material = this.materials[i];
				this.targetRenderer.materials[0] = this.materials[i];
				if (IconCreatorCanvas.instance != null)
				{
					IconCreatorCanvas.instance.SetInfo(this.materials.Length, i, this.materials[i].name, true, this.nextIconKey);
				}
				if (this.whiteCam != null)
				{
					this.whiteCam.enabled = false;
				}
				if (this.whiteCam != null)
				{
					this.blackCam.enabled = false;
				}
				if (this.mode == IconCreator.Mode.Manual)
				{
					this.CanMove = true;
					yield return new WaitUntil(() => Input.GetKeyDown(this.nextIconKey));
					this.CanMove = false;
				}
				if (IconCreatorCanvas.instance != null)
				{
					IconCreatorCanvas.instance.SetTakingPicture();
					yield return null;
					yield return null;
					yield return null;
				}
				yield return base.CaptureFrame(this.materials[i].name, i);
				num = i;
			}
			if (IconCreatorCanvas.instance != null)
			{
				IconCreatorCanvas.instance.SetInfo(0, 0, "", false, this.nextIconKey);
			}
			base.RevealInFinder();
			base.DeleteCameras();
			yield break;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0003747A File Offset: 0x0003567A
		private void Reset()
		{
			this.targetRenderer = null;
			this.materials = new Material[0];
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00037490 File Offset: 0x00035690
		protected override void Update()
		{
			if (this.preview && !this.isCreatingIcons)
			{
				if (this.targetRenderer != null)
				{
					if (this.dynamicFov)
					{
						base.UpdateFOV(this.targetRenderer.gameObject);
					}
					if (this.lookAtObjectCenter)
					{
						base.LookAtTargetCenter(this.targetRenderer.gameObject);
					}
				}
				return;
			}
			base.Update();
		}

		// Token: 0x04000D3B RID: 3387
		public Renderer targetRenderer;

		// Token: 0x04000D3C RID: 3388
		public Material[] materials;
	}
}
