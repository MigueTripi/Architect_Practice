using AutoMapper;

namespace SelfResearch.Financial.API.Feature.Wallet;

public class WalletMappingProfiles : Profile
{
    
    public WalletMappingProfiles()
    {
        CreateMap<Wallet, WalletDto>().ReverseMap();
        CreateMap<WalletStateEnumDto, WalletStateEnum>().ReverseMap();
    }

}
