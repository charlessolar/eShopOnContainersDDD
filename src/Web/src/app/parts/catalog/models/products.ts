import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { models } from '../../../utils';
import { FormatDefinition, FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { TypeModel, TypeType, TypeListModel, TypeListType } from './types';
import { BrandModel, BrandType, BrandListModel, BrandListType } from './brands';

const debug = new Debug('catalog products');

export interface ProductType {
  id: string;
  name: string;
  description: string;
  price: number;
  catalogTypeId: string;
  catalogType: string;
  catalogBrandId: string;
  catalogBrand: string;

  availableStock: number;
  restockThreshold: number;
  maxStockThreshold: number;

  onReorder: boolean;

  pictureContents: string;
  pictureContentType: string;

  readonly formatting?: {[idx: string]: FormatDefinition };
  readonly productPicture?: string;
  readonly canOrder?: boolean;
}
export const ProductModel = types
  .model('Catalog_Product', {
    id: types.identifier(types.string),
    name: types.string,
    description: types.maybe(types.string),
    price: types.number,
    catalogTypeId: types.string,
    catalogType: types.string,
    catalogBrandId: types.string,
    catalogBrand: types.string,
    availableStock: types.number,
    restockThreshold: types.number,
    maxStockThreshold: types.number,
    onReorder: types.boolean,
    pictureContents: types.maybe(types.string),
    pictureContentType: types.maybe(types.string)
  })
  .views(self => ({
    get formatting() {
      return ({
        price: {
          currency: true,
          normalize: 2
        }
      });
    },
    get productPicture() {
      if (!self.pictureContents) {
        return;
      }
      return 'data:' + self.pictureContentType + ';base64,' + self.pictureContents;
    },
    get canOrder() {
      return self.availableStock > 0 || self.onReorder;
    }
  }));

export interface ProductListType {
  entries: Map<string, ProductType>;
  loading: boolean;
  list: () => Promise<{}>;
  add: (product: ProductType) => void;
  remove: (id: string) => Promise<{}>;
}

export const ProductListModel = types
  .model('Catalog_Product_List', {
    entries: types.optional(types.map(ProductModel), {}),
    loading: types.optional(types.boolean, true)
  })
  .actions(self => {
    const list = flow(function*() {
      const request = new DTOs.ListProducts();

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.ProductIndex> = yield client.paged(request);

        results.records.forEach(record => {
          self.entries.put(record);
        });
        self.loading = false;
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = (product: ProductType) => {
      self.entries.put(product);
    };
    const remove = flow(function*(id: string) {
      const request = new DTOs.RemoveProduct();

      request.productId = id;

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
