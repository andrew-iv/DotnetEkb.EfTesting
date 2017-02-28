namespace DotnetEkb.EfTesting.Tests.Reflection
{
    public interface IMemberAccessor<in TObject, TResult> 
    {
        void SetValue(TObject destination, TResult value);
        TResult GetValue(TObject source);
    }
}