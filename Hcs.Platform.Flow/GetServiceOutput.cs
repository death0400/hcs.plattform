namespace Hcs.Platform.Flow
{
    public interface IGetServiceOutput<out T, out TService>
    {
        T Output { get; }
        TService Service { get; }
    }
    public class GetServiceOutput<T, TService> : IGetServiceOutput<T, TService>
    {
        internal GetServiceOutput(T output, TService service)
        {
            Output = output;
            Service = service;
        }

        public T Output { get; }
        public TService Service { get; }
    }

}