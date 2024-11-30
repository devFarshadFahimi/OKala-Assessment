using Application.Features.Quotes.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Quotes.Queries.GetQuotesByPage;

public record GetQuotesByPageQuery(string Symbol, int PageSize = 10, int PageNumber = 1) : IRequest<List<QuoteResponseDTO>>
{
    public int ToSkip => (PageNumber > 0 ? (PageNumber - 1) : PageNumber) * PageSize;
}


public class GetQuotesByPageQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetQuotesByPageQuery, List<QuoteResponseDTO>>
{
    public async Task<List<QuoteResponseDTO>> Handle(GetQuotesByPageQuery request, CancellationToken cancellationToken)
    {
        var data = await dbContext.Quotes
            .Where(p => p.Symbol == request.Symbol.ToUpper())
            .Include(p => p.Prices)
            .OrderByDescending(p => p.SubmitDate)
            .Skip(request.ToSkip)
            .Take(request.PageSize)
            .ProjectToType<QuoteResponseDTO>()
            .ToListAsync(cancellationToken);

        return data;
    }
}