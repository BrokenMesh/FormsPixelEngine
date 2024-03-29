﻿using FormsPixelEngine.FPE.Asset;
using System.Media;

namespace FormsPixelEngine.FPE.Sound
{

    /*! \class AudioClip
    \brief Represents a sound bit loaded form a audio file. 
    */
    public class AudioClip : IAsset
    {
        private SoundPlayer simpleSound;

        //! Constructor 
        /*!
          \param _filename filepath of the sound file
        */
        public AudioClip(string _filename) {
            simpleSound = new SoundPlayer(_filename);
        }

        //! Play the audio clip once.
        /*!
        */
        public void Play() {
            simpleSound.Play();
        }

        //! Play the audio clip looping
        /*!
        */
        public void PlayLoop() { 
            simpleSound.PlayLooping();
        }

        //! Stop the audio clip form playing
        /*!
        */
        public void Stop() {
            simpleSound.Stop();
        }
    }
}
 