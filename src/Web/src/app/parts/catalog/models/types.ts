import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import { TypeModel, TypeType } from '../../../models/catalog/types';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';

export { TypeType, TypeModel };

const debug = new Debug('catalog types');

export interface TypeListType {
  entries: Map<string, TypeType>;
  loading: boolean;
  list: (term?: string, id?: string) => Promise<{}>;
  add: (type: TypeType) => void;
  remove: (id: string) => Promise<{}>;
  clear(): void;
  readonly projection: { id: string, label: string}[];
}
export const TypeListModel = types
  .model('Catalog_Type_List', {
    entries: types.optional(types.map(TypeModel), {}),
    loading: types.optional(types.boolean, true)
  })
  .views(self => ({
    get projection() {
      return Array.from(self.entries.values()).map(x => ({ id: x.id, label: x.type }));
    }
  }))
  .actions(self => {
    const list = flow(function*(term?: string, id?: string) {
      const request = new DTOs.ListCatalogTypes();

      request.term = term;
      request.id = id;

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.CatalogType> = yield client.paged(request);

        results.records.forEach(record => {
          self.entries.put(record);
        });
        self.loading = false;
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = (type: TypeType) => {
        self.entries.put(type);
    };
    const remove = flow(function*(id: string) {
      const request = new DTOs.RemoveCatalogType();

      request.typeId = id;

      try {
        const client = getEnv(self).api as ApiClientType;
        const result: DTOs.CommandResponse = yield client.command(request);

        self.entries.delete(id);
      } catch (error) {
        debug('received http error: ', error);
      }
    });
    const clear = () => {
      self.entries.clear();
    };

    return { list, add, remove, clear };
  });
