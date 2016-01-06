using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Concrete
{
    public class ArticleRepository:IArticlesRepository
    {
        private readonly EFDBContext _articleСontext =new EFDBContext();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Article> All()
        {
            return _articleСontext.Articles;
        }
        public Article GetById(int id)
        {
            return _articleСontext.Articles.FirstOrDefault(t => t.Id == id);
        }
        public Article GetByAddressName(string addressName)
        {
            return _articleСontext.Articles.FirstOrDefault(a => a.AddressName == addressName);
        }

        


        public void Add(Article newArticle)
        {
            var tags = new List<Tag>(newArticle.Tags);
            newArticle.Tags.Clear();
            foreach (var tag in tags)
            {
                newArticle.Tags.Add(_articleСontext.Tags.FirstOrDefault(t => t.ID == tag.ID));
            }
            _articleСontext.Articles.Add(newArticle);
            _articleСontext.SaveChanges();
        }
        public void Update(Article articleToUpdate)
        {
            var existingArticle = _articleСontext.Articles.FirstOrDefault(a => a.Id == articleToUpdate.Id);
            var deletedTags = existingArticle.Tags.Except(articleToUpdate.Tags, new TagEqualityComparer()).ToList<Tag>();
            var addedTags = articleToUpdate.Tags.Except(existingArticle.Tags, new TagEqualityComparer()).ToList<Tag>();

            existingArticle.Title = articleToUpdate.Title;
            existingArticle.TitleImagePath = articleToUpdate.TitleImagePath;
            existingArticle.AddressName = articleToUpdate.AddressName;
            existingArticle.Body = articleToUpdate.Body;
            existingArticle.LastChangedTime = articleToUpdate.LastChangedTime;
            existingArticle.State = articleToUpdate.State;
            existingArticle.AuthorId = articleToUpdate.AuthorId;

            deletedTags.ForEach(t => existingArticle.Tags.Remove(t));

            foreach (var tag in addedTags)
            {
                var t = _articleСontext.Tags.FirstOrDefault(tg => tg.ID == tag.ID);
                existingArticle.Tags.Add(t);
            }

            _articleСontext.SaveChanges();
        }

        public void Delete(Article articleToDelete)
        {
            _articleСontext.Entry(articleToDelete).State = System.Data.Entity.EntityState.Deleted;
            _articleСontext.SaveChanges();
        }
    }
}
