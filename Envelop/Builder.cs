using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Envelop
{
    /// <summary>
    /// Base class for creating bindings and performing type resolution
    /// </summary>
    public class Builder : IBuilder
    {
        #region Fields

        private readonly List<IBinding> _bindings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder()
        {
            _bindings = new List<IBinding>();
        }

        #endregion

        #region Bind

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public virtual IKernel Kernel { get; protected set; }

        /// <summary>
        /// Binds this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IBindingTo<T> Bind<T>()
        {
            return new BindingTo<T>(this.Kernel, CreateBinding<T>());
        }

        /// <summary>
        /// Gets the bindings.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBinding> GetBindings()
        {
            return _bindings.ToArray();
        }

        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public virtual void AddBinding(IBinding binding)
        {
            _bindings.Add(binding);
        }

        #endregion

        #region Helper Methods

        private Binding<T> CreateBinding<T>()
        {
            var binding = new Binding<T>();
            AddBinding(binding);
            return binding;
        }

        #endregion
    }
}