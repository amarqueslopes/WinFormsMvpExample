using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.Unity;
using WinFormsMvp.Binder;

namespace WinFormsMvp.Unity
{
    public class UnityPresenterFactory : IPresenterFactory, IDisposable
    {
        IUnityContainer container;

        readonly IDictionary<IPresenter, IUnityContainer> presentersToContainers = new Dictionary<IPresenter, IUnityContainer>();
        readonly object presentersToContainersSyncLock = new object();

        readonly IDictionary<IntPtr, Type> presentersToViewTypesCache = new Dictionary<IntPtr, Type>();
        readonly object presentersToViewTypesSyncLock = new object();
        private bool Disposed;

        public UnityPresenterFactory(IUnityContainer container)
        {
            if (container == null) 
                throw new ArgumentNullException("container");

            this.container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            // If a PresenterBinding attribute is applied directly to a view and the ViewType
            // property is not explicitly set, it will be defaulted to the view type. 
            // If we register it into the container using this type, and then try and resolve
            // a presenter like Presenter<IView> unity will throw an exception because it
            // doesn't use covariance when resolving dependencies. To get around this, we
            // look at the generic type argument on the presenter type (in this case, IView)
            // and register the view instance against that instead.
            if (viewType == viewInstance.GetType())
            {
                viewType = FindPresenterDescribedViewTypeCached(presenterType, viewInstance);
            }

            var presenterScopedContainer = container.CreateChildContainer();
            presenterScopedContainer.RegisterInstance(viewType, viewInstance);

            var presenter = (IPresenter)presenterScopedContainer.Resolve(presenterType);

            lock (presentersToContainersSyncLock)
            {
                presentersToContainers[presenter] = presenterScopedContainer;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            var presenterScopedContainer = presentersToContainers[presenter];

            lock (presentersToContainersSyncLock)
            {
                presentersToContainers.Remove(presenter);
            }

            using (presenterScopedContainer)
            {
                presenterScopedContainer.Teardown(presenter);
            }

            Trace.WriteLine("Presenters released by Unity container");

            var disposable = presenter as IDisposable;

            if (disposable != null)
                disposable.Dispose();
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
                    "There was not enough information available about the view for the UnityPresenterFactory to " +
                    "successfully create a presenter. The integration between WebFormsMvp and Unity requires more " +
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
