using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class OscillateLightBrightness : MonoBehaviour
{
	// Token: 0x06000256 RID: 598 RVA: 0x0000D830 File Offset: 0x0000BA30
	private void Start()
	{
		this.lightComponent = base.GetComponent<Light>();
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000D83E File Offset: 0x0000BA3E
	private void Update()
	{
		this.lightComponent.intensity = UnityEngine.Random.Range(this.lower, this.upper);
	}

	// Token: 0x04000275 RID: 629
	private Light lightComponent;

	// Token: 0x04000276 RID: 630
	[SerializeField]
	[Range(0f, 10f)]
	private float lower;

	// Token: 0x04000277 RID: 631
	[SerializeField]
	[Range(0f, 10f)]
	private float upper;
}
