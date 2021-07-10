using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;


namespace Contracts.DAL.App.Repositories
{
    public interface IEventRepository: IBaseRepository<Event>, IEventRepositoryCustom<Event>
    {

    }
    public interface IEventRepositoryCustom<TEntity>
    {

        Task<IEnumerable<TEntity?>> GetAllComingEvents();
        Task<IEnumerable<TEntity?>> GetAllPreviousEvents();
        List<Participant> GetAllParticipants(Guid id);
    }
}