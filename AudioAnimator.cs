using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class AudioAnimator : MonoBehaviour
{
	// Token: 0x060009B0 RID: 2480 RVA: 0x00035082 File Offset: 0x00033282
	private void Start()
	{
		if (!this.didInitBaseVolume)
		{
			this.InitBaseVolume();
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00035094 File Offset: 0x00033294
	private void InitBaseVolume()
	{
		for (int i = 0; i < this.targets.Length; i++)
		{
			this.targets[i].baseVolume = this.targets[i].audioSource.volume;
		}
		this.didInitBaseVolume = true;
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x000350E2 File Offset: 0x000332E2
	public void UpdateValue(float value, bool ignoreSmoothing = false)
	{
		this.UpdatePitchAndVolume(value, value, ignoreSmoothing);
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x000350F0 File Offset: 0x000332F0
	public void UpdatePitchAndVolume(float pitchValue, float volumeValue, bool ignoreSmoothing = false)
	{
		if (!this.didInitBaseVolume)
		{
			this.InitBaseVolume();
		}
		for (int i = 0; i < this.targets.Length; i++)
		{
			AudioAnimator.AudioTarget audioTarget = this.targets[i];
			float num = audioTarget.pitchCurve.Evaluate(pitchValue);
			float num2 = Mathf.Pow(1.05946f, num);
			audioTarget.audioSource.pitch = num2;
			float num3 = audioTarget.volumeCurve.Evaluate(volumeValue);
			float volume = audioTarget.audioSource.volume;
			float num4 = audioTarget.baseVolume * num3;
			if (ignoreSmoothing)
			{
				audioTarget.audioSource.volume = num4;
			}
			else if (volume > num4)
			{
				audioTarget.audioSource.volume = Mathf.MoveTowards(audioTarget.audioSource.volume, audioTarget.baseVolume * num3, (1f - audioTarget.lowerSmoothing) * audioTarget.baseVolume * Time.deltaTime * 90f);
			}
			else
			{
				audioTarget.audioSource.volume = Mathf.MoveTowards(audioTarget.audioSource.volume, audioTarget.baseVolume * num3, (1f - audioTarget.riseSmoothing) * audioTarget.baseVolume * Time.deltaTime * 90f);
			}
		}
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x000026E9 File Offset: 0x000008E9
	public AudioAnimator()
	{
	}

	// Token: 0x04000B7E RID: 2942
	private bool didInitBaseVolume;

	// Token: 0x04000B7F RID: 2943
	[SerializeField]
	private AudioAnimator.AudioTarget[] targets;

	// Token: 0x02000179 RID: 377
	[Serializable]
	private struct AudioTarget
	{
		// Token: 0x04000B80 RID: 2944
		public AudioSource audioSource;

		// Token: 0x04000B81 RID: 2945
		public AnimationCurve pitchCurve;

		// Token: 0x04000B82 RID: 2946
		public AnimationCurve volumeCurve;

		// Token: 0x04000B83 RID: 2947
		[NonSerialized]
		public float baseVolume;

		// Token: 0x04000B84 RID: 2948
		public float riseSmoothing;

		// Token: 0x04000B85 RID: 2949
		public float lowerSmoothing;
	}
}
