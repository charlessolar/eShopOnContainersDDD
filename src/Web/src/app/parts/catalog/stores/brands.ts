import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BrandType, BrandListModel, BrandListType } from '../models/brands';

const debug = new Debug('catalog brand store');

export interface BrandFormType {
  id: string;
  brand: string;
  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const BrandForm = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    brand: types.maybe(types.string)
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
    get form(): {[idx: string]: FieldDefinition} {
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

        // add new brand to list, reset form
        const store = getParent(self) as BrandsStoreType;
        setTimeout(() => store.add(getSnapshot(self)), 1);
       } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { submit };
  });

export interface BrandsStoreType {
  list: BrandListType;
  form: BrandFormType;
  readonly loading: boolean;

  get: () => Promise<{}>;
  add: (brand: BrandType) => void;
}
export const BrandsStoreModel = types.model(
  'Catalog_Brands',
  {
    list: types.optional(BrandListModel, {}),
    form: types.optional(BrandForm, {})
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
    const add = (brand: BrandType) => {
      self.list.add(brand);
      self.form = BrandForm.create({});
    };

    return { add, get };
  });
