import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import { BrandModel, BrandType } from '../../../models/catalog/brands';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';

export { BrandType, BrandModel };

const debug = new Debug('catalog brands');

export interface BrandListType {
  entries: Map<string, BrandType>;
  loading: boolean;
  list: (term?: string, id?: string) => Promise<{}>;
  add: (brand: BrandType) => void;
  remove: (id: string) => Promise<{}>;
  clear(): void;
  readonly projection: { id: string, label: string}[];
}
export const BrandListModel = types
  .model('Catalog_Brand_List', {
    entries: types.optional(types.map(BrandModel), {}),
    loading: types.optional(types.boolean, true)
  })
  .views(self => ({
    get projection() {
      return Array.from(self.entries.values()).map(x => ({ id: x.id, label: x.brand }));
    }
  }))
  .actions(self => {
    const list = flow(function*(term?: string, id?: string) {
      const request = new DTOs.ListCatalogBrands();

      request.term = term;
      request.id = id;

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.CatalogBrand> = yield client.paged(request);

        results.records.forEach(record => {
          self.entries.put(record);
        });
      } catch (error) {
        debug('received http error: ', error);
      }
      self.loading = false;
    });
    const add = (brand: BrandType) => {
        self.entries.put(brand);
    };
    const remove = flow(function*(id: string) {
      const request = new DTOs.RemoveCatalogBrand();

      request.brandId = id;

      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.CommandResponse = yield client.command(request);

        self.entries.delete(id);
      } catch (error) {
        debug('received http error: ', error);
      }
    });
    const clear = () => {
      self.entries.clear();
    };

    return { list, add, remove, clear };
  });
