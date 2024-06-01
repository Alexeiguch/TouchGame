namespace TouchGame
{
	public class VibrationEffects
	{
		public static void Vibrate(int duration)
		{
			if (Vibration.Default.IsSupported)
			{
				Vibration.Default.Vibrate(duration);
			}
		}

		public static void PerformHaptic(HapticFeedbackType type)
		{
			if (HapticFeedback.Default.IsSupported)
			{
				HapticFeedback.Default.Perform(type);
			}
		}
	}
}

