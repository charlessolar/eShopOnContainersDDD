import { types, getEnv, flow } from 'mobx-state-tree';
import Debug from 'debug';

import rules from '../validation';
import { models } from '../../../utils';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { AddressType, AddressModel } from '../../../models/ordering/address';

const debug = new Debug('buyer address');

export { AddressType, AddressModel };

export interface AddressListType {
  entries: Map<string, AddressType>;
  loading: boolean;
  list: (term?: string, id?: string) => Promise<{}>;
  add: (type: AddressType) => void;
  clear(): void;
  readonly projection: { id: string, label: string}[];
}
export const AddressListModel = types
  .model('Buyer_Address_List', {
    entries: types.optional(types.map(AddressModel), {}),
    loading: types.optional(types.boolean, true)
  })
  .views(self => ({
    get projection() {
      return Array.from(self.entries.values()).map(x => ({ id: x.id, label: x.alias + ' - ' + x.street }));
    }
  }))
  .actions(self => {
    const list = flow(function*(term?: string, id?: string) {
      const request = new DTOs.ListAddresses();

      request.term = term;
      request.id = id;

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.Address> = yield client.paged(request);

        results.records.forEach(record => {
          self.entries.put(record);
        });
        self.loading = false;
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = (address: AddressType) => {
        self.entries.put(address);
    };
    const clear = () => {
      self.entries.clear();
    };

    return { list, add, clear };
  });
