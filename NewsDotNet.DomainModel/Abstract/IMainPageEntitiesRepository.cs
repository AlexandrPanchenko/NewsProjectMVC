using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Abstract
{
    public interface IMainPageEntitiesRepository
    {
        /// <summary>
        /// Returns all entities from the main page
        /// </summary>
        IEnumerable<MainPageEntity> All();
        /// <summary>
        /// Add a new entity which should be displayed on the main page
        /// </summary>
        /// <param name="newEntity">Enttity to be added to the main page </param>
        void Add(MainPageEntity newEntity);

        /// <summary>
        /// Adds a collection of entities which should be displayed on the main page
        /// </summary>
        /// <param name="newEntities">Collection of entities to be added to the main page</param>
        void Add(IEnumerable<MainPageEntity> newEntities);

        /// <summary>
        /// Updates a single enity on the main page
        /// </summary>
        /// <param name="entityToUpdate">Updated entity</param>
        void Update(MainPageEntity entityToUpdate);
        /// <summary>
        /// Deletes a single entity from the main page
        /// </summary>
        /// <param name="entityToDelete">Entity to be deleted from the main page</param>
        void Delete(MainPageEntity entityToDelete);

        /// <summary>
        /// Deletes from main page all entities in range
        /// </summary>
        /// <param name="entitiesToDelete">Collection with enities which should be removed from the main page</param>
        void Delete(IEnumerable<MainPageEntity> entitiesToDelete);
        /// <summary>
        /// Deletes all entities from main page
        /// </summary>
        void Clear();
         /// <summary>
         /// Clears main page and fills it with new entities
         /// </summary>
         /// <param name="newEntities">New entities to be added to main page after it is cleared</param>
        void Refill(IEnumerable<MainPageEntity> newEntities);
    }
}
