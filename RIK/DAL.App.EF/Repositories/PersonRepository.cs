using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;

using DAL.App.DTO;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class PersonRepository : BaseRepository<Person, Domain.App.Person, AppDbContext>,
        IPersonRepository
    {

        public PersonRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new PersonMapper(mapper))
        {
        }
        public void RemoveAllPersonsAsync(Guid id)
        {
            var query = CreateQuery();

            query = query
                .Where(x => x.EventId == id);

            foreach (var l in query)
            {

                RepoDbSet.Remove(l);
            }
        }
    }
}