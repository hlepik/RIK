using System;
using System.Linq;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class CompanyRepository : BaseRepository<DAL.App.DTO.Company, Domain.App.Company, AppDbContext>,
        ICompanyRepository
    {

        public CompanyRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new CompanyMapper(mapper))
        {
        }
        public void RemoveAllCompaniesAsync(Guid id)
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