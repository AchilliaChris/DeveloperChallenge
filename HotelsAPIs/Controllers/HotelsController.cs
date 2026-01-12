using AutoMapper;
using DataAccess;
using DeveloperChallenge;
using DeveloperChallenge.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace HotelsAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService hotelService;
        private readonly ILogger<HotelsController> logger;
        private readonly IMapper mapper;
        public HotelsController(IHotelService _hotelService,
            ILogger<HotelsController> _logger,
            IMapper _mapper) {
        hotelService = _hotelService;
            logger = _logger;
            mapper = _mapper;
        }

        [Route("getbyname")]
        [HttpGet(Name = "GetByName")]
        public async Task<IEnumerable<HotelViewModel>> GetHotelByName(string name)
        { 
            return mapper.Map<List<Hotel>, List<HotelViewModel>>(await hotelService.GetHotelByName(name))
                .ToArray();
        }

        
    }
}
