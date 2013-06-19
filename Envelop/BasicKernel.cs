using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Envelop
{
    /// <summary>
    /// The <c>Kernel</c> is the primary objet for type resolution and dependency injection.
    /// </summary>
    /// <example>
    /// TODO: provide some examples here
    /// </example>
    public class BasicKernel : Builder, IKernel
    {
        private readonly IBindingResolver _bindingResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="Kernel"/> class.
        /// </summary>
        public BasicKernel()
        {
            _bindingResolver = new DefaultBindingResolver();
        }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public override IKernel Kernel
        {
            get { return this; }
        }

        #region Load

        /// <summary>
        /// Loads the modules at the specified file paths.
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        public void Load(params string[] filePaths)
        {

            throw new NotImplementedException();

            //foreach (var filePath in filePaths)
            //{
            //    if (File.Exists(filePath))
            //    {
            //        this.Load(Assembly.LoadFile(filePath));
            //    } 
            //    else if (Directory.Exists(filePath))
            //    {
            //        foreach (var file in Directory.EnumerateFiles(filePath, "*.dll"))
            //        {
            //            this.Load(Assembly.LoadFile(filePath));
            //        }
            //    }
            //    else
            //    {
            //        //Directory.GetD
            //        throw new NotImplementedException();
            //    }
            //}
            
        }

        /// <summary>
        /// Loads modules from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void Load(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var modules = assembly.GetTypes().Where(t => t.IsAssignableFrom(typeof (IModule)));

                foreach (var module in modules.Select(m => (IModule) Activator.CreateInstance(m)))
                    this.Load(module);
            }
        }

        /// <summary>
        /// Loads the specified modules.
        /// </summary>
        /// <param name="modules">The modules.</param>
        public void Load(params IModule[] modules)
        {
            foreach (var module in modules)
                module.OnLoad(this);
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Resolves a given type based on the generic type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T Resolve<T>()
        {
            IRequest req = CreateRequest(typeof(T));
            return (T)Resolve(req);
        }

        /// <summary>
        /// Resolves a given type based on service
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Resolve(Type service)
        {
            IRequest req = CreateRequest(service);
            return Resolve(req);
        }

        /// <summary>
        /// Resolves the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        /// <exception cref="BindingNotFoundException"></exception>
        public object Resolve(IRequest req)
        {
            var binding = ResolveBindings(req).FirstOrDefault();
            if (binding == null)
                throw new BindingNotFoundException();

            return binding.Activate(req);
        }

        #endregion

        #region ResolveAll

        /// <summary>
        /// Resolves a given type based on the generic type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<T> ResolveAll<T>()
        {
            IRequest req = CreateRequest(typeof(T));
            return ResolveAll(req).Cast<T>();
        }

        /// <summary>
        /// Resolves all bindings based on the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type service)
        {
            IRequest req = CreateRequest(service);
            return ResolveAll(req);
        }

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(IRequest req)
        {
            return ResolveBindings(req).Select(b => b.Activate(req));
        }

        #endregion

        #region CanResolve

        /// <summary>
        /// Determines whether this instance can resolve the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>
        ///   <c>true</c> if this instance can resolve the specified service; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolve(Type service)
        {
            return CanResolve(CreateRequest(service));
        }

        /// <summary>
        /// Determines whether this instance can resolve the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if this instance can resolve the specified request; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool CanResolve(IRequest request)
        {
            return ResolveBindings(request).Any();
        }

        #endregion


        #region Helper Methods

        private IEnumerable<IBinding> ResolveBindings(IRequest req)
        {
            return _bindingResolver.Resolve(this.GetBindings(), req);
        }

        private Request CreateRequest(Type service)
        {
            return new Request { Resolver = this, ServiceType = service };
        }
        #endregion
    }
}
