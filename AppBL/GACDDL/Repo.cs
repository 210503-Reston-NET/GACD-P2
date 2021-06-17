using GACDModels;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDDL
{
    public class Repo : IRepo
    {
        private GACDDBContext _context;
        public Repo(GACDDBContext context)
        {
            _context = context;
            Log.Debug("Repo instantiated");
        }

        public async Task<Category> AddCategory(Category cat)
        {
            try
            {
                //Make sure category doesn't already exists
                Category category = await (from c in _context.Categories
                                     where c.Name == cat.Name
                                     select c).SingleAsync();
                return null;
            }catch (Exception)
            {
                await _context.Categories.AddAsync(cat);
                await _context.SaveChangesAsync();
                Log.Information("New category created.");
                return cat;
            }
        }

        public async Task<int> AddCompetition(Competition comp)
        {
            try
            {
                await _context.Competitions.AddAsync(comp);
                await _context.SaveChangesAsync();
                return comp.Id;
            }
            catch( Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error adding competition returning -1");
                return -1;
            }
        }

        public async Task<CompetitionStat> AddCompStat(CompetitionStat c)
        {
            try
            {
                CompetitionStat cstat = await (from compStat in _context.CompetitionStats
                                               where compStat.UserId == c.UserId
                                               && compStat.CompetitionId == c.CompetitionId
                                               select compStat).SingleAsync();
                cstat.WPM = c.WPM;
                cstat.Accuracy = c.Accuracy;
                await _context.SaveChangesAsync();
                _context.Entry(cstat).State = EntityState.Detached;
                return c;
            }
            catch (Exception) { }
            try
            {
                await _context.CompetitionStats.AddAsync(c);
                await _context.SaveChangesAsync();
                return c;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error adding competitionstat returning null");
                return null;
            }
        }

        public async Task<TypeTest> AddTest(TypeTest typeTest)
        {
            try
            {
                await _context.TypeTests.AddAsync(typeTest);
                await _context.SaveChangesAsync();
                Log.Information("Test added");
                return typeTest;
            } catch(Exception)
            {
                Log.Error("Issue adding test");
                return null;
            }
        }

        public async Task<UserStat> AddUpdateStats(int categoryid, int userid, UserStat userStat)
        {
            int userIdsave = userid;
            //Assuming these categories and users exist
            try 
            {
                int userStatId = await (from uscj in _context.UserStatCatJoins
                                  where uscj.CategoryId == categoryid &&
                                  uscj.UserId == userid
                                  select uscj.UserStatId).SingleAsync();
                UserStat uStatInDB = await (from uS in _context.UserStats
                                      where uS.Id == userStatId
                                      select uS).SingleAsync();
                uStatInDB.AverageWPM = userStat.AverageWPM;
                uStatInDB.AverageAccuracy = userStat.AverageAccuracy;
                uStatInDB.NumberOfTests = userStat.NumberOfTests;
                uStatInDB.TotalTestTime = userStat.TotalTestTime;
                await _context.SaveChangesAsync();
                return uStatInDB;
            } catch (Exception)
            {
                await _context.UserStats.AddAsync(userStat);
                await _context.SaveChangesAsync();
                //this might miss timing just call me if you have an issue
                UserStatCatJoin uscj = new UserStatCatJoin();
                uscj.CategoryId = categoryid;
                uscj.UserId = userIdsave;
                uscj.UserStatId = userStat.Id;
                await _context.UserStatCatJoins.AddAsync(uscj);
                await _context.SaveChangesAsync();
            }
            return userStat;
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                if (await GetUser(user.Auth0Id) != null) return null;
                user.Revapoints = 0;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public async Task<List<Bet>> ClaimBets(int userId)
        {
            try
            {
                User u = await GetUser(userId);
                List<Bet> bets = await (from b in _context.Bets
                                        where b.BettingUserId == userId
                                        && b.Claimed == false
                                        select b).ToListAsync();
                List<Bet> wonBets = new List<Bet>();
                foreach(Bet bt in bets)
                {
                    CompetitionStat cStat = await (from cS in _context.CompetitionStats
                                                   where cS.CompetitionId == bt.CompetitionId
                                                   && cS.UserId == bt.UserId
                                                   select cS).SingleAsync();
                    if((cStat.rank == 1)&&(!bt.Claimed))
                    {
                        bt.Won = true;
                        bt.Claimed = true;
                        u.Revapoints += (bt.PointsBet * 2);
                        wonBets.Add(bt);
                    }
                    else
                    {
                        bt.Won = false;
                        bt.Claimed = true;
                    }
                }
                await _context.SaveChangesAsync();
                return wonBets;
            }catch(Exception e)
            {
                Log.Error(e.StackTrace);
                Log.Error("No bets found");
                return new List<Bet>();
            }
        }

        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                return await (from c in _context.Categories
                        select c).ToListAsync();
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        public async Task<List<Competition>> GetAllCompetitions()
        {
            try
            {
                return await(from c in _context.Competitions
                             select c).ToListAsync();
            }
            catch (Exception)
            {
                Log.Information("No competitions found, returning empty list");
                return new List<Competition>();
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try {
                return await (from u in _context.Users
                        select u).ToListAsync();
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                return new List<User>();
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                return await(from c in _context.Categories
                             where c.Id == id
                             select c).SingleAsync();
            }catch(Exception e)
            {
                Log.Error("Error finding category returning null");
                return null;
            }
        }

        public async Task<Category> GetCategoryByName(int name)
        {
            try
            {
                return await (from cat in _context.Categories
                             where cat.Name == name
                             select cat).SingleAsync();
            } catch(Exception e)
            {
                Log.Information("No such category found");
                return null;

            }
        }

        public async Task<Competition> GetCompetition(int id)
        {
            try
            {
                return await (from c in _context.Competitions
                              where c.Id == id
                              select c).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("No competition found");
                return null;
            }
        }

        public async Task<string> GetCompetitionString(int compId)
        {
            try
            {
                return await (from comp in _context.Competitions
                              where comp.Id == compId
                              select comp.TestString).SingleAsync();
            }
            catch( Exception e)
            {
                Log.Error("Error retrieving string");
                return null;
            }
        }

        public async Task<List<CompetitionStat>> GetCompStats(int compId)
        {
            try
            {
                return await (from compStat in _context.CompetitionStats
                              where compStat.CompetitionId == compId
                              orderby compStat.WPM descending
                              select compStat).ToListAsync();

            }catch(Exception e)
            {
                Log.Information("No relevant stats foound returning empty list");
                return new List<CompetitionStat>();
            }
        }

        public async Task<Tuple<string, string, int>> GetCompStuff(int compId)
        {
            try
            {
                return await(from comp in _context.Competitions
                             where comp.Id == compId
                             select Tuple.Create(comp.TestAuthor, comp.TestString, comp.CategoryId)).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("Competition not found returning null");
                return null;
            }
        }

        public async Task<UserStat> GetSatUserCat(int categoryId, int userId)
        {
            try
            {
                int userStatId = await (from uscj in _context.UserStatCatJoins
                                  where uscj.CategoryId == categoryId &&
                                  uscj.UserId == userId
                                  select uscj.UserStatId).SingleAsync();
                UserStat uStatInDB = await (from uS in _context.UserStats
                                      where uS.Id == userStatId
                                      select uS).SingleAsync();
                return uStatInDB;
            } catch (Exception)
            {
                Log.Debug("Stat not found, returning null");
                return null;
            }
        }

        public async Task<List<TypeTest>> GetTypeTestsForUser(int userId)
        {
            try
            {
                List<int> userStatIds= await (from uscj in _context.UserStatCatJoins
                                                                where uscj.UserId == userId
                                                                select uscj.UserStatId).ToListAsync();
                List<TypeTest> typeTests = new List<TypeTest>();
                foreach(int usId in userStatIds)
                {
                    List<TypeTest> testForStat = await (from test in _context.TypeTests
                                                        where test.UserStatId == usId
                                                        select test).ToListAsync();
                    typeTests.AddRange(testForStat);
                }
                typeTests = typeTests.OrderByDescending(t => t.Date).ToList();
                typeTests = typeTests.Take(100).ToList();
                typeTests = typeTests.OrderBy(t => t.Date).ToList();
                return typeTests;
            }catch(Exception)
            {
                Log.Error("No Type Tests found returning empty list");
                return new List<TypeTest>();
            }
        }

        public async  Task<User> GetUser(int id)
        {
            try
            {
                return await (from u in _context.Users
                        where u.Id == id
                        select u).SingleAsync();
            }
            catch (Exception)
            {
                Log.Error("User Not Found");
                return null;
            }
        }

        public async Task<User> GetUser(string auth0Id)
        {
            try
            {
                return await (from u in _context.Users
                        where u.Auth0Id == auth0Id
                        select u).SingleAsync();
            }
            catch(Exception)
            {
                Log.Error("User not found");
                return null;
            }
        }

        public async Task<UserStat> GetUserStatById(int id)
        {
            try
            {
                return await (from u in _context.UserStats
                              where u.Id == id
                              select u
                        ).SingleAsync();
            }
            catch(Exception)
            {
                Log.Fatal("UserStatJoin not maintained correctly returning null");
                return null;
            }
        }

        public async Task<List<UserStatCatJoin>> GetUserStats(int userId)
        {
            try
            {
                List<UserStatCatJoin> uscjs = await (from uscj in _context.UserStatCatJoins
                                        where uscj.UserId == userId
                                        select uscj).ToListAsync();
                return uscjs;
            }catch(Exception e)
            {
                Log.Error("No stats for user were found");
                Log.Error(e.Message);
                return new List<UserStatCatJoin>();
            }
            throw new NotImplementedException();
        }

        public async Task<Bet> PlaceBetOnCompUser(int better, int bettee, int compId, int betaAmount)
        {
            try
            {
                User better1 = await (from usr in _context.Users
                                      where usr.Id == better
                                      select usr).SingleAsync();
                if (better1.Revapoints < betaAmount) return null;
                else
                {
                    CompetitionStat competitionStat = await (from cS in _context.CompetitionStats
                                                             where cS.CompetitionId == compId
                                                             && cS.UserId == bettee
                                                             select cS).SingleAsync();
                    try
                    {
                        Bet bet = await (from bt in _context.Bets
                                         where bt.UserId == bettee
                                         && bt.CompetitionId == compId
                                         select bt).SingleAsync();
                        bet.PointsBet += betaAmount;
                        better1.Revapoints -= betaAmount;
                        await _context.SaveChangesAsync();
                        return bet;
                    }
                    catch(Exception e)
                    {
                        Log.Information("e.Message");
                        Log.Information("Creating new bet");
                        Bet bet = new Bet();
                        bet.UserId = bettee;
                        bet.CompetitionId = compId;
                        bet.BettingUserId = better;
                        bet.Claimed = false;
                        bet.PointsBet = betaAmount;
                        await _context.Bets.AddAsync(bet);
                        better1.Revapoints -= betaAmount;
                        await _context.SaveChangesAsync();
                        return bet;
                    }
                }
            }catch(Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Can't find relavent info, returning null");
                return null;
            }
        }

        public async Task<CompetitionStat> UpdateCompStat(CompetitionStat c)
        {
            try
            {
                 _context.CompetitionStats.Update(c);
                await _context.SaveChangesAsync();
                return c;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Error adding competitionstat returning null");
                return null;
            }
        }

    }
}
