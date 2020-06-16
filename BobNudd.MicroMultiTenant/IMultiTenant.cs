using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BobNudd.MicroMultiTenant
{

    public interface IMultiTenant
    {
        /// <summary>
        /// Execute configuration options
        /// </summary>
        /// <returns></returns>
        Task Execute(IConfigurationRoot configuration);
    }
}
