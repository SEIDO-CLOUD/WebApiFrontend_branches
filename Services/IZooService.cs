using Models;
using Models.DTO;

namespace Services;

public interface IZooService {
    public string BearerToken {set;}
    public Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IZoo>> ReadZooAsync(Guid id, bool flat);
    public Task<ResponseItemDto<ZooCuDto>> ReadZooDtoAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IZoo>> DeleteZooAsync(Guid id);
    public Task<ResponseItemDto<IZoo>> UpdateZooAsync(ZooCuDto item);
    public Task<ResponseItemDto<IZoo>> CreateZooAsync(ZooCuDto item);
}