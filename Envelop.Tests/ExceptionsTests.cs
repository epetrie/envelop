﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using NUnit.Framework;

namespace Envelop.Tests
{
    [TestFixture]
    class ExceptionsTests
    {
        [Test, ExpectedException(typeof(IncompleteBindingException))]
        public void IncompleteBindingException()
        {
            var kernel = new BasicKernel();
            kernel.Bind<ISomeInterface>();

            kernel.Resolve<ISomeInterface>();
        }

        [Test, ExpectedException(typeof(BindingNotFoundException))]
        public void BindingNotFoundException()
        {
            var kernel = new BasicKernel();
            kernel.Resolve<ISomeInterface>();
        }
    }
}
