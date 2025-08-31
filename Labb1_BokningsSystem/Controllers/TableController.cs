using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases;
using Labb1_BokningsSystem.Services.UseCases.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers;

[ApiController]
[Route("api/table")]
public class TableController(
    IUseCase<TableDtos.CreateTableDto, CreateTable.Response> createTable,
    IUseCase<TableDtos.UpdateTableDto, UpdateTable.Response> updateTable,
    IUseCase<int, DeleteTable.Response> deleteTable
) : ControllerBase
{
    // Creates a new table.
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTable([FromBody] TableDtos.CreateTableDto dto)
    {
        var response = await createTable.ExecuteAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Updates table information (capacity).
    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateTable([FromBody] TableDtos.UpdateTableDto dto)
    {
        var response = await updateTable.ExecuteAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Deletes a table (only if no bookings exist).
    [HttpDelete("delete/{tableId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTable(int tableId)
    {
        var response = await deleteTable.ExecuteAsync(tableId);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}