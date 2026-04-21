using Dapper;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Application.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.BaseModels;
using NET7WebAPI_OrgztnApp.Domain.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Infrastructure.Persistences.DataContexts;
using System.Data;

namespace NET7WebAPI_OrgztnApp.Infrastructure.Persistences.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : IDbEntity
    {
        private readonly DapperDataContext _dapperDataContext;

        public GenericRepository(DapperDataContext dapperDataContext)
        {
            _dapperDataContext = dapperDataContext;
        }

        public async Task<string> AddRecordAsync(TEntity entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("columnNames", typeof(TEntity).GetDbTableColumnNames(new string[0]), DbType.String, ParameterDirection.Input);
            parameters.Add("columnValues", typeof(TEntity).GetColumnValuesForInsert(entity), DbType.String, ParameterDirection.Input);

            var results = await _dapperDataContext.DbConnection!.ExecuteScalarAsync<string>
            (
                "uspInsertRecord",                                                         
                parameters,                                                         
                _dapperDataContext.DbTransaction,                                                         
                commandType: CommandType.StoredProcedure
            );

            return results!.ToString();
        }

        public async Task<IEnumerable<TEntity>> GetAllRecordsAsync(PagingSortingQueryParameters paginationQueryParameters, params string[] selectedColumns)
        {
            var parameters = new DynamicParameters();

            parameters.Add("tableName", 
                                typeof(TEntity).GetDbTableName(), 
                                DbType.String, 
                                ParameterDirection.Input, 
                                size: 50  //The size of the parameter
            );

            parameters.Add("pageNumber", paginationQueryParameters.PageNumber, DbType.Int32, ParameterDirection.Input);
            parameters.Add("pageSize", paginationQueryParameters.PageSize, DbType.Int32, ParameterDirection.Input);


            if (selectedColumns is not null || selectedColumns?.Length > 0)
            {
                parameters.Add("columns", 
                                typeof(TEntity).GetDbTableColumnNames(selectedColumns), 
                                DbType.String,
                                ParameterDirection.Input
                );
            }

            using (var connection = _dapperDataContext.DbConnection)
            {
                return await connection!.QueryAsync<TEntity>
                (
                    "uspGetRecords",                                                                
                    parameters,                                                                 
                    commandType: CommandType.StoredProcedure
                );
            }

        }


        public async Task<TEntity> GetRecordByIdAsync(string guid, params string[] selectData)
        {
            var parameters = new DynamicParameters();

            parameters.Add("tableName", 
                            typeof(TEntity).GetDbTableName(), 
                            DbType.String, 
                            ParameterDirection.Input, 
                            size: 50
            );

            parameters.Add("id", 
                            guid, 
                            DbType.String, 
                            ParameterDirection.Input, 
                            size: 22
            );

            if (selectData is not null || selectData?.Length > 0)
            {
                parameters.Add("columns", 
                                typeof(TEntity).GetDbTableColumnNames(selectData), 
                                DbType.String, 
                                ParameterDirection.Input
                );
            }

            using (var connection = _dapperDataContext.DbConnection)
            {
                var results = await connection!.QuerySingleOrDefaultAsync<TEntity>
                (
                    "uspGetRecordsById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return results!;
            }
        }

        public async Task<IEnumerable<TEntity>> GetRecordBySpecificColumnAsync(string columnName, string columnValue, params string[] selectData)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("columnName", columnName, DbType.String, ParameterDirection.Input, size: 60);
            parameters.Add("columnValue", columnValue, DbType.String, ParameterDirection.Input, size: 100);

            if (selectData is not null || selectData?.Length > 0)
            {
                parameters.Add("columns", 
                                typeof(TEntity).GetDbTableColumnNames(selectData), 
                                DbType.String, 
                                ParameterDirection.Input
                );
            }

            using (var connection = _dapperDataContext.DbConnection)
            {
                var results = await connection!.QueryAsync<TEntity>
                (                            
                    "uspGetRecordsBySpecificColumn",                                  
                    parameters,                                  
                    commandType: CommandType.StoredProcedure
                );

                return results;
            }
        }

        public async Task<int> GetTotalRecordCountsAsync()
        {
            var parameters = new DynamicParameters();

            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);

            using (var connection = _dapperDataContext.DbConnection)
            {
                var totalRecordCounts = await connection!.QuerySingleOrDefaultAsync<int>
                (
                                "uspGetTotalRecordsCount", 
                                parameters, 
                                commandType: CommandType.StoredProcedure
                );

                return totalRecordCounts;
            }
        }

        public async Task<bool> IsExistedAsync(string distinguishingUniqueKeyValue)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("distinguishingUniqueKeyColumnName", typeof(TEntity).GetDistinguishingUniqueKeyName(), DbType.String, ParameterDirection.Input, size: 100);
            parameters.Add("distinguishingUniquekeyColumnValue", distinguishingUniqueKeyValue, DbType.String, ParameterDirection.Input, size: 100);

            using (var connection = _dapperDataContext.DbConnection)
            {
                var isRecordExisted = await connection!.QuerySingleOrDefaultAsync<bool>
                (                                               
                    "uspDoesRecordExist",                                                 
                    parameters,                                                 
                    commandType: CommandType.StoredProcedure
                );

                return isRecordExisted;
            }
        }

        public async Task SoftDeleteRecordAsync(string id, bool softDeleteFromRelatedChildTable = false)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("id", id, DbType.String, ParameterDirection.Input, size: 22);

            await _dapperDataContext.DbConnection!.ExecuteAsync
            (                                
                "uspSoftDeleteRecord",                                 
                parameters, _dapperDataContext.DbTransaction,                                 
                commandType: CommandType.StoredProcedure
            );

            if (softDeleteFromRelatedChildTable is true)
            {
                foreach (var associatedType in typeof(TEntity).GetAssociatedTypes())
                {
                    parameters = new DynamicParameters();
                    parameters.Add("tableName", associatedType.Type!.GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
                    parameters.Add("foreignkeyColumnName", associatedType.ForeignKeyProperty!.GetDbColumnName(), DbType.String, ParameterDirection.Input, size: 50);
                    parameters.Add("foreignkeyColumnValue", id, DbType.String, ParameterDirection.Input, size: 22);

                    await _dapperDataContext.DbConnection!.ExecuteAsync
                    (                                
                        "uspSoftDeleteForeignKeyRecord",                                 
                        parameters, _dapperDataContext.DbTransaction,                                 
                        commandType: CommandType.StoredProcedure
                   );
                }
            }
        }

        public async Task<bool> UpdateRecordAsync(TEntity entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("tableName", typeof(TEntity).GetDbTableName(), DbType.String, ParameterDirection.Input, size: 50);
            parameters.Add("columnsToUpdate", typeof(TEntity).GetColumnValuesForUpdate(entity), DbType.String, ParameterDirection.Input);
            parameters.Add("id", entity.Id, DbType.String, ParameterDirection.Input, size: 22);

            int isUpdated = await _dapperDataContext.DbConnection!.ExecuteAsync
            (
                "uspUpdateRecord",                             
                parameters,                             
                _dapperDataContext.DbTransaction,                             
                commandType: CommandType.StoredProcedure
            );

            if ( isUpdated == 0 )
            {
                return false;
            }

            return true;
        }
    }
}
