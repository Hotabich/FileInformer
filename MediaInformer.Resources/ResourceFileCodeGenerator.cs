
namespace MediaInformer.Resources
{
	using GalaSoft.MvvmLight;
	using System.Globalization;
	using MediaInformer.Resources.Core;
	using System;

	/// <summary>
    /// Provides access to resources from CommonResources.resw file.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "Yet it will be unstatic.")]
	public class CommonResources
	{
		/// <summary>
        /// Contains logic for accessing contsnt of resource file.
        /// </summary>
		private static ResourceProvider resourceProvider = new ResourceProvider("MediaInformer.Resources/CommonResources");
		
		/// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        public static CultureInfo Culture
        {
            get
            {
                return resourceProvider.OverridedCulture;
            }
            set
            {
                resourceProvider.OverridedCulture = value;
            }
        }

		/// <summary>
        /// Gets a localized string similar to "About"
        /// </summary>
		public static string AboutText
        {
            get
            {
                return resourceProvider.GetString("AboutText");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Language"
        /// </summary>
		public static string LanguageText
        {
            get
            {
                return resourceProvider.GetString("LanguageText");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Description"
        /// </summary>
		public static string NavigationPanelDescriptionButtonText
        {
            get
            {
                return resourceProvider.GetString("NavigationPanelDescriptionButtonText");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Favorite"
        /// </summary>
		public static string NavigationPanelFavoriteButtonText
        {
            get
            {
                return resourceProvider.GetString("NavigationPanelFavoriteButtonText");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Main"
        /// </summary>
		public static string NavigationPanelMainButtonText
        {
            get
            {
                return resourceProvider.GetString("NavigationPanelMainButtonText");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Recent"
        /// </summary>
		public static string NavigationPanelRecentButtonText
        {
            get
            {
                return resourceProvider.GetString("NavigationPanelRecentButtonText");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Settings"
        /// </summary>
		public static string SettingsText
        {
            get
            {
                return resourceProvider.GetString("SettingsText");
            }
        }

	}


	public sealed class LocalizedStrings : ObservableObject
    {
		/// <summary>
        /// Initialize a new instance of <see cref="LocalizedStrings"/> class.
        /// </summary>
        public LocalizedStrings()
        {
            this.RefreshLanguageSettings();
        }

		public static event EventHandler LocalizedStringsRefreshedEvent;

		public void OnLocalizedStringsRefreshedEvent()
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler handler = LocalizedStringsRefreshedEvent;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, new EventArgs());
            }
        }

		        /// <summary>
		/// Gets resources that are common across application.
		/// </summary>
		public CommonResources CommonResources { get; private set; }

	
		public void RefreshLanguageSettings()
        {
			            this.CommonResources = new CommonResources();
			this.RaisePropertyChanged("CommonResources");
		

			OnLocalizedStringsRefreshedEvent();
		}
	}
}