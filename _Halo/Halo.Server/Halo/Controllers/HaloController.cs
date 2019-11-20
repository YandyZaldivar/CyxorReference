/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Halo.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;

    class HaloController : BaseController
    {
        /*
        TApiModel NewApiModel<TApiModel, TModel>(TModel model)
            where TApiModel : IIdNombreApiModel, new()
            where TModel : IIdNombreApiModel
            => new TApiModel { Id = model.Id, Nombre = model.Nombre };

        public async Task<IEnumerable<TApiModel>> List<TApiModel, TListApiModel, TModel>(TListApiModel model, DbSet<TModel> dbSet)
        where TModel : class, IIdNombreApiModel
            where TApiModel : IIdNombreApiModel, new()
            where TListApiModel : ListApiModel
        {
            var entries = new List<TApiModel>();

            if (model.Id != 0)
                entries.Add(NewApiModel<TApiModel, TModel>(await HaloDbContext.Areas.FindAsync(model.Id).ConfigureAwait(false) as TModel));
            else
            {
                var items = await Query(dbSet.OrderBy(p => p.Id), model).ToListAsync().ConfigureAwait(false);

                foreach (var item in items)
                    entries.Add(NewApiModel<TApiModel, TModel>(item as TModel));
            }

            return entries;
        }

        public Task<IEnumerable<AreaApiModel>> List(AreaListApiModel model)
            => List<AreaApiModel, AreaListApiModel, Area>(model, HaloDbContext.Areas);

        public Task<IEnumerable<CausaMuerteDirectaApiModel>> List(CausaMuerteDirectaListApiModel model)
            => List<CausaMuerteDirectaApiModel, CausaMuerteDirectaListApiModel, CausaMuerteDirecta>(model, HaloDbContext.CausasMuerteDirecta);

        public Task<IEnumerable<CausaMuerteIndirectaApiModel>> List(CausaMuerteIndirectaListApiModel model)
            => List<CausaMuerteIndirectaApiModel, CausaMuerteIndirectaListApiModel, CausaMuerteIndirecta>(model, HaloDbContext.CausasMuerteIndirecta);

        public Task<IEnumerable<EscolaridadApiModel>> List(EscolaridadListApiModel model)
            => List<EscolaridadApiModel, EscolaridadListApiModel, Escolaridad>(model, HaloDbContext.Escolaridades);

        //public async Task<IEnumerable<Hemorragia>> List(HemorragiaListApiModel model)
        //    => await Query(HaloDbContext.Hemorragias.OrderBy(p => p.Id), model).ToListAsync();

        public Task<IEnumerable<IndiceMasaCorporalApiModel>> List(IndiceMasaCorporalListApiModel model)
            => List<IndiceMasaCorporalApiModel, IndiceMasaCorporalListApiModel, IndiceMasaCorporal>(model, HaloDbContext.IndicesMasaCorporal);

        public Task<IEnumerable<MorbilidadPartoApiModel>> List(MorbilidadPartoListApiModel model)
            => List<MorbilidadPartoApiModel, MorbilidadPartoListApiModel, MorbilidadParto>(model, HaloDbContext.MorbilidadPartos);

        //public async Task<IEnumerable<Ocitocico>> List(OcitocicoListApiModel model)
        //    => await Query(HaloDbContext.Ocitocicos.OrderBy(p => p.Id), model).ToListAsync();

        public Task<IEnumerable<OcupacionApiModel>> List(OcupacionListApiModel model)
            => List<OcupacionApiModel, OcupacionListApiModel, Ocupacion>(model, HaloDbContext.Ocupaciones);

        public Task<IEnumerable<PartoApiModel>> List(PartoListApiModel model)
            => List<PartoApiModel, PartoListApiModel, Parto>(model, HaloDbContext.Partos);
        */
    }
}
/* { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave */
