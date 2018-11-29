using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Hcs.Platform.Data
{
    public interface IDbModelCreatingConfig
    {
        IEnumerable<Action<ModelBuilder>> Configuraions { get; }
    }
}