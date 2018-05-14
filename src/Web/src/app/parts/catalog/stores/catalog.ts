import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BrandType, BrandModel, BrandListModel, BrandListType } from '../models/brands';
import { TypeType, TypeModel, TypeListModel, TypeListType } from '../models/types';
import { ProductModel, ProductType } from '../models/products';

const debug = new Debug('catalog');

export interface CatalogStoreType {
  products: Map<string, ProductType>;
  catalogType: TypeType;
  catalogBrand: BrandType;
  search: string;
  loading: boolean;

  readonly validation: any;
  readonly form: {[idx: string]: FieldDefinition};
  get: () => Promise<{}>;
}
export const CatalogStoreModel = types
  .model('CatalogStore',
  {
    products: types.optional(types.map(ProductModel), {}),

    catalogType: types.maybe(TypeModel),
    catalogBrand: types.maybe(BrandModel),

    search: types.optional(types.string, ''),

    loading: types.optional(types.boolean, false)
  })
  .views(self => ({
    get validation() {
      return;
    },
    get form(): { [idx: string]: FieldDefinition } {
      return ({
        search: {
          input: 'text',
          label: 'Search',
        },
        catalogType: {
          input: 'selecter',
          label: 'Catalog Type',
          selectStore: TypeListModel,
        },
        catalogBrand: {
          input: 'selecter',
          label: 'Catalog Brand',
          selectStore: BrandListModel,
        }
      });
    }
  }))
  .actions(self => {
    const get = flow(function*() {
      const request = new DTOs.Catalog();

      if (self.catalogBrand) {
        request.brandId = self.catalogBrand.id;
      }
      if (self.catalogType) {
        request.typeId = self.catalogType.id;
      }
      if (self.search) {
        request.search = self.search;
      }

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.PagedResponse<DTOs.ProductIndex> = yield client.paged(request);

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

    return { get };
  });
