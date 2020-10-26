using AutoMapper;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using PozitronDev.SpecSampleApp.Core.Specifications.Filters;
using PozitronDev.SpecSampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PozitronDev.SpecSampleApp.Web
{
    public class AutomapperMaps : Profile
    {
        public AutomapperMaps()
        {
            CreateMap<BaseFilterDto, BaseFilter>().IncludeAllDerived().ReverseMap();
            CreateMap<CustomerFilterDto, CustomerFilter>().ReverseMap();
            
            CreateMap<Customer, CustomerDto>();
            CreateMap<Store, StoreDto>();
        }
    }
}
