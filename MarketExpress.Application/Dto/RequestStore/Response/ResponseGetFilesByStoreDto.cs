using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.RequestStore.Response
{
    public class ResponseGetFilesByStoreDto
    {
        public string NationalIdFrontImage { get; set; }
       
        public string NationalIdBackImage { get; set; }

        public string StoreLicenseImage {  get; set; }
    }
}
