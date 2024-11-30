using Application.Features.Quotes.Commands.SubmitQuote;
using Application.Features.Quotes.Queries.GetQuotesByPage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class QuoteController(IMediator mediator) : BaseApiController
{
    [HttpGet(nameof(GetLatestSubmitedQuotes))]
    public async Task<IActionResult> GetLatestSubmitedQuotes(string symbol, int pageNumber, int pageSize)
    {
        return Ok(await mediator.Send(new GetQuotesByPageQuery(symbol, pageSize, pageNumber)));
    }

    [HttpGet(nameof(GetQuote))]
    public async Task<IActionResult> GetQuote(string symbol)
    {
        return Ok(await mediator.Send(new SubmitQuoteCommand(symbol)));
    }
}


