using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Added NameSpace
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Abstract
{
    ///<summary>
    ///is an interface for work with the Tags table in the Database</summary>
    public interface ITagsRepository
    {
        /// <summary>
        /// Get List of all tags from the Tags table in the Database</summary>
        IEnumerable<Tag> All(); 
        /// <summary>
        /// Get the tag from the Tags table in the Database with specified id</summary>
        Tag GetById(int id);
        /// <summary>
        /// Get the tag from the Tags table in the Database with specified name</summary>
        Tag GetByName(string name);

        /// <summary>
        /// Add new tag to the Tags table
        /// </summary>
        Tag Add(Tag newTag);
        void Update(Tag tagToUpdate);
        void Delete(int tagIdToDelete);
    }
}
