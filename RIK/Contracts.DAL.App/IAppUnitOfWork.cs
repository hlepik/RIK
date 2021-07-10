using System;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IEventRepository Event { get; }
        ICompanyRepository Company { get; }
        IPersonRepository Person { get; }

    }
}