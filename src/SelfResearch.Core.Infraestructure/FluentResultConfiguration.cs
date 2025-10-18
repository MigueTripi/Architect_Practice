using FluentResults.Extensions.AspNetCore;
using SelfResearch.Core.Infraestructure.ErrorHandlers;

namespace SelfResearch.Core.Infraestructure;

public static class FluentResultConfiguration
{
    public static void AddCustomErrorHandling()
    {
        AspNetCoreResult.Setup(config =>
        {
            config.DefaultProfile = new ResultProfile();
        });
    }
}
