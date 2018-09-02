namespace MediaInformer.ViewModels
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight.Ioc;

    public class ViewModelsLocator
    {
        static ViewModelsLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<NavigationMenuViewModel>();

        }
        public static NavigationMenuViewModel NavigationMenuViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<NavigationMenuViewModel>();
            }
        }
    }
}
