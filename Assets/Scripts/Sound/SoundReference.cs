namespace Sound
{
    public class SoundReference
    {
        // BGM
        public static readonly SoundReference BackgroundMusic = new SoundReference("Sound/BackgroundMusic");
        public static readonly SoundReference BamBoo = new SoundReference("Sound/BamBoo");
        
        // SFX
        public static readonly SoundReference BillSFX = new SoundReference("Sound/BillSFX");
        public static readonly SoundReference ButtonClickSFX = new SoundReference("Sound/ButtonClickSFX");
        public static readonly SoundReference GameOverSFX = new SoundReference("Sound/GameOverSFX");
        public static readonly SoundReference ScoreSFX = new SoundReference("Sound/ScoreSFX");
        public static readonly SoundReference SwipeSFX = new SoundReference("Sound/SwipeSFX");

        // Legacy
        public static readonly SoundReference GameOver = new SoundReference("SFX/GameOver");

        
        private readonly string path;

        public string Path => path;
        
        public SoundReference(string path)
        {
            this.path = path;
        }

        public static implicit operator string(SoundReference reference)
        {
            return reference.path;
        }
    }
}