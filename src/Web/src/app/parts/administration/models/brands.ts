import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { models } from '../../../utils';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BrandType, BrandModel } from '../../../models/catalog/brands';

export { BrandType, BrandModel };

const debug = new Debug('catalog brands');

export interface BrandListType {
  entries: Map<string, BrandType>;
  loading: boolean;
  list: (term?: string, id?: string) => Promise<{}>;
  add: (brand: BrandType) => void;
  remove: (id: string) => Promise<{}>;
}
export const BrandListModel = types
  .model('Catalog_Brand_List', {
    entries: types.optional(types.map(BrandModel), {}),
    loading: types.optional(types.boolean, true)
  })
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
        self.loading = false;
      } catch (error) {
        debug('received http error: ', error);
      }

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

    return { list, add, remove };
  });
