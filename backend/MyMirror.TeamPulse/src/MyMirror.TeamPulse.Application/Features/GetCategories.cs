using MediatR;
using MyMirror.TeamPulse.Application.DTOs;
using MyMirror.TeamPulse.Application.Interfaces;

namespace MyMirror.TeamPulse.Application.Features;

public record GetCategoriesQuery : IRequest<List<PulseCategoryDto>>;

public class GetCategoriesHandler(ICategoryRepository categoryRepo) : IRequestHandler<GetCategoriesQuery, List<PulseCategoryDto>>
{
    private readonly ICategoryRepository _categoryRepo = categoryRepo;

    public async Task<List<PulseCategoryDto>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var categories = await _categoryRepo.GetAllAsync(ct);
        return [.. categories.Select(c => new PulseCategoryDto(c.Id, c.Name))];
    }
}
