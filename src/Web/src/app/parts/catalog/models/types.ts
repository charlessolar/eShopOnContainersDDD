import { types, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { models, FormType, ComponentDefinition } from '../../../utils';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

const debug = new Debug('category types');

export interface TypeFormType {
  id: string;
  type: string;
  readonly validation: { type: string };
  readonly valid: boolean;
  readonly form: { [idx: string]: ComponentDefinition };
  submit(): Promise<{}>;
}
export const TypeForm = types
  .model({
    id: types.optional(types.identifier(types.string), uuid),
    type: types.optional(types.string, '')
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
    get valid() {
      return self.validation === undefined;
    },
    get form(): {[idx: string]: ComponentDefinition} {
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

      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
    });

    return { submit };
  });

export interface TypeType {
  id: string;
  type: string;
}
export const Type = types
  .model('Catalog_Type', {
    id: types.identifier(types.string),
    type: types.optional(types.string, '')
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
    get valid() {
      return self.validation !== undefined;
    }
  }));

export interface ListType {
  entries: Map<string, TypeType>;
  loading: boolean;
  list: (term?: string, limit?: number) => Promise<{}>;
  add: (type: TypeType) => void;
  remove: (id: string) => Promise<{}>;
}
export const List = types
  .model('Catalog_Type_List', {
    entries: types.optional(types.map(Type), {}),
    loading: types.optional(types.boolean, true)
  })
  .actions(self => {
    const list = flow(function*(term?: string, limit?: number) {
      const request = new DTOs.ListCategoryTypes();

      request.term = term;
      request.limit = limit || 10;

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.CategoryType> = yield client.paged(request);

        self.loading = false;
        results.records.forEach(record => {
          self.entries.put(record);
        });
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = (type: TypeType) => {
        self.entries.put(Type.create(type));
    };
    const remove = flow(function*(id: string) {
      const request = new DTOs.RemoveCategoryType();

      request.typeId = id;

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

export interface TypesType {
  List: ListType;
}
export const Types = types.model(
  'Catalog_Types',
  {
    List: types.optional(List, {})
  }
).actions(self => ({
  typeForm() {
    const api = getEnv(self).api;
    return models.form(TypeForm, { api }, (model, success) => {
      if (!success) {
        return;
      }
      self.List.add({ id: model.id, type: model.type });
    });
  }
}));
