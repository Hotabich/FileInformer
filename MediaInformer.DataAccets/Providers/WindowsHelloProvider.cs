namespace MediaInformer.DataAccets.Providers
{
    using System;
    using System.Threading.Tasks;
    using Windows.Security.Credentials;

    public class WindowsHelloProvider
    {
        private const string DefaultLogin = "Login";

        private static Lazy<WindowsHelloProvider> instance = new Lazy<WindowsHelloProvider>(() => new WindowsHelloProvider(), false);

        public static WindowsHelloProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }


        public async Task<bool> CheckWindowsHelloAsync()
        {
            var result = false;
            var isMicrosoftPassportActive = await this.IsMicrosoftPassportActiveAsync();
            if (isMicrosoftPassportActive)
            {
                result = await this.CreatePassportKeyAsync();
            }
            return result;
        }

        private async Task<bool> IsMicrosoftPassportActiveAsync()
        {
            return await KeyCredentialManager.IsSupportedAsync();
        }

        private async Task<bool> CreatePassportKeyAsync()
        {
            KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync(DefaultLogin, KeyCredentialCreationOption.ReplaceExisting);
            return this.IsCredentialValid(keyCreationResult);
        }

        private bool IsCredentialValid(KeyCredentialRetrievalResult keyCreation)
        {
            bool result = false;
            switch (keyCreation.Status)
            {
                case KeyCredentialStatus.Success:
                    result = true;
                    break;
                case KeyCredentialStatus.UserCanceled:
                    break;
                case KeyCredentialStatus.NotFound:
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
