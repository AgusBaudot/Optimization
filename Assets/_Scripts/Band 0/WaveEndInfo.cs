namespace Sim.Foundation
{
    public struct WaveEndInfo
    {
        public readonly WaveFinishConditionType FinishCondition;
        /// <summary>
        /// Kill based: Normalized percentage of total enemies needed to kill to advance.
        /// Time based: Time in seconds.
        /// </summary>
        public readonly float Amount;

        public WaveEndInfo(WaveFinishConditionType finishCondition, float amount)
        {
            FinishCondition = finishCondition;
            Amount = amount;
        }
    }
}