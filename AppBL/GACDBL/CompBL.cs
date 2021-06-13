using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDDL;
using GACDModels;
using Serilog;


namespace GACDBL
{
    public class CompBL : ICompBL
    {
        private Repo _repo;
        public CompBL(GACDDBContext context)
        {
            _repo = new Repo(context);
            
        }
        public async Task<int> AddCompetition(DateTime startDate, DateTime endDate, int categoryId, string competitionName, int userId, string teststring)
        {
            Competition competition = new Competition();
            competition.StartDate = startDate;
            competition.EndDate = endDate;
            competition.CategoryId = categoryId;
            competition.CompetitionName = competitionName;
            competition.TestString = teststring;
            return  await _repo.AddCompetition(competition);
        }

        public async Task<List<CompetitionStat>> GetCompetitionStats(int competitionId)
        {
            return await _repo.GetCompStats(competitionId);
        }

        public async Task<string> GetCompString(int compId)
        {
            return await _repo.GetCompetitionString(compId);
        }

        public async Task<int> InsertCompStatUpdate(CompetitionStat competitionStat,int numberWords, int numberErrors)
        {
            try
            {
                double numWords = (double)numberWords;
                numWords = numWords / 5;
                double numErrors = (double)numberErrors;
                numErrors = numErrors / 5;
                competitionStat.Accuracy = (numWords - numErrors) / numWords;
                if (await _repo.AddCompStat(competitionStat) == null) throw new Exception("Error adding competition stat");
                List<CompetitionStat> competitionStats = await _repo.GetCompStats(competitionStat.CompetitionId);
                int i = 0;
                foreach(CompetitionStat c in competitionStats)
                {
                    i += 1;
                    c.rank = i;
                    await _repo.UpdateCompStat(c);
                }

                return competitionStats.First(comp => comp.UserId == competitionStat.UserId).rank;
            }
            catch (Exception)
            {
                Log.Error("error in insertCompStat returning null");
                return -1;
            }
        }
    }
}
