using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Added NameSpace
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Concrete
{
    ///<summary>
    ///is a realisation of the ITagsRepository interface</summary>
    public class TagsRepository : ITagsRepository
    {
        ///<summary>
        ///EFDBContext object</summary>
        private readonly EFDBContext _context = new EFDBContext();
        /// <summary>
        /// Return list of all tags from the Tags table in the Database</summary>
        public IEnumerable<Tag> All()
        {
            return _context.Tags.ToList();
        }
        /// <summary>
        /// Return tag from the Tags table in the Database with specified id </summary>
        public Tag GetById(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.ID == id);
        }
        
        public void Update(Tag tagToUpdate)
        {
            _context.Entry(tagToUpdate).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(int tagIdToDelete)
        {
            var tag = _context.Tags.FirstOrDefault(t => t.ID == tagIdToDelete);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }
        }

        public Tag GetByName(string name)
        {
            return _context.Tags.FirstOrDefault(t => t.Name == name);
        }

        public Tag Add(Tag newTag)
        {
            if (null != GetByName(newTag.Name))
                return null;
            Tag t = _context.Tags.Add(newTag);
            _context.SaveChanges();
            return t;
        }
    }
}
