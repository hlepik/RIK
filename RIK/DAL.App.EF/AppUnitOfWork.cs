using System;
using System.Collections.Generic;
using AutoMapper;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Repositories;
using DAL.App.EF.Repositories;
using DAL.Base.EF;
using DAL.Base.EF.Repositories;
using Domain.App;

namespace DAL.App.EF
{
    public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
    {
        protected IMapper Mapper;
        public AppUnitOfWork(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
        {
            Mapper = mapper;
        }

        public ICompanyRepository Company => GetRepository(() => new CompanyRepository(UowDbContext, Mapper));
        public IPersonRepository Person => GetRepository(() => new PersonRepository(UowDbContext, Mapper));
        public IEventRepository Event => GetRepository(() => new EventRepository(UowDbContext, Mapper));


    }
}