import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import { AddressModel, AddressType } from '../../../models/ordering/address';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';

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
