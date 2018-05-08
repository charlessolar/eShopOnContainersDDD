import { types, flow, getEnv, getParent, getSnapshot, applySnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { TypeType, TypeListModel, TypeListType } from '../models/types';

const debug = new Debug('category type store');

export interface TypeFormType {
  id: string;
  type: string;
  readonly form: { [idx: string]: FieldDefinition };
  submit: () => Promise<{}>;
}
export const TypeForm = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    type: types.maybe(types.string)
  })
  .views(self => ({
    get validation() {
      const validation = {
        type: rules.type
      };

      return validate(self, validation);
    }
  }))
  .views(self => ({
    get form(): {[idx: string]: FieldDefinition} {
      return ({
        type: {
          input: 'text',
          label: 'Type',
          required: true,
        }
      });
    }
  }))
  .actions(self => {
    const submit = flow(function*() {
      const request = new DTOs.AddCategoryType();

      request.typeId = self.id;
      request.type = self.type;

      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.CommandResponse = yield client.command(request);

        // add new type to list, reset form
        const store = getParent(self) as TypesStoreType;
        setTimeout(() => store.add(getSnapshot(self)), 1);
      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { submit };
  });

export interface TypesStoreType {
  list: TypeListType;
  form: TypeFormType;
  readonly loading: boolean;

  get: () => Promise<{}>;
  add: (type: TypeType) => void;
}
export const TypesStoreModel = types.model(
  'Catalog_Types',
  {
    list: types.optional(TypeListModel, {}),
    form: types.optional(TypeForm, {})
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
    const add = (type: TypeType) => {
      self.list.add(type);
      self.form = TypeForm.create({});
    };

    return { add, get };
  });
