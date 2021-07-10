using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class CompanyMapper : BaseMapper<DAL.App.DTO.Company, Domain.App.Company>,
        IBaseMapper<DAL.App.DTO.Company, Domain.App.Company>
    {
        public CompanyMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}