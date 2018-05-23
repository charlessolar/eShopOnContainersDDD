import Debug from 'debug';
import { flow, getEnv, types } from 'mobx-state-tree';
import { FieldDefinition } from '../../../components/models';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';
import { OrderIndexModel, OrderIndexType } from '../models/order';

const debug = new Debug('orders');

export interface OrdersStoreType {
  orders: Map<string, OrderIndexType>;
  loading: boolean;

  orderStatus: string;

  period: { from: string, to: string};

  get: () => Promise<{}>;
  readonly form: {[idx: string]: FieldDefinition};
}

export const OrdersStoreModel = types
.model('OrderStore',
{
  orders: types.optional(types.map(OrderIndexModel), {}),

  orderStatus: types.maybe(types.string),

  period: types.maybe(types.model({
    from: types.string,
    to: types.string
  })),

  loading: types.optional(types.boolean, false)
})
.views(self => ({
  get form() {
    return ({
      orderStatus: {
        input: 'dropdown',
        label: 'Status',
        options: [
          { value: 'SUBMITTED', label: 'Submitted'},
          { value: 'WAITING_VALIDATION', label: 'Waiting Validation'},
          { value: 'CONFIRMED', label: 'Confirmed'},
          { value: 'PAID', label: 'Paid'},
          { value: 'SHIPPED', label: 'Shipped' },
          { value: 'NO_STOCK', label: 'Stock Exception'},
          { value: 'CANCELLED', label: 'Cancelled' }
        ]
      },
      period: {
        input: 'daterange',
        label: 'Period'
      }
    });
  }
}))
.actions(self => {
  const get = flow(function*() {
    const request = new DTOs.ListOrders();

    if (self.orderStatus) {
      request.orderStatus = self.orderStatus;
    }
    if (self.period && self.period.from) {
      request.from = self.period.from;
    }
    if (self.period && self.period.to) {
      request.to = self.period.to;
    }

    self.loading = true;
    try {
      const client = getEnv(self).api as ApiClientType;
      const result: DTOs.PagedResponse<DTOs.OrderingOrderIndex> = yield client.paged(request);

      self.orders.clear();
      result.records.forEach((record) => {
        self.orders.put(record);
      });
     } catch (error) {
      debug('received http error: ', error);
      throw error;
    }
    self.loading = false;
  });

  return { get };
});
