namespace P02.VillainNames
{
    using System.Text;

    using Microsoft.Data.SqlClient;

    public class StartUp
    {
        static async Task Main(string[] args)
        {
            // In .NET 6 ToString() is working properly
            // If you are using 'using' operator you would not need to call Close() explicitly
            // If you are not using 'using' - > Close()/Dispose()
            await using SqlConnection sqlConnection =
                new SqlConnection(Config.ConnectionString);
            await sqlConnection.OpenAsync();

            string[] minionArgs = Console.ReadLine()
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string[] villainArgs = Console.ReadLine()
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            string result = 
                await AddNewMinionAsync(sqlConnection, minionArgs[1], villainArgs[1]);
            Console.WriteLine(result);
        }

        // Problem 02
        static async Task<string> GetAllVillainsWithTheirMinionsAsync(SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand sqlCommand =
                new SqlCommand(SqlQueries.GetAllVillainsAndCountOfTheirMinions, sqlConnection);
            // One row with many columns
            // First the reader hasn't loaded any data. We must call Read() first!
            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
            while (reader.Read())
            {
                string villainName = (string)reader["Name"];
                int minionsCount = (int)reader["MinionsCount"];

                sb.AppendLine($"{villainName} – {minionsCount}");
            }

            // No more data
            return sb.ToString().TrimEnd();
        }

        // Problem 03
        static async Task<string> GetVillainWithAllMinionsByIdAsync(SqlConnection sqlConnection, int villainId)
        {
            // SQL Injection Prevention -> Using SqlParameters
            // One row with one column -> ExecuteScalar
            SqlCommand getVillainNameCmd =
                new SqlCommand(SqlQueries.GetVillainNameById, sqlConnection);
            getVillainNameCmd.Parameters.AddWithValue("@Id", villainId);

            object? villainNameObj = await getVillainNameCmd.ExecuteScalarAsync();
            if (villainNameObj == null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }

            string villainName = (string)villainNameObj;

            // Use SqlParameters -> Automatically escapes SqlInjection
            // ExecuteReader() -> Many rows with many columns
            SqlCommand getAllMinionsCmd =
                new SqlCommand(SqlQueries.GetAllMinionsByVillainId, sqlConnection);
            getAllMinionsCmd.Parameters.AddWithValue("@Id", villainId);
            SqlDataReader minionsReader = await getAllMinionsCmd.ExecuteReaderAsync();

            string output = 
                GenerateVillainWithMinionsOutput(villainName, minionsReader);
            return output;
        }

        private static string GenerateVillainWithMinionsOutput(string villainName, SqlDataReader minionsReader)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Villain: {villainName}");
            if (!minionsReader.HasRows)
            {
                sb.AppendLine("(no minions)");
            }
            else
            {
                while (minionsReader.Read())
                {
                    long rowNum = (long)minionsReader["RowNum"];
                    string minionName = (string)minionsReader["Name"];
                    int minionAge = (int)minionsReader["Age"];

                    sb.AppendLine($"{rowNum}. {minionName} {minionAge}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 04
        static async Task<string> AddNewMinionAsync(SqlConnection sqlConnection, string minionInfo, string villainName)
        {
            StringBuilder sb = new StringBuilder();

            string[] minionArgs = minionInfo
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string minionName = minionArgs[0];
            int minionAge = int.Parse(minionArgs[1]);
            string townName = minionArgs[2];
            
            // Check if given Town exist and if it does not exist -> adding it
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                int townId = await GetTownIdOrAddByNameAsync(sqlConnection, sqlTransaction, sb, townName);
                int villainId = await GetVillainIdOrAddByNameAsync(sqlConnection, sqlTransaction, sb, villainName);
                int minionId = await AddNewMinionAndReturnIdAsync(sqlConnection, sqlTransaction, minionName, minionAge, townId);

                await SetMinionToBeServantOfVillainAsync(sqlConnection, sqlTransaction, minionId, villainId);
                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}");

                await sqlTransaction.CommitAsync();
            }
            catch (Exception e)
            {
                await sqlTransaction.RollbackAsync();
                sb.AppendLine($"Transaction Failed!");
            }

            return sb.ToString().TrimEnd();
        }

        private static async Task<int> GetTownIdOrAddByNameAsync(SqlConnection sqlConnection, SqlTransaction transaction, StringBuilder sb, string townName)
        {
            SqlCommand getTownIdCmd =
                new SqlCommand(SqlQueries.GetTownIdByName, sqlConnection, transaction);
            getTownIdCmd.Parameters.AddWithValue("@townName", townName);

            int? townId = (int?)await getTownIdCmd.ExecuteScalarAsync();
            if (!townId.HasValue)
            {
                SqlCommand addNewTownCmd =
                    new SqlCommand(SqlQueries.AddNewTown, sqlConnection, transaction);
                addNewTownCmd.Parameters.AddWithValue("@townName", townName);

                // Add the town command
                await addNewTownCmd.ExecuteNonQueryAsync();

                // Take the ID of the newly added town
                townId = (int?)await getTownIdCmd.ExecuteScalarAsync();
                sb.AppendLine($"Town {townName} was added to the database.");
            }

            return townId.Value;
        }

        private static async Task<int> GetVillainIdOrAddByNameAsync(SqlConnection sqlConnection, SqlTransaction transaction, StringBuilder sb,
            string villainName)
        {
            SqlCommand getVillainIdCmd =
                new SqlCommand(SqlQueries.GetVillainIdByName, sqlConnection, transaction);
            getVillainIdCmd.Parameters.AddWithValue("@Name", villainName);

            int? villainId = (int?)await getVillainIdCmd.ExecuteScalarAsync();
            if (!villainId.HasValue)
            {
                SqlCommand addVillainCmd =
                    new SqlCommand(SqlQueries.AddVillainWithDefaultEvilnessFactor, sqlConnection, transaction);
                addVillainCmd.Parameters.AddWithValue("@villainName", villainName);

                // Add new villain to the db
                await addVillainCmd.ExecuteNonQueryAsync();

                // Find the id of the newly created Villain
                villainId = (int?)await getVillainIdCmd.ExecuteScalarAsync();
                sb.AppendLine($"Villain {villainName} was added to the database.");
            }

            return villainId.Value;
        }

        private static async Task<int> AddNewMinionAndReturnIdAsync(SqlConnection sqlConnection, SqlTransaction transaction, string minionName,
            int minionAge, int townId)
        {
            SqlCommand addMinionCmd =
                new SqlCommand(SqlQueries.AddNewMinion, sqlConnection, transaction);
            addMinionCmd.Parameters.AddWithValue("@name", minionName);
            addMinionCmd.Parameters.AddWithValue("@age", minionAge);
            addMinionCmd.Parameters.AddWithValue("@townId", townId);

            // We are adding new minion
            await addMinionCmd.ExecuteNonQueryAsync();

            // We need to find the id of the newly created Minion
            SqlCommand getMinionIdCmd =
                new SqlCommand(SqlQueries.GetMinionIdByName, sqlConnection, transaction);
            getMinionIdCmd.Parameters.AddWithValue("@Name", minionName);

            int minionId = (int)await getMinionIdCmd.ExecuteScalarAsync();
            return minionId;
        }

        private static async Task SetMinionToBeServantOfVillainAsync(SqlConnection sqlConnection, SqlTransaction transaction, int minionId, int villainId)
        {
            SqlCommand addMinionVillainCmd =
                new SqlCommand(SqlQueries.SetMinionToBeServantOfVillain, sqlConnection, transaction);
            addMinionVillainCmd.Parameters.AddWithValue("@minionId", minionId);
            addMinionVillainCmd.Parameters.AddWithValue("@villainId", villainId);

            await addMinionVillainCmd.ExecuteNonQueryAsync();
        }
    }
}