namespace TouchGame
{
	public class BaseViewModel : ObservableObject
	{
		private string _title;
        private bool _isLoading;

		public BaseViewModel()
		{
		}

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public virtual void Initialize() { }

        protected async Task<bool> ExecuteBlocking(Func<Task> func, string title = null, string message = null, string okButton = null, bool showLoadingWithoutDelay = false)
        {
            var task = func();

            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                {
                    //DialogServices.DisplayAlertAsync(title ?? AppResources.StringSomethingWrongHere, message ?? AppResources.StringSomethingWrongHere, okButton ?? AppResources.StringOK).Forget();

                    //CrashesHelper.TrackError(task.Exception);

                    return false;
                }

                return true;
            }

            try
            {
                IsLoading = true;
                //ShowLoader(showLoadingWithoutDelay);

                await task;

                return true;
            }
            catch (Exception ex)
            {
                //DialogServices.DisplayAlertAsync(title ?? AppResources.StringSomethingWrongHere, message ?? AppResources.StringSomethingWrongHere, okButton ?? AppResources.StringOK).Forget();

                //CrashesHelper.TrackError(ex);

                return false;
            }
            finally
            {
                //await DialogServices.HideLoader();
                IsLoading = false;
            }
        }
    }
}

