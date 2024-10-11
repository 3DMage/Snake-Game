using Microsoft.Xna.Framework.Audio;

namespace GameComponents
{
    // Manages audio assets and their state.
    public static class AudioManager
    {
        //? MUSIC ==================================================================================================
        public static SoundEffect src_gameMusic;
        public static SoundEffect src_menuMusic;

        public static SoundEffectInstance gameMusic { get; private set; }
        public static SoundEffectInstance menuMusic { get; private set; }
        //? MUSIC ==================================================================================================


        //? SOUND EFFECTS ==========================================================================================
        public static SoundEffect src_moveSelectBeep;
        public static SoundEffect src_selectBeep;
        public static SoundEffect src_rocketRumble;
        public static SoundEffect src_explosion;
        public static SoundEffect src_alert;
        public static SoundEffect src_messageBeep;
        public static SoundEffect src_success;
        public static SoundEffect src_pop;
        public static SoundEffect src_damageBig;
        public static SoundEffect src_gameOver;


        public static SoundEffectInstance rocketRumble { get; private set; }
        public static SoundEffectInstance alert { get; private set; }
        //? SOUND EFFECTS ==========================================================================================














        public static void Initialize()
        {

        }

        // Configures sound and music assets.
        public static void ConfigureAudio()
        {
            //Music setup
            gameMusic.IsLooped = true;
            gameMusic.Volume = 0.08f;

            menuMusic.IsLooped = true;
            menuMusic.Volume = 0.08f;

            // Sound setup
            rocketRumble.IsLooped = true;
            rocketRumble.Volume = 0.96f;

            alert.IsLooped = true;
        }

        // Load music and sound assets.
        public static void LoadAudio()
        {
            // Music
            src_gameMusic = ContentManagerHandle.Content.Load<SoundEffect>("audio/gameMusic");
            src_menuMusic = ContentManagerHandle.Content.Load<SoundEffect>("audio/menuMusic");

            gameMusic = src_gameMusic.CreateInstance();
            menuMusic = src_menuMusic.CreateInstance();

            // Sound effects.
            src_moveSelectBeep = ContentManagerHandle.Content.Load<SoundEffect>("audio/moveSelection");
            src_selectBeep = ContentManagerHandle.Content.Load<SoundEffect>("audio/selection");
            src_rocketRumble = ContentManagerHandle.Content.Load<SoundEffect>("audio/rocket");
            src_explosion = ContentManagerHandle.Content.Load<SoundEffect>("audio/explosion");
            src_alert = ContentManagerHandle.Content.Load<SoundEffect>("audio/alarm");
            src_messageBeep = ContentManagerHandle.Content.Load<SoundEffect>("audio/messageBeep");
            src_success = ContentManagerHandle.Content.Load<SoundEffect>("audio/success");
            src_pop = ContentManagerHandle.Content.Load<SoundEffect>("audio/pop");
            src_damageBig = ContentManagerHandle.Content.Load<SoundEffect>("audio/damage_big");
            src_gameOver = ContentManagerHandle.Content.Load<SoundEffect>("audio/gameOver");

            rocketRumble = src_rocketRumble.CreateInstance();
            alert = src_alert.CreateInstance();
        }
    }
}