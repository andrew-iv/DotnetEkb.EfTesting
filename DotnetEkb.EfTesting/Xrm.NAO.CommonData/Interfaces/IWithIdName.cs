namespace DotnetEkb.EfTesting.CommonData.Interfaces
{
    public interface IWithIdName : IWithId
    {
        string Name { get; set; }
    }
}