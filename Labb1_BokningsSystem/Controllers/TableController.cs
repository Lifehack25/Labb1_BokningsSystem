using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers;

[ApiController]
[Route("api/table")]
public class TableController(ITableService useCase) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetTables()
    {
        var response = await useCase.GetTablesAsync();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Creates a new table.
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateTable([FromBody] TableDtos.CreateTableDto dto)
    {
        var response = await useCase.CreateTableAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Updates table information (capacity).
    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateTable([FromBody] TableDtos.UpdateTableDto dto)
    {
        var response = await useCase.UpdateTableAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Deletes a table (only if no bookings exist).
    [HttpDelete("delete/{tableId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTable(int tableId)
    {
        var response = await useCase.DeleteTableAsync(tableId);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}