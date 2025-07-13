using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06000261 RID: 609 RVA: 0x0000DAC6 File Offset: 0x0000BCC6
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000DAE4 File Offset: 0x0000BCE4
	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), 5);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), 5);
		}
		float value = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(value, -0.25f, 0.25f));
		}
	}

	// Token: 0x04000281 RID: 641
	public float acceleration;

	// Token: 0x04000282 RID: 642
	public Renderer motionVectorRenderer;

	// Token: 0x04000283 RID: 643
	private Rigidbody m_Rigidbody;

	// Token: 0x0200006C RID: 108
	private static class Uniforms
	{
		// Token: 0x04000284 RID: 644
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
