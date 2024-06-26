﻿using Microsoft.Extensions.Caching.Distributed;
using MvcCoreElastiCacheAWS.Helpers;
using MvcCoreElastiCacheAWS.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Services
{
    public class ServiceAWSCache
    {
        //private IDatabase cache;
        private readonly IDistributedCache cache;

        public ServiceAWSCache(IDistributedCache cache)
        {
            //this.cache = HelperCacheRedis.Connection.GetDatabase();
            this.cache = cache;
        }

        public async Task<List<Coche>> GetcochesFavoritosAsync()
        {
            //almacenaremos una coleccion de cochesd en formato json
            //las keyus deben ser unicas para cada user

            //string jsonCoches = await this.cache.StringGetAsync("cochesfavoritos");
            string jsonCoches = await this.cache.GetStringAsync("cochesfavoritos");

            if (jsonCoches == null)
            {
                return null;
            }
            else 
            {
                List<Coche> cars = JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);

                return cars;
            }
        }

        public async Task AddCochesFavoritosAsync(Coche car)
        {
            List<Coche> coches = await this.GetcochesFavoritosAsync();

            //si no existen cocnes favoritos todavia creamos la coleccion 
            if (coches == null)
            {
                coches = new List<Coche>();
            }
            //añadiomos el nnuevo coche a la coleccion
            coches.Add(car);
            //serializamos a json la coleccion

            string jsonCoches = JsonConvert.SerializeObject(coches);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };
            //almacenamos la collecion dentro de cache redis
            //indicaremos que los datos duraran 30 minutos

            //await this.cache.StringSetAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));
            await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, options);


        }

        public async Task DeleteCochesFavoritosAync(int idcoche)
        {
            List<Coche> cars = await this.GetcochesFavoritosAsync();
            if (cars != null)
            {
                Coche cocheEliminar = cars.FirstOrDefault(x => x.IdCoche == idcoche);
                cars.Remove(cocheEliminar);

                //comprobamos si la coleccion tiene coches favoritos todavia o no.
                //si no tenemos coches, eliminamos la key de cache redis
                if (cars.Count == 0)
                {
                    //await this.cache.KeyDeleteAsync("cochesfavoritos");
                    await this.cache.RemoveAsync("cochesfavoritos");
                }
                else 
                {
                    //almacenamos de nuevo los coches sin el car eliminado

                    string jsonCoches = JsonConvert.SerializeObject(cars);
                    //actualizamos cache redis
                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30)
                    };
                    await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, options);


                }
            }

        }

    }
}
