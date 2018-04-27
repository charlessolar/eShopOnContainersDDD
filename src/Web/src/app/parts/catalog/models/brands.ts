import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { models, FormType, ComponentDefinition } from '../../../utils';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

const debug = new Debug('category brands');

export interface BrandFormType {
  id: string;
  brand: string;
  readonly validation: { brand: string };
  readonly valid: boolean;
  readonly form: { [idx: string]: ComponentDefinition };
  submit(): Promise<{}>;
}
export const BrandForm = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    brand: types.optional(types.string, '')
  })
  .views(self => ({
    get validation() {
      const validation = {
        brand: rules.brand
      };

      return validate(self, validation);
    }
  }))
  .views(self => ({
    get valid() {
      return self.validation === undefined;
    },
    get form(): {[idx: string]: ComponentDefinition} {
      return ({
        brand: {
          input: 'text',
          label: 'Brand',
          required: true,
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.AddCategoryBrand();

      request.brandId = self.id;
      request.brand = self.brand;

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

export interface BrandType {
  id: string;
  brand: string;
}
export const Brand = types
  .model('Catalog_Brand', {
    id: types.identifier(types.string),
    brand: types.string
  });

export interface ListType {
  entries: Map<string, BrandType>;
  loading: boolean;
  list: (term?: string, limit?: number) => Promise<{}>;
  add: (brand: BrandType) => void;
  remove: (id: string) => Promise<{}>;
}
export const List = types
  .model('Catalog_Brand_List', {
    entries: types.optional(types.map(Brand), {}),
    loading: types.optional(types.boolean, true)
  })
  .actions(self => {
    const list = flow(function*(term?: string, limit?: number) {
      const request = new DTOs.ListCategoryBrands();

      request.term = term;
      request.limit = limit || 10;

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.CategoryBrand> = yield client.paged(request);

        self.loading = false;
        results.records.forEach(record => {
          self.entries.put(record);
        });
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = (brand: BrandType) => {
        self.entries.put(Brand.create(brand));
    };
    const remove = flow(function*(id: string) {
      const request = new DTOs.RemoveCategoryBrand();

      request.brandId = id;

      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.CommandResponse = yield client.command(request);

        self.entries.delete(id);
      } catch (error) {
        debug('received http error: ', error);
      }
    });

    return {  list, add, remove };
  });

export interface BrandsType {
  List: ListType;
  brandForm: () => FormType<BrandFormType>;
}
export const Brands = types.model(
  'Catalog_Brands',
  {
    List: types.optional(List, {})
  }
).actions(self => ({
  brandForm() {
    const api = getEnv(self).api;
    return models.form(BrandForm, { api }, (model, success) => {
      if (!success) {
        return;
      }
      self.List.add({ id: model.id, brand: model.brand });
    });
  }
}));
