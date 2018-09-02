namespace MediaInformer.DataAccets.Providers
{
    using MediaInformer.DI;
    using MediaInformer.Resources;
    using MediaInformer.Storage.Interfaces;
    using System;
    using System.Linq;
    using System.Globalization;
    using Windows.UI.Xaml;

    public class CultureProvider
    {
        private const string CultureInfoKey = "Language";

        private static readonly CultureInfo[] suportedLanguage =
            {
            new CultureInfo("en"),
            new CultureInfo("ru"),
            new CultureInfo("uk")
        };
        private readonly IStorageProvider storageProvider = Factory.CommonFactory.GetInstance<IStorageProvider>();
        private LocalizedStrings localizedStrings = Application.Current.Resources["LocalizedStrings"] as LocalizedStrings;

        private static readonly Lazy<CultureProvider> instance = new Lazy<CultureProvider>(() => new CultureProvider(), false);

        public static CultureProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public static CultureInfo[] SupportedLanguage => suportedLanguage;

        public string CurrentCulture
        {
            get
            {

                return storageProvider.ReadFromSettings<string>(CultureInfoKey);
            }
            set
            {
                this.storageProvider.WriteToSettings(CultureInfoKey, value);
            }
        }

        public void ApplyCulture()
        {
            var cultureInfo = new CultureInfo(this.CurrentCulture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            this.localizedStrings.RefreshLanguageSettings();
        }

        public void CheckCulture()
        {
            var culture = storageProvider.ReadFromSettings<string>(CultureInfoKey);
            if (culture == null)
            {
                this.CurrentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }
            this.ApplyCulture();
        }
    }
}
