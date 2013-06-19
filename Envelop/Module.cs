namespace Envelop
{
    /// <summary>
    /// <c>Module</c> is a container for a set of bindings to be applied to the <c>Kernel</c>.
    /// </summary>
    public abstract class Module : Builder, IModule
    {
        /// <summary>
        /// The name of the module
        /// </summary>
        public virtual string Name
        {
            get { return GetType().FullName; }
        }

        /// <summary>
        /// Called to load the module bindings into the Kernel
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public void OnLoad(IKernel kernel)
        {
            this.Kernel = kernel;
            Load();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public override void AddBinding(IBinding binding)
        {
            this.Kernel.AddBinding(binding);
        }
    }
}