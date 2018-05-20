import { types, getEnv, flow } from 'mobx-state-tree';
import Debug from 'debug';

import rules from '../validation';
import { models } from '../../../utils';
import { FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { PaymentMethodType, PaymentMethodModel } from '../../../models/ordering/paymentMethod';

const debug = new Debug('buyer payment method');

export { PaymentMethodType, PaymentMethodModel };

export interface PaymentMethodListType {
  entries: Map<string, PaymentMethodType>;
  loading: boolean;
  list: (term?: string, id?: string) => Promise<{}>;
  add: (type: PaymentMethodType) => void;
  clear(): void;
  readonly projection: { id: string, label: string}[];
}
export const PaymentMethodListModel = types
  .model('Buyer_PaymentMethod_List', {
    entries: types.optional(types.map(PaymentMethodModel), {}),
    loading: types.optional(types.boolean, true)
  })
  .views(self => ({
    get projection() {
      return Array.from(self.entries.values()).map(x => ({ id: x.id, label: x.alias + ' - ' + x.cardType }));
    }
  }))
  .actions(self => {
    const list = flow(function*(term?: string, id?: string) {
      const request = new DTOs.ListPaymentMethods();

      request.term = term;
      request.id = id;

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;
        const results: DTOs.PagedResponse<DTOs.PaymentMethod> = yield client.paged(request);

        results.records.forEach(record => {
          self.entries.put(record);
        });
        self.loading = false;
      } catch (error) {
        debug('received http error: ', error);
      }

    });
    const add = (method: PaymentMethodType) => {
        self.entries.put(method);
    };
    const clear = () => {
      self.entries.clear();
    };

    return { list, add, clear };
  });
