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
    public class EventRepository : BaseRepository<DAL.App.DTO.Event, Domain.App.Event, AppDbContext>,
        IEventRepository
    {

        public EventRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new EventMapper(mapper))
        {
        }

        public async Task<IEnumerable<DAL.App.DTO.Event?>> GetAllComingEvents()
        {
            var query = CreateQuery();


            query = query.Where(x => x.EventDate >= DateTime.Today).OrderBy(x => x.EventDate);

            var res = await query.Select(x => Mapper.Map(x)).ToListAsync();

            return res!;

        }

        public async Task<IEnumerable<DAL.App.DTO.Event?>> GetAllPreviousEvents()
        {
            var query = CreateQuery();


            query = query.Where(x => x.EventDate < DateTime.Today).OrderBy(x => x.EventDate).Reverse();

            var res = await query.Select(x => Mapper.Map(x)).ToListAsync();

            return res!;

        }

        public List<Participant> GetAllParticipants(Guid id)

        {
            var query = CreateQuery();

             query = query
                .Include(c => c.Persons)
                .Include(c => c.Companies)
                .Where(c => c.Id == id);


            var result = new List<Participant>();
            foreach (var each in query)
            {

                foreach (var person in each.Persons!)
                {
                    result.Add(new Participant()
                    {
                        Id = person.Id,
                        Name = person.FirstName + " " + person.LastName,
                        Code = person.IdentificationCode
                    });
                }
                foreach (var company in each.Companies!)
                {
                    result.Add(new Participant()
                    {
                        Id = company.Id,
                        Name = company.CompanyName,
                        Code = company.RegisterCode
                    });
                }
            }

            result = result.OrderBy(x => x.Name).ToList();
            return result;
        }


    }
}