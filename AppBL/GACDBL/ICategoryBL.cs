using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDModels;

namespace GACDBL
{
    public interface ICategoryBL
    {
        /// <summary>
        /// Adds a user, returns added category, null if an error
        /// </summary>
        /// <param name="c">category to add</param>
        /// <returns>Category added, null if not found</returns>
        Task<Category> AddCategory(Category c);
        /// <summary>
        /// Method to return all categories found in the database
        /// </summary>
        /// <returns>List of all categories</returns>
        Task<List<Category>> GetAllCategories();
    }
}
