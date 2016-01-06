using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Entities;
using NewsDotNet.WebUI.Areas.Admin.Models;

namespace NewsDotNet.WebUI.Areas.Admin.Mappers
{
    public class CommonMapper : IMapper
    {
        static CommonMapper()
        {
            Mapper.CreateMap<Article, ArticleViewModel>();
            Mapper.CreateMap<ArticleViewModel, Article>();
            Mapper.CreateMap<CreateUserModel, User>();
            Mapper.CreateMap<User, UserListViewModel>().ForMember(model => model.Role, opt => opt.Ignore());
            Mapper.CreateMap<User, EditProfileViewModel>();
            Mapper.CreateMap<Article, JqGridArticleModel>();
        }

        public TDestinationType Map<TSourceType, TDestinationType>(TSourceType source)
        {
            return Mapper.Map<TDestinationType>(source);
        }
    }
}