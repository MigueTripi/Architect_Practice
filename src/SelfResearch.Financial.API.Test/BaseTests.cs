using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using SelfResearch.Financial.API.Feature.Wallet;

namespace SelfResearch.Financial.API.Test;

public class BaseTests
{
    protected IMapper _mapper;
    protected MapperConfiguration _config;

    public BaseTests()
    {        
        _config = new MapperConfiguration(cfg =>
        { 
            cfg.AddProfile<WalletMappingProfiles>();
        },
        NullLoggerFactory.Instance);

        _mapper = _config.CreateMapper();
    }
}