import { types, flow, getEnv, getParent, applySnapshot, getSnapshot, onPatch, addDisposer, recordPatches } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';
import { when } from '../../../utils';

import { DTOs } from '../../../utils/eShop.dtos';
import { Data, DataModel, urlToBase64 } from '../../../utils/image';
import { ApiClientType } from '../../../stores';

import { TypeListType, TypeListModel, TypeType, TypeModel } from '../models/types';
import { BrandListType, BrandListModel, BrandType, BrandModel } from '../models/brands';

import BrandFormView from '../components/brandForm';
import TypeFormView from '../components/typeForm';

const debug = new Debug('product');

export interface ProductFormType {
  id: string;
  name: string;
  description: string;
  price: number;
  catalogType: TypeType;
  catalogBrand: BrandType;

  picture: Data;

  readonly editing: boolean;
  readonly form: { [idx: string]: FieldDefinition };
  readonly partial: {};
  submit: () => Promise<{}>;
  destroy: () => Promise<{}>;
}
export const ProductFormModel = types
  .model({
    id: types.optional(types.string, ''),
    name: types.maybe(types.string),
    description: types.maybe(types.string),
    price: types.maybe(types.number),
    catalogType: types.maybe(TypeModel),
    catalogBrand: types.maybe(BrandModel),
    picture: types.maybe(DataModel),

    dirty: types.optional(types.array(types.string), [])
  })
  .views(self => ({
    get editing() {
      return self.id !== '';
    },
    get partial() {
      // return a small object of only changed fields
      return self.dirty.reduce<{}>((prev, cur) => { prev[cur] = self[cur]; return prev; }, {});
    }
  }))
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
          disabled: self.editing
        },
        description: {
          input: 'textarea',
          label: 'Description',
        },
        price: {
          input: 'number',
          label: 'Price',
          required: true,
          normalize: 2
        },
        catalogType: {
          input: 'selecter',
          label: 'Catalog Type',
          required: true,
          selectStore: TypeListModel,
          addComponent: TypeFormView,
          disabled: self.editing
        },
        catalogBrand: {
          input: 'selecter',
          label: 'Catalog Brand',
          required: true,
          selectStore: BrandListModel,
          addComponent: BrandFormView,
          disabled: self.editing
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
    const afterCreate = () => {
      const disposer = onPatch(self, patch => {
        // todo: might be more interesting to use recordPatches
        const prop = patch.path.match(/\/([a-zA-Z]+)\/?/);
        if (prop[1] !== 'dirty' && self.dirty.indexOf(prop[1]) === -1) {
          self.dirty.push(prop[1]);
        }
      });
      addDisposer(self, disposer);
    };

    const addProduct = () => {
      const request = new DTOs.AddProduct();

      self.id = uuid();
      request.productId = self.id;
      request.name = self.name;
      request.price = self.price;
      request.catalogBrandId = self.catalogBrand.id;
      request.catalogTypeId = self.catalogType.id;

      return request;
    };
    const setPicture = () => {
      const request = new DTOs.SetPictureProduct();

      request.productId = self.id;
      request.content = self.picture.data as any;
      request.contentType = self.picture.contentType;

      return request;
    };
    const setDescription = () => {
      const request = new DTOs.UpdateDescriptionProduct();

      request.productId = self.id;
      request.description = self.description;

      return request;
    };
    const setPrice = () => {
      const request = new DTOs.UpdatePriceProduct();

      request.productId = self.id;
      request.price = self.price;

      return request;
    };

    const submit = flow(function*() {
      const requests = [
        ...when(self.id === '', addProduct),
        ...when(self.id !== '' && self.dirty.indexOf('price') !== -1, setPrice),
        ...when(self.dirty.indexOf('picture') !== -1, setPicture),
        ...when(self.dirty.indexOf('description') !== -1, setDescription)
      ];

      const client = getEnv(self).api as ApiClientType;
      for (let i = 0; i < requests.length; i++) {
        try {
          const result: DTOs.CommandResponse = yield client.command(requests[i]());
        } catch (error) {
          debug('received http error: ', error);
          throw error;
        }
      }

    });
    const destroy = flow(function*() {
      const client = getEnv(self).api as ApiClientType;
      const request = new DTOs.RemoveProduct();

      request.productId = self.id;

      try {
        const result: DTOs.CommandResponse = yield client.command(request);
      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { afterCreate, submit, destroy };
  });
