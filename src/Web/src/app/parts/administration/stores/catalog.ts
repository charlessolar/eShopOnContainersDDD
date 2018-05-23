import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import { ProductType as ProductTypeBase } from '../../../models/catalog/products';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';
import { ProductModel, ProductType } from '../models/products';

const debug = new Debug('catalog');

export interface CatalogStoreType {
  products: Map<string, ProductType>;
  loading: boolean;

  get: () => Promise<{}>;
  add: (product: ProductTypeBase) => void;
  remove: (id: string) => void;
}

export const CatalogStoreModel = types
.model('CatalogStore',
{
  products: types.optional(types.map(ProductModel), {}),

  loading: types.optional(types.boolean, false)
})
.actions(self => {
  const get = flow(function*() {
    const request = new DTOs.Catalog();

    self.loading = true;
    try {
      const client = getEnv(self).api as ApiClientType;
      const result: DTOs.PagedResponse<DTOs.CatalogProductIndex> = yield client.paged(request);

      self.products.clear();
      result.records.forEach((record) => {
        self.products.put(record);
      });
     } catch (error) {
      debug('received http error: ', error);
      throw error;
    }
    self.loading = false;
  });
  const add = (product: ProductTypeBase) => {
    self.products.put(product);
  };
  const remove = (id: string) => {
    self.products.delete(id);
  };

  return { get, add, remove };
});
