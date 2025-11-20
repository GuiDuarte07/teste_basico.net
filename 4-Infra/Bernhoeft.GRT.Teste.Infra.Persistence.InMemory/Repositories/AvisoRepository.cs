using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.Attributes;
using Bernhoeft.GRT.Core.EntityFramework.Infra;
using Bernhoeft.GRT.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bernhoeft.GRT.ContractWeb.Infra.Persistence.SqlServer.ContractStore.Repositories
{
    [InjectService(Interface: typeof(IAvisoRepository))]
    public class AvisoRepository : Repository<AvisoEntity>, IAvisoRepository
    {
        public AvisoRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<List<AvisoEntity>> ObterTodosAvisosAsync(TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default)
        {
            var query = tracking is TrackingBehavior.NoTracking ? Set.AsNoTrackingWithIdentityResolution() : Set;
            return query.ToListAsync();
        }

        public Task<AvisoEntity> ObterAvisoByIdAsync(int id, TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default)
        {
            var query = tracking is TrackingBehavior.NoTracking ? Set.AsNoTrackingWithIdentityResolution() : Set;
            return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<AvisoEntity> AddAvisoAsync(AvisoEntity entity, CancellationToken cancellationToken)
        {
            var createdEntity = Set.Add(entity);

            await Db.SaveChangesAsync(cancellationToken);

            return createdEntity.Entity;
        }

        public async Task<AvisoEntity> UpdateAvisoAsync(AvisoEntity entity, CancellationToken cancellationToken = default)
        {
            var updatedEntity = Set.Update(entity);

            await Db.SaveChangesAsync(cancellationToken);

            return updatedEntity.Entity;
        }

        public Task SoftDeleteAvisoAsync(AvisoEntity entity, CancellationToken cancellationToken = default)
        {
            entity.Ativo = false;

            Set.Update(entity);

            return Db.SaveChangesAsync(cancellationToken);
        }
    }
}