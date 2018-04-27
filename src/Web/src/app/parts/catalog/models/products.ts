import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { models, FormType, ComponentDefinition } from '../../../utils';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { Type, TypeType, List as Types, ListType as TypesType } from './types';
import { Brand, BrandType, List as Brands, ListType as BrandsType } from './brands';

const debug = new Debug('category brands');

export interface ProductFormType {
  id: string;
  name: string;
  description: string;
  price: number;
  catalogTypeId: string;
  catalogBrandId: string;

  readonly validation: { product: string };
  readonly valid: boolean;
  readonly form: { [idx: string]: ComponentDefinition };
  submit(): Promise<{}>;
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
    }
  }))
  .views(self => ({
    get valid() {
      return self.validation === undefined;
    },
    get form(): { [idx: string]: ComponentDefinition } {
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
          input: 'select',
          label: 'Catalog Type',
          required: true,
          projectionStore: Types,
          projection: async (store: TypesType, term: string) => {
            await store.list(term);
            return Array.from(store.entries.values()).map(type => ({ id: type.id, label: type.type }));
          }
        },
        catalogBrandId: {
          input: 'select',
          label: 'Catalog Brand',
          required: true,
          projectionStore: Brands,
          projection: async (store: BrandsType, term: string) => {
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

      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { submit };
  });

export interface ProductType {
  id: string;
  name: string;
  description: string;
  price: number;
  catalogTypeId: string;
  catalogType: string;
  catalogBrandId: string;
  catalogBrand: string;
}
export const Product = types
  .model('Catalog_Product', {
    id: types.identifier(types.string),
    name: types.string,
    description: types.maybe(types.string),
    price: types.number,
    catalogTypeId: types.string,
    catalogType: types.string,
    catalogBrandId: types.string,
    catalogBrand: types.string,
  });

export interface ListType {
  entries: Map<string, ProductType>;
  loading: boolean;
  list: () => Promise<{}>;
  add: (product: ProductType) => void;
  remove: (id: string) => Promise<{}>;
}

export const List = types
  .model('Catalog_Product_List', {
    entries: types.optional(types.map(Product), {}),
    loading: types.optional(types.boolean, true)
  })
  .actions(self => {
    const list = flow(function*() {
      const request = new DTOs.ListProducts();

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.Product> = yield client.paged(request);

        self.loading = false;
        results.records.forEach(record => {
          self.entries.put(record);
        });
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = (product: ProductType) => {
      self.entries.put(Product.create(product));
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

export interface ProductsType {
  List: ListType;
  productForm: () => FormType<ProductFormType>;
}
export const Products = types.model(
  'Catalog_Products',
  {
    List: types.optional(List, {})
  }
).actions(self => ({
  productForm() {
    const api = getEnv(self).api;
    return models.form(ProductForm, { api }, (model, success) => {
      if (!success) {
        return;
      }
      self.List.add({
        id: model.id,
        name: model.name,
        description: model.description,
        price: model.price,
        catalogBrandId: model.catalogBrandId,
        catalogBrand: '',
        catalogTypeId: model.catalogTypeId,
        catalogType: ''
      });
    });
  }
}));
