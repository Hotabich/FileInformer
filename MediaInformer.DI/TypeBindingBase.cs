namespace MediaInformer.DI
{
    internal abstract class TypeBindingBase
    {
        internal LifetimeMode Mode { get; set; }

        internal abstract object GetRealInstance();
    }
}
