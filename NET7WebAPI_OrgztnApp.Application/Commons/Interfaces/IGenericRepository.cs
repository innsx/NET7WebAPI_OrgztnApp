using NET7WebAPI_OrgztnApp.Domain.BaseModels;
using NET7WebAPI_OrgztnApp.Domain.Commons.Utilities;

namespace NET7WebAPI_OrgztnApp.Application.Commons.Interfaces
{
    /*
     in IGenericRepository.cs, we have it takes a GENERIC object TYPE, 
    with restrict to a GENERIC constraint to IDbEntity Interface because 
    EVERY Object (models) we SPECIFIED as TEntity Type ALSO INHERITS IDbEntity:
     */
    public interface IGenericRepository<TEntity> where TEntity : IDbEntity
    {
        Task<IEnumerable<TEntity>> GetAllRecordsAsync(PagingSortingQueryParameters queryParameters, params string[] selectData);
        Task<TEntity> GetRecordByIdAsync(string guid, params string[] selectData);
        Task<IEnumerable<TEntity>> GetRecordBySpecificColumnAsync(string columnName, string columnValue, params string[] selectData);
        Task<string> AddRecordAsync(TEntity entity);
        Task<bool> UpdateRecordAsync(TEntity entity);
        Task SoftDeleteRecordAsync(string id, bool softDeleteFromRelatedChildTable = false);
        Task<int> GetTotalRecordCountsAsync();
        Task<bool> IsRecordExistedAsync(string distinguishingUniqueKeyValue); 
    }
}
