namespace MediaInformer.DI
{
    using System;
    using System.Collections.Generic;

    public class Factory
    {
        private readonly Dictionary<Type, TypeBindingBase> typeBindings = new Dictionary<Type, TypeBindingBase>();

        private readonly object syncLock = new object();

        static Factory()
        {
            CommonFactory = new Factory();
        }
        public static Factory CommonFactory { get; private set; }

        public void Initialize(FactoryInitializer initializer)
        {
            // Guard.CheckIsNotNull(initializer, "initializer");
            initializer.SetBindings(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Type parameter passed to TypeBinding class.")]
        public void Bind<TSource, TReal>(LifetimeMode mode) where TReal : TSource, new()
        {
            lock (syncLock)
            {
                var binding = new TypeBinding<TSource, TReal>() { Mode = mode };

                var sourceType = typeof(TSource);

                this.typeBindings[sourceType] = binding;
            }
        }
        public void Remove<TSource>()
        {
            lock (syncLock)
            {
                var sourceType = typeof(TSource);

                if (this.typeBindings.ContainsKey(sourceType))
                {
                    this.typeBindings.Remove(sourceType);
                }
            }
        }
        public TSource GetInstance<TSource>()
        {
            TypeBindingBase binding = null;

            lock (syncLock)
            {
                var sourceType = typeof(TSource);
                var isBindingExist = this.typeBindings.TryGetValue(sourceType, out binding);

                if (!isBindingExist)
                {
                    var errorMessage = $"Type {sourceType.FullName} was not bound to any real type. Call Bind method for this type.";
                    throw new InvalidOperationException(errorMessage);
                }
            }

            return (TSource)binding.GetRealInstance();
        }
        public bool IsBindingExist(Type checkedType)
        {
            if (checkedType == null)
            {
                throw new ArgumentNullException(nameof(checkedType));
            }

            var result = false;

            lock (syncLock)
            {
                result = this.typeBindings.ContainsKey(checkedType);
            }

            return result;
        }
    }
}
