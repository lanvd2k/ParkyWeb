using ParkyWEB.Repository.IRepository;

namespace ParkyWEB.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository 
    {
        private readonly IHttpClientFactory _clientFactory;

        public NationalParkRepository(IHttpClientFactory clientFactory) :base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
