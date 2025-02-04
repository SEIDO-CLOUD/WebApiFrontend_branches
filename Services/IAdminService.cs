using Models;
using Models.DTO;

namespace Services;

public interface IAdminService {

    public string BearerToken {set;}
    public Task<AdminInfo> AdminInfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded);
}