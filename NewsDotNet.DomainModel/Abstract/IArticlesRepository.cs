using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Abstract
{
    public interface IArticlesRepository
    {
        IEnumerable<Article> All();
        Article GetById(int id);
        Article GetByAddressName(string addressName);
        void Add(Article newArticle);
        void Delete(Article articleToDelete);
        void Update(Article articleToUpdate);
    }
}
