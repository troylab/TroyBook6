using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;
    private readonly BookManager _bookManager;

    public BookController(
        ILogger<BookController> logger,
        BookManager bookManager
        )
    {
        _logger = logger;
        _bookManager = bookManager;
    }

    [HttpPost("GetBookWithImage")]
    public async Task<IEnumerable<GetBookWithImage.Rs>> GetBookWithImage([FromBody] GetBookWithImage.Qy qy)
    {
        var r = await _bookManager.GetBooks(qy);
        return r;
    }
}

