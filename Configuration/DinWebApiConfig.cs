using DinnamuSWebApi.Filters;
using DinnamuSWebApi.Infraestructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DinnamuSWebApi.Configuration
{
    public static class DinWebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*
             * Altera o Serializador padrão dos retornos (responses) da Web Api para Json.
             * Não achei produtivo, uma vez que é necessário também mudar os valores de Content-Type e Accept
             * no cabeçalho da requisição.
             * Preferível usar o retorno IHttpActionResult para Json()
             * config.Formatters.JsonFormatter.UseDataContractJsonSerializer = true;
             */
            

            /*
            Evita o loop infinito por relacionamentos nas propriedade de navegação (reference loop).
            Antes porém, faz-se necessário desabilitar o LazyLoading no construtor da classe que implementa
            DBContext(No caso de usar EntityFramework):
            this.Configuration.LazyLoadingEnable = false;
            */
            var json = config.Formatters.JsonFormatter.SerializerSettings;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //Implementa a Injeção de Dependencia na app:
            config.DependencyResolver = new NinjectResolver();


            config.Filters.Add(new GeneralExceptionFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{Controller}/{id}",
                defaults: new {id = RouteParameter.Optional }
                );

            //IHttpRoute defaultRoute = config.Routes.CreateRoute("api/{controller}",
                                           // null, null);

            // Add route
            //config.Routes.Add("DefaultApi", defaultRoute);

            
            
        }
    }
}