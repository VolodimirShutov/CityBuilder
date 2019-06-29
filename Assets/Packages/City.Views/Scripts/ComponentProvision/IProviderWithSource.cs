namespace City.Views
{
    public interface IProviderWithSource<TSource>: IComponentProvider
        where TSource: class
    {
        void InitSource(TSource source);
    }
}