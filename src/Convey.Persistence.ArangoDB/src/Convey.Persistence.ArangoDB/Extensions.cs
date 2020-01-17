using System;
using Convey.Persistence.ArangoDB.Builders;
using Convey.Persistence.ArangoDB.Initializers;
using Convey.Persistence.ArangoDB.Repositories;
using Convey.Types;
using Microsoft.Extensions.DependencyInjection;
using ArangoDB.Client;
using System.Net;

namespace Convey.Persistence.ArangoDB
{
    public static class Extensions
    {
        private const string SectionName = "arango";
        private const string RegistryName = "persistence.arangoDb";

        public static IConveyBuilder AddArango(this IConveyBuilder builder, string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = SectionName;
            }
            
            var arangoOptions = builder.GetOptions<ArangoDbOptions>(sectionName);
            return builder.AddArango(arangoOptions);
        }

        public static IConveyBuilder AddArango(this IConveyBuilder builder, Func<IArangoDbOptionsBuilder, IArangoDbOptionsBuilder> buildOptions)
        {
            var arangoOptions = buildOptions(new ArangoDbOptionsBuilder()).Build();
            return builder.AddArango(arangoOptions);
        }

        public static IConveyBuilder AddArango(this IConveyBuilder builder, ArangoDbOptions options)
        {
            if (!builder.TryRegister(RegistryName))
            {
                return builder;
            }

            builder.Services.AddSingleton(options);
            builder.Services.AddTransient(sp => ArangoDatabase.CreateWithSetting());
            builder.Services.AddTransient<IArangoDbInitializer, ArangoDbInitializer>();

            builder.AddInitializer<IArangoDbInitializer>();

            return builder;
        }

        public static IConveyBuilder AddArangoRepository<TEntity, TIdentifiable>(this IConveyBuilder builder)
            where TEntity : IIdentifiable<TIdentifiable>
        {
            builder.Services.AddTransient<IArangoRepository<TEntity, TIdentifiable>>(sp =>
            {
                var database = sp.GetService<IArangoDatabase>();
                return new ArangoRepository<TEntity, TIdentifiable>(database);
            });

            return builder;
        }
    }
}