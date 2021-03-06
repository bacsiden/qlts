// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DK.Web.DependencyResolution
{
    using DK.Application.Repositories;
    using MongoDB.Driver;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(m => m.FullName.StartsWith("DK."));
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            //For<DK.Framework.Models.DefaultConnection>().Use<DK.Framework.Models.DefaultConnection>().Singleton();

            //For<DefaultConnection>().Use<DefaultConnection>().Singleton();
            //For<DK.Framework.Services.TSqlService>().Use<DK.Framework.Services.TSqlService>().Singleton();
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var url = new MongoUrl(connectionString);
            var database = new MongoClient(url).GetDatabase(url.DatabaseName);
            For<IMongoDatabase>().Use(database);

            For<ITypeRepository>().Use<TypeRepository>().Singleton();
            For<ITaiSanRepository>().Use<TaiSanRepository>().Singleton();
        }

        #endregion
    }
}