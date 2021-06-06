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
    class UserStatBL:IUserStatBL
    {
        private Repo _repo;
        public UserStatBL(GACDDBContext context)
        {
            _repo = new Repo(context);
        }

        public UserStat AddTestUpdateStat(int userId, int categoryId, TypeTest typeTest)
        {
            UserStat userStat;
            try
            {
                if(_repo.GetSatUserCat(userId, categoryId) != null) userStat = _repo.GetSatUserCat(userId, categoryId);
                else
                {
                    userStat = new UserStat();
                    userStat.AverageAccuracy = 0;
                    userStat.AverageWPM = 0;
                    userStat.NumberOfTests = 0;
                    userStat.TotalTestTime = 0;
                }
                userStat.NumberOfTests += 1;
                userStat.TotalTestTime += typeTest.TimeTaken;
                //TODO:other math
                userStat = _repo.AddUpdateStats(userId, categoryId, userStat);
                typeTest.UserStatId = userStat.Id;
                _repo.AddTest(typeTest);
                return userStat;


            }catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error occured in AddTest BL");
                return null;
            }
        }
    }
}
