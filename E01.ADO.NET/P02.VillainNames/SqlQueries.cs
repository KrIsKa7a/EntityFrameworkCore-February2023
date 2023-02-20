// ReSharper disable InconsistentNaming
namespace P02.VillainNames
{
    public static class SqlQueries
    {
        public const string GetAllVillainsAndCountOfTheirMinions =
            @"  SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                  FROM Villains AS v 
                  JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
              GROUP BY v.Id, v.Name 
                HAVING COUNT(mv.VillainId) > 3 
              ORDER BY COUNT(mv.VillainId)";

        public const string GetVillainNameById =
            @"SELECT Name FROM Villains WHERE Id = @Id";

        public const string GetAllMinionsByVillainId =
            @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

        public const string GetTownIdByName =
            @"SELECT Id FROM Towns WHERE Name = @townName";

        public const string AddNewTown =
            @"INSERT INTO Towns (Name) VALUES (@townName)";

        public const string GetVillainIdByName =
            @"SELECT Id FROM Villains WHERE Name = @Name";

        public const string AddVillainWithDefaultEvilnessFactor =
            @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

        public const string AddNewMinion =
            @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

        public const string GetMinionIdByName =
            @"SELECT Id FROM Minions WHERE Name = @Name";

        public const string SetMinionToBeServantOfVillain =
            @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
    }
}
