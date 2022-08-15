using ParkyWEB.Repository.IRepository;

namespace ParkyWEB.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {

        }
    }
}
