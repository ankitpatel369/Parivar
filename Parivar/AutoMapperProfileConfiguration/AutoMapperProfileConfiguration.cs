using AutoMapper;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using System;
using System.Linq.Expressions;

namespace Parivar.AutoMapperProfileConfiguration
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            //CreateMap<FamilyMemberDetails, FamilyUser>();
            CreateMap<FamilyMemberDetailsModel, FamilyMemberDetails>();
            CreateMap<ContactUs, ContactUsModel>().ReverseMap();
        }

    }

    public static class MappingExpression
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
