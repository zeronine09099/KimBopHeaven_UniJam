namespace Sound
{
    public class SoundReference
    {
        // BGM
        // public static readonly SoundReference MainTitleBGM = new SoundReference("BGM/MainTitleBGM_V1");
        
        // SFX

        public static readonly SoundReference BackgroundMusic = new SoundReference("Sound/BackgroundMusic");
        public static readonly SoundReference ButtonClick = new SoundReference("Sound/ButtonClickSFX");
        public static readonly SoundReference BamBoo = new SoundReference("Sound/BamBooShoot");
        public static readonly SoundReference Bill = new SoundReference("Sound/BillSFX");
        public static readonly SoundReference GameOver = new SoundReference("Sound/GameOverSFX");

        
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