namespace Dajunctic
{
    public class TacticianStats
    {
        public IStat PlayerHealth {get; }
        public IStat Level {get; }
        public IStat Experience {get; }
        public IStat Gold {get; }

        public TacticianStats()
        {
            PlayerHealth = new BaseStat(100);
            Level = new BaseStat(1);
            Gold = new BaseStat(0);
            Experience = new BaseStat(0);
        }
    }
}