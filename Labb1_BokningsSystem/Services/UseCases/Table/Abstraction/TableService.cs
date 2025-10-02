using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Table;

namespace Labb1_BokningsSystem.Services;

public class TableService(CreateTable createTable, UpdateTable updateTable, DeleteTable deleteTable, GetTables getTables) : ITableService
{
    public async Task<CreateTable.Response> CreateTableAsync(TableDtos.CreateTableDto request)
        => await createTable.ExecuteAsync(request);

    public async Task<UpdateTable.Response> UpdateTableAsync(TableDtos.UpdateTableDto request)
        => await updateTable.ExecuteAsync(request);

    public async Task<DeleteTable.Response> DeleteTableAsync(int tableId)
        => await deleteTable.ExecuteAsync(tableId);

    public async Task<GetTables.Response> GetTablesAsync()
        => await getTables.ExecuteAsync(new TableDtos.GetTablesDto());
}
