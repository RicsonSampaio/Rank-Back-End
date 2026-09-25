using Rank.Core.DomainEntity;
using Rank.Core.DTO.Response;

namespace Rank.Core.Auth;

public interface ITokenGenerator
{
    TokenResponse Generate(User user);
}
