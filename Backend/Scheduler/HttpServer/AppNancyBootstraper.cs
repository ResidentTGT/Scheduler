using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Scheduler.Log;
using System;
using System.Collections.Generic;

namespace Scheduler.HttpServer
{
    public class AppNancyBootstrapper : DefaultNancyBootstrapper
    {
        private readonly Dictionary<Type, object> _dependencies;

        public AppNancyBootstrapper(Dictionary<Type, object> dependencies = null)
        {
            _dependencies = dependencies ?? new Dictionary<Type, object>();
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/", "wwwroot"));
            base.ConfigureConventions(nancyConventions);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            foreach (var dependency in _dependencies)
                container.Register(dependency.Key, dependency.Value);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += (NancyContext ctx) =>
            {
                Logger.Log( $"Handler is called. {ctx.Request.Url}");

                return null;
            };

            pipelines.AfterRequest += (NancyContext ctx) =>
            {
                var reason = string.IsNullOrWhiteSpace(ctx.Response.ReasonPhrase)
                    ? ctx.Response.StatusCode.ToString()
                    : ctx.Response.ReasonPhrase;

                Logger.Log($"Request successfully complete ({(int)ctx.Response.StatusCode} - {reason}) {ctx.Request.Url} ");
            };

            pipelines.OnError += (NancyContext ctx, Exception ex) =>
            {
                Logger.Log($"Request failed. {ex.Message}", LogLevel.Warn );

                return null;
            };

            /* === CORS === */

            pipelines.AfterRequest += ctx =>
            {
                ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST, GET, DELETE, PUT, OPTIONS")
                    .WithHeader("Access-Control-Allow-Headers", "Content-Type");
            };
        }
    }
}