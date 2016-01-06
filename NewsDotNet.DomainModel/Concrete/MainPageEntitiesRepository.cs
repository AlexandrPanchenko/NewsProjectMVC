using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsDotNet.DomainModel.Abstract;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Concrete
{
    public class MainPageEntitiesRepository: IMainPageEntitiesRepository
    {
        EFDBContext _context = new EFDBContext();
        /// <summary>
        /// Returns all entities from the main page
        /// </summary>
        public IEnumerable<MainPageEntity> All()
        {
            return _context.MainPageEntities.Include("Article");
        }

        /// <summary>
        /// Add a new entity which should be displayed on the main page
        /// </summary>
        /// <param name="newEntity">Enttity to be added to the main page </param>
        public void Add(MainPageEntity newEntity)
        {
            _context.MainPageEntities.Add(newEntity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Adds a collection of entities which should be displayed on the main page
        /// </summary>
        /// <param name="newEntities">Collection of entities to be added to the main page</param>
        public void Add(IEnumerable<MainPageEntity> newEntities)
        {
            _context.MainPageEntities.AddRange(newEntities);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates a single enity on the main page
        /// </summary>
        /// <param name="entityToUpdate">Updated entity</param>
        public void Update(MainPageEntity entityToUpdate)
        {
            _context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        /// <summary>
        /// Deletes a single entity from the main page
        /// </summary>
        /// <param name="entityToDelete">Entity to be deleted from the main page</param>
        public void Delete(MainPageEntity entityToDelete)
        {
            var entity = _context.MainPageEntities.FirstOrDefault(a => a.ID == entityToDelete.ID);
            if(entity != null)
            {
                _context.MainPageEntities.Remove(entity);
                _context.SaveChanges();
            }
        }


        /// <summary>
        /// Deletes from main page all entities in range
        /// </summary>
        /// <param name="entitiesToDelete">Collection with enities which should be removed from the main page</param>
        public void Delete(IEnumerable<MainPageEntity> entitiesToDelete)
        {
            var dbSet = _context.MainPageEntities;

            _context.MainPageEntities.RemoveRange(dbSet.Where(e => entitiesToDelete.Any(ex => ex.ID == e.ID)));
            _context.SaveChanges();
        }
        /// <summary>
        /// Deletes all entities from MainPage
        /// </summary>
        public void Clear()
        {
            _context.MainPageEntities.RemoveRange(_context.MainPageEntities);
            _context.SaveChanges();
        }

        /// <summary>
        /// Clears main page and fills it with new entities if any provided
        /// </summary>
        /// <param name="newEntities">New entities to be added to main page after it is cleared</param>
        public void Refill(IEnumerable<MainPageEntity> newEntities = null)
        {
            _context.MainPageEntities.RemoveRange(_context.MainPageEntities);
            if (newEntities != null)
            {
                _context.MainPageEntities.AddRange(newEntities); 
            }
            _context.SaveChanges();
        }
    }
}
