import { types, getRoot, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { models } from '../../../utils';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

const debug = new Debug('category types');

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
        brand: rules.brand
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
  add: (type: string) => Promise<{}>;
  remove: (id: string) => Promise<{}>;
}
export const List = types
  .model('Catalog_Type_List', {
    entries: types.map(Type),
    loading: types.boolean
  })
  .views(self => ({
    get client(): ApiClientType {
      return getRoot(this).api;
    }
  }))
  .actions(self => {
    const list = flow(function*(term?: string, limit?: number) {
      const request = new DTOs.ListCategoryTypes();

      request.term = term;
      request.limit = limit || 10;

      self.loading = true;
      try {
        const results: DTOs.PagedResponse<DTOs.CategoryType> = yield self.client.paged(request);

        self.loading = false;
        results.records.forEach(record => {
          self.entries.put(record);
        });
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = flow(function*(type: string) {
      const request = new DTOs.AddCategoryType();

      const model = Type.create({
        id: uuid(),
        type
      });

      request.typeId = model.id;
      request.type = model.type;

      try {
        const result: DTOs.CommandResponse = yield self.client.command(request);

        self.entries.set(model.id, model as any);
      } catch (error) {
        debug('received http error: ', error);
      }
    });
    const remove = flow(function*(id: string) {
      const request = new DTOs.RemoveCategoryType();

      request.typeId = id;

      try {
        const result: DTOs.CommandResponse = yield self.client.command(request);

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
    List: types.optional(List, { entries: {}, loading: true })
  }
);
