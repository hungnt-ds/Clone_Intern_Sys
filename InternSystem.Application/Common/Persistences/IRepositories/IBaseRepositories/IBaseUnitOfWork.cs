namespace InternSystem.Application.Common.Persistences.IRepositories.IBaseRepositories
{
    public interface IBaseUnitOfWork
    {
        Task<int> SaveChangeAsync();
        void Dispose();
    }
}
