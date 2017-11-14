using System;
using System.Media;

namespace ConsoleApp1
{
    class Sound
    {        
        public static void Play(int selection)
        {
            if (selection == 3)
            {
                SoundPlayer player = new SoundPlayer();
                player.Stop();
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @".\\jupalt.wav";
                player.Play();
            }
            else if (selection == 2)
            {
                SoundPlayer player = new SoundPlayer();
                player.Stop();
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @".\\hallking.wav";
                player.PlayLooping();
            }
            else if (selection == 1)
            {
                SoundPlayer player = new SoundPlayer();
                player.Stop();
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @".\\elitet.wav";
                player.PlayLooping();
            }
        }
    }
}
