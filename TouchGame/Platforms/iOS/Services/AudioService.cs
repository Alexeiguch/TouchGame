using AVFoundation;
using Foundation;

namespace TouchGame
{
	public partial class AudioService : IAudioService, IDisposable
	{
        private AVPlayer _player;
        private AVPlayerItem _playerItem;

        public AudioService()
		{
            _player = new AVPlayer();
        }

        public void PlayAudio(string audio)
        {
            try
            {
                var directory = Path.GetDirectoryName(audio);
                var filename = Path.GetFileNameWithoutExtension(audio);
                var extension = Path.GetExtension(audio)[1..]; // Removes char '.' from extension

                var url = NSBundle.MainBundle.GetUrlForResource(filename, extension, directory);
                var asset = AVAsset.FromUrl(url);

                _playerItem = new AVPlayerItem(asset);
                _player.ReplaceCurrentItemWithPlayerItem(_playerItem);
                _player.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            if (_player != null)
            {
                _player.ReplaceCurrentItemWithPlayerItem(null);
                _player.Dispose();
                _player = null;
            }

            if (_playerItem != null)
            {
                _playerItem.Dispose();
                _playerItem = null;
            }
        }
    }
}

