using System;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>, IPersonRepositoryCustom<Person>
    {

    }
    public interface IPersonRepositoryCustom<TEntity>
    {

        void RemoveAllPersonsAsync(Guid id);
    }
}