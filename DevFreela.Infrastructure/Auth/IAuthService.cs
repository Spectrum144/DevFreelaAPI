using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Auth {
    public interface IAuthService {
        string ComputeHash(string password);
        string GenerateToken(string email, string role);

    }
}
