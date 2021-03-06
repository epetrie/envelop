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
    public class InjectionTests
    {
        [Test]
        public void InjectionTest()
        {
            var kernel = new Kernel();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

            var t1 = kernel.Resolve<IAnotherInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterface);
        }

        [Test]
        public void MultiInjectionArrayTest()
        {
            var kernel = new Kernel();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
            kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation>();

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.AreEqual(2, t1.SomeInterfaces.Length);
            Assert.That(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
            Assert.That(t1.SomeInterfaces[1] is SomeInterfaceImplementation2);
        }

        [Test]
        public void MultiInjectionIEnumerableTest()
        {
            var kernel = new Kernel();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
            kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation2>();

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.AreEqual(2, t1.SomeInterfaces.Length);
            Assert.That(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
            Assert.That(t1.SomeInterfaces[1] is SomeInterfaceImplementation2);
        }

        [Test]
        public void MultiInjectionListTest()
        {
            var kernel = new Kernel();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
            kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation3>();

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.AreEqual(2, t1.SomeInterfaces.Length);
            Assert.That(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
            Assert.That(t1.SomeInterfaces[1] is SomeInterfaceImplementation2);
        }
    }
}
