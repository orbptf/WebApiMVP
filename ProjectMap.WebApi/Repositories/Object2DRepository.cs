using Dapper;
using Microsoft.Data.SqlClient;
using ProjectMap.WebApi.Interfaces;
using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public class Object2DRepository : IObject2DRepository
    {
        private readonly string sqlConnectionString;

        public Object2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<Object2D> InsertAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environmentId = await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (Id, EnvironmentId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer) VALUES (@Id, @EnvironmentId, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)", object2D);
                return object2D;
            }
        }

        public async Task<Object2D?> ReadAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<Object2D>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            }
        }

        public async Task UpdateAsync(Object2D environment)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Object2D] SET " +
                                                 "PositionX = @PositionX, " +
                                                 "PositionY = @PositionY, " +
                                                 "ScaleX = @ScaleX, " +
                                                 "ScaleY = @ScaleY, " +
                                                 "RotationZ = @RotationZ, " +
                                                 "SortingLayer = @SortingLayer"
                                                 , environment);

            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }





        public async Task<IEnumerable<Object2D>> ReadByEnvironmentIdAsync(Guid environmentId)
        {
            // Maak een verbinding met de database (using zorgt dat deze na afloop netjes sluit)
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                // Dit is de SQL die we willen uitvoeren.
                // We halen alle rijen op uit [Object2D] waar de EnvironmentId gelijk is aan @EnvId
                string sqlQuery = @"SELECT * FROM [Object2D] WHERE EnvironmentId = @EnvironmentId";

                // Hier maken we een 'parameters-object' voor de query.
                // Daardoor weet Dapper dat @EnvId in de query overeenkomt met de waarde van environmentId.
                var parameters = new
                {
                    EnvironmentId = environmentId
                };

                // Voer de query asynchroon uit en parse de resultaten als een lijst van Object2D
                IEnumerable<Object2D> object2Ds = await sqlConnection.QueryAsync<Object2D>(sqlQuery, parameters);

                // Geef de lijst terug aan de aanroeper van deze methode
                return object2Ds;
            }
        }

    }
}