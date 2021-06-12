using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDDL;
using GACDModels;

namespace GACDBL
{
    class CompBL : ICompBL
    {
        private Repo _repo;
        public CompBL(GACDDBContext context)
        {
            _repo = new Repo(context);
            
        }
        public Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int userId, string teststring)
        {
            Competition competition = new Competition();
            competition.StartDate = startDate;
            competition.EndDate = endDate;
            competition.CategoryId = categoryId;
            competition.CompetitionName = competitionName;
            competition.TestString = teststring;
            return _repo.AddCompetition(competition);
        }
    }
}
