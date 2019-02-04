using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Ninject;
using System;
using System.Diagnostics;
using WinFormsMvp.Binder;

namespace WinFormsMvp.Ninject
{
    public class NinjectPresenterFactory : IPresenterFactory, IDisposable
    {
        /// <summary>
        /// Ninject kernel 
        /// </summary>
        protected IKernel Kernel;
        protected bool Disposed;

        readonly Dictionary<IntPtr, Type> _presentersToViewTypesCache = new Dictionary<IntPtr, Type>();
        readonly object _presentersToViewTypesSyncLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectPresenterFactory"/> class.
        /// </summary>
        /// <param name="kernel">The <see cref="IKernel"/> to use within the presenter factory</param>
        public NinjectPresenterFactory(IKernel kernel)
        {
            if (kernel == null) 
                throw new ArgumentNullException("kernel");

            Kernel = kernel;
        }

        #region IPresenterFactory Members

        /// <summary>
        /// Creates the specified presenter type.
        /// </summary>
        /// <param name="presenterType">Type of the presenter.</param>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="viewInstance">The view instance.</param>
        /// <returns></returns>
        public virtual IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (presenterType == null)
                throw new ArgumentNullException("presenterType");
            if (viewType == null)
                throw new ArgumentNullException("viewType");
            if (viewInstance == null)
                throw new ArgumentNullException("viewInstance");

            if (viewType == viewInstance.GetType())
            {
                viewType = FindPresenterDescribedViewTypeCached(presenterType, viewInstance);
            }

            Kernel.Bind(viewType).ToMethod(ctx => viewInstance).InTransientScope();
            Kernel.Bind<IPresenter>().To(presenterType).InTransientScope();

            var presenter = Kernel.Get<IPresenter>();

            // Not the most elegant or efficient solution, but it beats having to manually map all 
            // of your presenters to views.
            Kernel.Unbind<IView>();
            Kernel.Unbind<IPresenter>();

            return presenter;
        }

        Type FindPresenterDescribedViewTypeCached(Type presenterType, IView viewInstance)
        {
            var presenterTypeHandle = presenterType.TypeHandle.Value;

            if (!_presentersToViewTypesCache.ContainsKey(presenterTypeHandle))
            {
                lock (_presentersToViewTypesSyncLock)
                {
                    if (!_presentersToViewTypesCache.ContainsKey(presenterTypeHandle))
                    {
                        var viewType = FindPresenterDescribedViewType(presenterType, viewInstance);
                        _presentersToViewTypesCache[presenterTypeHandle] = viewType;
                        return viewType;
                    }
                }
            }

            return _presentersToViewTypesCache[presenterTypeHandle];
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
                    "There was not enough information available about the view for the NinjectPresenterFactory to " +
                    "successfully create a presenter. The integration between WinFormsMvp and Ninject requires more " +
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

        /// <summary>
        /// Releases the specified presenter.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        public virtual void Release(IPresenter presenter)
        {
            var released = Kernel.Release(presenter);

            Trace.WriteLine(string.Format("Presenter instance found and released by kernel {0}", released));

            var disposablePresenter = presenter as IDisposable;
            if (disposablePresenter != null)
                disposablePresenter.Dispose();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(Disposed)
                return;

            if (disposing && Kernel != null)
            {
                Kernel.Dispose();
                Kernel = null;
                Disposed = true;
            }
        }

        #endregion

    }
}
