using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using StructureMap;
using WinFormsMvp.Binder;

namespace WinFormsMvp.StructureMap
{
    public class StructureMapPresenterFactory : IPresenterFactory, IDisposable
    {
        protected bool Disposed;
        IContainer container;
        readonly object registerLock = new object();
        readonly object presentersToContainersSyncLock = new object();
        readonly IDictionary<IntPtr, Type> presentersToViewTypesCache = new Dictionary<IntPtr, Type>();
        readonly object presentersToViewTypesSyncLock = new object();

        public StructureMapPresenterFactory(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (presenterType == null)
                throw new ArgumentNullException("presenterType");
            if (viewType == null)
                throw new ArgumentNullException("viewType");
            if (viewInstance == null)
                throw new ArgumentNullException("viewInstance");

            if (!container.Model.HasImplementationsFor(presenterType))
            {
                lock (registerLock)
                {
                    if (!container.Model.HasImplementationsFor(presenterType))
                    {
                        if (viewType == viewInstance.GetType())
                        {
                            viewType = FindPresenterDescribedViewTypeCached(presenterType, viewInstance);
                        }

                        container.Configure(x => x.For(viewType).Use(viewInstance));
                        container.Configure(x => x.For(presenterType).Use(presenterType).Named(presenterType.Name));
                    }
                }
            }

            return (IPresenter)container.GetInstance(presenterType);
        }

        Type FindPresenterDescribedViewTypeCached(Type presenterType, IView viewInstance)
        {
            var presenterTypeHandle = presenterType.TypeHandle.Value;

            if (!presentersToViewTypesCache.ContainsKey(presenterTypeHandle))
            {
                lock (presentersToViewTypesSyncLock)
                {
                    if (!presentersToViewTypesCache.ContainsKey(presenterTypeHandle))
                    {
                        var viewType = FindPresenterDescribedViewType(presenterType, viewInstance);
                        presentersToViewTypesCache[presenterTypeHandle] = viewType;
                        return viewType;
                    }
                }
            }

            return presentersToViewTypesCache[presenterTypeHandle];
        }

        static Type FindPresenterDescribedViewType(Type presenterType, IView viewInstance)
        {
            var genericPresenterInterface = presenterType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPresenter<>))
                .SingleOrDefault();

            if (genericPresenterInterface == null)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "There was not enough information available about the view for the StructureMapPresenterFactory to " +
                    "successfully create a presenter. The integration between WinFormsMvp and StructureMap requires more " +
                    "information about the view to support constructor based dependency injection. Either set the " +
                    "ViewType property of the [PresenterBinding], or change the presenter to implement " +
                    "IPresenter<TView>. The presenter we were trying to create was {0} and the view instance was " +
                    "of type {1}.",
                    presenterType.FullName,
                    viewInstance.GetType().FullName
                ));
            }

            return genericPresenterInterface.GetGenericArguments()[0];
        }

        public void Release(IPresenter presenter)
        {
            container.EjectAllInstancesOf<IPresenter>();

            Trace.WriteLine("Presenter released by kernel");

            var disposablePresenter = presenter as IDisposable;
            if (disposablePresenter != null)
                disposablePresenter.Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing && container != null)
            {
                container.Dispose();
                container = null;
                Disposed = true;
            }
        }

        #endregion
    }
}
