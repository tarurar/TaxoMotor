namespace TM.SP.AppPages.Strategies
{
    public interface IPipelineStrategy<T>: IStrategy
    {
        void Handle(T element);
    }
}
