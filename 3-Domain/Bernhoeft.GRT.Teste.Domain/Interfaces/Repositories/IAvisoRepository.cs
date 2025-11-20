using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Enums;

namespace Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories
{
    public interface IAvisoRepository : IRepository<AvisoEntity>
    {
        Task<List<AvisoEntity>> ObterTodosAvisosAsync(TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default);
        Task<AvisoEntity> ObterAvisoByIdAsync(int id, TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default);
        Task<AvisoEntity> AddAvisoAsync(AvisoEntity entity, CancellationToken cancellationToken = default);
        Task<AvisoEntity> UpdateAvisoAsync(AvisoEntity entity, CancellationToken cancellationToken = default);
        Task SoftDeleteAvisoAsync(AvisoEntity entity, CancellationToken cancellationToken = default);
    }
}