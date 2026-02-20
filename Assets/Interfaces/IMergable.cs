namespace Merger
{
    interface IMergable<in T>
    {
        public void Merge(T first, T second);
    }
}