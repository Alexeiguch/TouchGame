using Android.Media;

namespace TouchGame
{
    public partial class AudioService : IAudioService, IDisposable
    {
        private MediaPlayer _mediaPlayer;

        public void PlayAudio(string audio)
        {
            Dispose();

            try
            {
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Prepared += MediaPlayerOnPrepared;
                _mediaPlayer.SetDataSource(Platform.CurrentActivity.Assets!.OpenFd(audio));
                _mediaPlayer.PrepareAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void MediaPlayerOnPrepared(object sender, EventArgs e)
        {
            _mediaPlayer.Start();
        }

        public void Dispose()
        {
            _mediaPlayer?.Release();
            _mediaPlayer?.Dispose();
            _mediaPlayer = null;
        }
    }
}

