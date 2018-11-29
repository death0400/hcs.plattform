using System;
using Hcs.Platform.Flow;
using Microsoft.Extensions.DependencyInjection;
namespace Hcs.Platform.Flow
{
    public static class ServiceBundleDependencyInjectionRegister
    {
        public static void RegistFlowServiceBundle(this IServiceCollection collection)
        {
            collection.AddTransient(typeof(ServiceBundle<>));
            collection.AddTransient(typeof(ServiceBundle<,>));
            collection.AddTransient(typeof(ServiceBundle<,,>));
            collection.AddTransient(typeof(ServiceBundle<,,,>));
            collection.AddTransient(typeof(ServiceBundle<,,,,>));
            collection.AddTransient(typeof(ServiceBundle<,,,,,>));
            collection.AddTransient(typeof(ServiceBundle<,,,,,,>));
            collection.AddTransient(typeof(ServiceBundle<,,,,,,,>));
            collection.AddTransient(typeof(ServiceBundle<,,,,,,,,>));
        }
    }

    public class ServiceBundle<TService1>
    {
        private readonly TService1 service1;
        public ServiceBundle(TService1 service1)
        {
            this.service1 = service1;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TIn, TOut> method)
        {
            return method(service1, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TIn> method)
        {
            method(service1, input);
        }
    }


    public class ServiceBundle<TService1, TService2>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        public ServiceBundle(TService1 service1, TService2 service2)
        {
            this.service1 = service1;
            this.service2 = service2;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TIn, TOut> method)
        {
            return method(service1, service2, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TIn> method)
        {
            method(service1, service2, input);
        }
    }


    public class ServiceBundle<TService1, TService2, TService3>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        private readonly TService3 service3;
        public ServiceBundle(TService1 service1, TService2 service2, TService3 service3)
        {
            this.service1 = service1;
            this.service2 = service2;
            this.service3 = service3;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TService3, TIn, TOut> method)
        {
            return method(service1, service2, service3, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TService3, TIn> method)
        {
            method(service1, service2, service3, input);
        }
    }


    public class ServiceBundle<TService1, TService2, TService3, TService4>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        private readonly TService3 service3;
        private readonly TService4 service4;
        public ServiceBundle(TService1 service1, TService2 service2, TService3 service3, TService4 service4)
        {
            this.service1 = service1;
            this.service2 = service2;
            this.service3 = service3;
            this.service4 = service4;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TService3, TService4, TIn, TOut> method)
        {
            return method(service1, service2, service3, service4, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TService3, TService4, TIn> method)
        {
            method(service1, service2, service3, service4, input);
        }
    }


    public class ServiceBundle<TService1, TService2, TService3, TService4, TService5>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        private readonly TService3 service3;
        private readonly TService4 service4;
        private readonly TService5 service5;
        public ServiceBundle(TService1 service1, TService2 service2, TService3 service3, TService4 service4, TService5 service5)
        {
            this.service1 = service1;
            this.service2 = service2;
            this.service3 = service3;
            this.service4 = service4;
            this.service5 = service5;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TService3, TService4, TService5, TIn, TOut> method)
        {
            return method(service1, service2, service3, service4, service5, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TService3, TService4, TService5, TIn> method)
        {
            method(service1, service2, service3, service4, service5, input);
        }
    }


    public class ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        private readonly TService3 service3;
        private readonly TService4 service4;
        private readonly TService5 service5;
        private readonly TService6 service6;
        public ServiceBundle(TService1 service1, TService2 service2, TService3 service3, TService4 service4, TService5 service5, TService6 service6)
        {
            this.service1 = service1;
            this.service2 = service2;
            this.service3 = service3;
            this.service4 = service4;
            this.service5 = service5;
            this.service6 = service6;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TService3, TService4, TService5, TService6, TIn, TOut> method)
        {
            return method(service1, service2, service3, service4, service5, service6, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TService3, TService4, TService5, TService6, TIn> method)
        {
            method(service1, service2, service3, service4, service5, service6, input);
        }
    }


    public class ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        private readonly TService3 service3;
        private readonly TService4 service4;
        private readonly TService5 service5;
        private readonly TService6 service6;
        private readonly TService7 service7;
        public ServiceBundle(TService1 service1, TService2 service2, TService3 service3, TService4 service4, TService5 service5, TService6 service6, TService7 service7)
        {
            this.service1 = service1;
            this.service2 = service2;
            this.service3 = service3;
            this.service4 = service4;
            this.service5 = service5;
            this.service6 = service6;
            this.service7 = service7;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TIn, TOut> method)
        {
            return method(service1, service2, service3, service4, service5, service6, service7, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TIn> method)
        {
            method(service1, service2, service3, service4, service5, service6, service7, input);
        }
    }


    public class ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        private readonly TService3 service3;
        private readonly TService4 service4;
        private readonly TService5 service5;
        private readonly TService6 service6;
        private readonly TService7 service7;
        private readonly TService8 service8;
        public ServiceBundle(TService1 service1, TService2 service2, TService3 service3, TService4 service4, TService5 service5, TService6 service6, TService7 service7, TService8 service8)
        {
            this.service1 = service1;
            this.service2 = service2;
            this.service3 = service3;
            this.service4 = service4;
            this.service5 = service5;
            this.service6 = service6;
            this.service7 = service7;
            this.service8 = service8;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TIn, TOut> method)
        {
            return method(service1, service2, service3, service4, service5, service6, service7, service8, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TIn> method)
        {
            method(service1, service2, service3, service4, service5, service6, service7, service8, input);
        }
    }


    public class ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>
    {
        private readonly TService1 service1;
        private readonly TService2 service2;
        private readonly TService3 service3;
        private readonly TService4 service4;
        private readonly TService5 service5;
        private readonly TService6 service6;
        private readonly TService7 service7;
        private readonly TService8 service8;
        private readonly TService9 service9;
        public ServiceBundle(TService1 service1, TService2 service2, TService3 service3, TService4 service4, TService5 service5, TService6 service6, TService7 service7, TService8 service8, TService9 service9)
        {
            this.service1 = service1;
            this.service2 = service2;
            this.service3 = service3;
            this.service4 = service4;
            this.service5 = service5;
            this.service6 = service6;
            this.service7 = service7;
            this.service8 = service8;
            this.service9 = service9;

        }
        public TOut InvokeMethod<TOut, TIn>(TIn input, Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TIn, TOut> method)
        {
            return method(service1, service2, service3, service4, service5, service6, service7, service8, service9, input);
        }
        public void InvokeMethod<TIn>(TIn input, Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TIn> method)
        {
            method(service1, service2, service3, service4, service5, service6, service7, service8, service9, input);
        }
    }
}