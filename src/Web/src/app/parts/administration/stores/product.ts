import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { Data, DataModel, urlToBase64 } from '../../../utils/image';
import { ApiClientType } from '../../../stores';

import { TypeListType, TypeListModel, TypeType, TypeModel } from '../models/types';
import { BrandListType, BrandListModel, BrandType, BrandModel } from '../models/brands';

const debug = new Debug('product');

export interface ProductFormType {
  id: string;
  name: string;
  description: string;
  price: number;
  catalogType: TypeType;
  catalogBrand: BrandType;

  picture: Data;

  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const ProductFormModel = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    name: types.maybe(types.string),
    description: types.maybe(types.string),
    price: types.maybe(types.number),
    catalogType: types.maybe(TypeModel),
    catalogBrand: types.maybe(BrandModel),
    picture: types.maybe(DataModel)
  })
  .views(self => ({
    get validation() {
      const validation = {
        name: rules.product.name,
        price: rules.product.price,
        catalogType: rules.product.catalogType,
        catalogBrand: rules.product.catalogBrand,
        picture: rules.product.picture
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
        catalogType: {
          input: 'selecter',
          label: 'Catalog Type',
          required: true,
          projectionStore: TypeListModel,
          projection: async (store: TypeListType, term: string, id?: string) => {
            await store.list(term, id);
            return Array.from(store.entries.values()).map(type => ({ id: type.id, label: type.type }));
          },
          select: (store: TypeListType, id: string) => {
            return store.entries.get(id);
          },
          getIdentity: (model: TypeType) => {
            return model.id;
          }
        },
        catalogBrand: {
          input: 'selecter',
          label: 'Catalog Brand',
          required: true,
          projectionStore: BrandListModel,
          projection: async (store: BrandListType, term: string, id?: string) => {
            await store.list(term, id);
            return Array.from(store.entries.values()).map(type => ({ id: type.id, label: type.brand }));
          },
          select: (store: BrandListType, id: string) => {
            return store.entries.get(id);
          },
          getIdentity: (model: BrandType) => {
            return model.id;
          }
        },
        picture: {
          input: 'image',
          label: 'Picture',
          imageRatio: 1,
          required: true,
        },
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.AddProduct();

      request.productId = self.id;
      request.name = self.name;
      request.price = self.price;
      request.catalogBrandId = self.catalogBrand.id;
      request.catalogTypeId = self.catalogType.id;

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
