import { types, flow, getEnv, getParent, getSnapshot, applySnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { TypeListModel, TypeListType } from '../models/types';
import { BrandListModel, BrandListType } from '../models/brands';
import { ProductType, ProductListModel, ProductListType } from '../models/products';

const debug = new Debug('category product store');

export interface ProductFormType {
  id: string;
  name: string;
  description: string;
  price: number;
  catalogTypeId: string;
  catalogBrandId: string;

  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const ProductForm = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    name: types.maybe(types.string),
    description: types.maybe(types.string),
    price: types.maybe(types.number),
    catalogTypeId: types.maybe(types.string),
    catalogBrandId: types.maybe(types.string)
  })
  .views(self => ({
    get validation() {
      const validation = {
        name: rules.product.name,
        price: rules.product.price
      };

      return validate(self, validation);
    },
    get form(): { [idx: string]: FieldDefinition } {
      return ({
        name: {
          input: 'text',
          label: 'Name',
          required: true,
        },
        description: {
          input: 'textarea',
          label: 'Description',
        },
        price: {
          input: 'number',
          label: 'Price',
          required: true
        },
        catalogTypeId: {
          input: 'selecter',
          label: 'Catalog Type',
          required: true,
          projectionStore: TypeListModel,
          projection: async (store: TypeListType, term: string) => {
            await store.list(term);
            return Array.from(store.entries.values()).map(type => ({ id: type.id, label: type.type }));
          }
        },
        catalogBrandId: {
          input: 'selecter',
          label: 'Catalog Brand',
          required: true,
          projectionStore: BrandListModel,
          projection: async (store: BrandListType, term: string) => {
            await store.list(term);
            return Array.from(store.entries.values()).map(type => ({ id: type.id, label: type.brand }));
          }
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.AddProduct();

      request.productId = self.id;
      request.name = self.name;
      request.price = self.price;
      request.categoryBrandId = self.catalogBrandId;
      request.categoryTypeId = self.catalogTypeId;

      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.CommandResponse = yield client.command(request);

        // add new brand to list, reset form
        const store = getParent(self) as ProductsStoreType;
        setTimeout(() => store.add(getSnapshot(self)), 1);
      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { submit };
  });

export interface ProductsStoreType {
  list: ProductListType;
  form: ProductFormType;
  readonly loading: boolean;

  get: () => Promise<{}>;
  add: (type: ProductType) => void;
}
export const ProductsStoreModel = types.model(
  'Catalog_Products',
  {
    list: types.optional(ProductListModel, {}),
    form: types.optional(ProductForm, {})
  })
  .views(self => ({
    get loading() {
      return self.list.loading;
    }
  }))
  .actions(self => {
    const get = flow(function*() {
      yield self.list.list();
    });
    const add = (product: ProductType) => {
      self.list.add(product);
      self.form = ProductForm.create({});
    };

    return { add, get };
  });
