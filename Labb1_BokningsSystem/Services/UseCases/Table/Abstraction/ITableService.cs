using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Table;

namespace Labb1_BokningsSystem.Services;

public interface ITableService
{
    Task<CreateTable.Response> CreateTableAsync(TableDtos.CreateTableDto request);
    Task<UpdateTable.Response> UpdateTableAsync(TableDtos.UpdateTableDto request);
    Task<DeleteTable.Response> DeleteTableAsync(int tableId);
    Task<GetTables.Response> GetTablesAsync();
}
