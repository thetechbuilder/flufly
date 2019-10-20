using FLUFLY.API.Helpers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using FLUFLY.API.Model.DTO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FNCPLT.API.Service
{
    public class ProductService : BaseService, IProductService
	{
		private readonly IWebClientHelper _client;
		private readonly IWebClientHelper _locationClient;

		public ProductService(
			IWebClientLocator<string> locator, 
			ILogger<IProductService> logger, 
			ICacheService cache,
			IOptions<ProcessAPIApplicationConfiguration> config = null) : base(locator, logger, config, cache)
		{
		    this._client = locator.Get(Model.Enum.SERVICE_CLIENTS.FluflyProductService);
		}

        public async Task<IServiceResponse<FluflyProductDTO>> GetFluflyProductsAsync(string productCategory, bool isActive) 
		{

			var restResponse = await this._client.GetAsync($"flufly-products?product_category={productCategory}");
			var response = await ValidateResponse(restResponse);
			var responseObject = JsonConvert.DeserealizeObject<ServiceResponse<IEnumerable<FluflyProductDTO>>>(response);

			if (responseObject != null && responseObject.Data.Count() != 0)
			{
				IList<FluflyProductDTO> fluflyDTO = AutoMapper.Mapper.Map<IEnumerable<FluflyProductDTO>>(responseObject.Data).toList();
				if (fluflyDTO != null && fluflyDTO.Count > 0)
				{
					if (isActive == true)
					{
						for (int i = 0; i < fluflyDTO.Count; i++) {
							if ((fluflyDTO[i].ProductEndDate == null || fluflyDTO[i].ProductEndDate >= DateTime.Now) && fluflyDTO[i].ProductStartDate >= DateTime.Now) {
								fluflyDTO.Remove(fluflyDTO[i]);
								i--;
							}
						}
					}
				}
				return fluflyDTO;
			}
			return responseObject.Data;
		}
    }
}
