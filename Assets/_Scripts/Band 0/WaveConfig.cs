namespace Sim.Foundation
{
    public struct WaveConfig
    {
        public readonly int WaveIndex; //Also de ID.

        public readonly int EnemyAmount;

        //Enemy types, if any.
        public readonly float Radius;
        public readonly WaveEndInfo EndInfo;
        public readonly float HealthScaling;
        public readonly float DamageScaling;

        public WaveConfig(int waveIdx, int enemyAmount, float radius, WaveEndInfo endInfo, float healthScaling,
            float damageScaling)
        {
            WaveIndex = waveIdx;
            EnemyAmount = enemyAmount;
            Radius = radius;
            EndInfo = endInfo;
            HealthScaling = healthScaling;
            DamageScaling = damageScaling;
        }
    }
}