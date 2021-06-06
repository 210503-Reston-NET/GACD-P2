using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDModels;


namespace GACDBL
{
    interface IUserStatBL
    {
        /// <summary>
        /// Method that Adds test and updates the user's stats
        /// </summary>
        /// <param name="userId">userId of test taker</param>
        /// <param name="categoryId">id of the category</param>
        /// <param name="typeTest">Test user has taken</param>
        /// <returns>user stat of test taker</returns>
        UserStat AddTestUpdateStat(int userId, int categoryId, TypeTest typeTest);

        TypeTest SaveTypeTest(UserStat userStats, int errors, int words, int timeTaken, DateTime date, double wpm);

        
    }
}
