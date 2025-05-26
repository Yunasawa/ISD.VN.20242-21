namespace YNL.JAMOS
{
    public interface IRefreshable
    {
        void Refresh();
    }

    public interface IRefreshable<T>
    {
        void Refresh(T input);
    }

    public interface IRefreshable<TIn, TOut>
    {
        TOut Refresh(TIn input);
    }
}