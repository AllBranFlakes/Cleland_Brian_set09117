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
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @".\\sa8B.WAV";
                player.Play();
            }
            else if (selection == 2)
            {
                SoundPlayer player = new SoundPlayer();
                player.Stop();
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @".\\4mtking.WAV";
                player.PlayLooping();
            }
            else if (selection == 1)
            {
                SoundPlayer player = new SoundPlayer();
                player.Stop();
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @".\\on9C.WAV";
                player.PlayLooping();
            }
        }
    }
}
