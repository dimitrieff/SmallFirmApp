namespace SmallFirmApp.Repositories
{
    public interface IRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        //Task<T> GetByDateAsync(DateTime d);
        Task AddAsync(T entity);

        Task<IEnumerable<T>> GetAllActiveAsync();//probaaaaaaaaaaaaa
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        //Task<T> ConsInUse(int consId);

        //Task ExtraInUse(int extraId);


        //task
    }
}
