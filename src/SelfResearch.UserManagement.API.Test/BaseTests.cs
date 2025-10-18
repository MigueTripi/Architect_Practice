using System;
using SelfResearch.Core.Infraestructure;

namespace SelfResearch.UserManagement.API.Test;

public class BaseTests
{

    public BaseTests()
    {
        FluentResultConfiguration.AddCustomErrorHandling();
    }
}
