using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Scheduler.Log;
using System;
using System.Collections.Generic;
using System.Configuration;

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
            pipelines.AfterRequest += ctx =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*");
                ctx.Response.WithHeader("Access-Control-Allow-Headers", "Content-Type");
            };
        }
    }
}