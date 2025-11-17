using Android.Media;
using Android.OS;

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
                
                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    // Android 24+ (API 24)
                    _mediaPlayer.SetDataSource(Platform.CurrentActivity.Assets!.OpenFd(audio));
                }
                else
                {
                    // Android 21-23 fallback
                    var afd = Platform.CurrentActivity.Assets!.OpenFd(audio);
                    _mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                    afd.Close();
                }
                
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