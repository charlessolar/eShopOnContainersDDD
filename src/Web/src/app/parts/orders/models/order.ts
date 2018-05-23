import Debug from 'debug';
import { DateTime } from 'luxon';
import { types } from 'mobx-state-tree';
import { FieldDefinition } from '../../../components/models';
import { OrderModel as OrderModelBase, OrderType as OrderTypeBase } from '../../../models/ordering/order';
import { OrderItemModel, OrderItemType } from '../models/item';

const debug = new Debug('orders');

export interface OrderType extends OrderTypeBase {
  items: OrderItemType[];

  readonly placed: string;
  readonly formatting: {[idx: string]: FieldDefinition};
}
export const OrderModel = OrderModelBase
  .props({
    items: types.array(OrderItemModel)
  })
  .views(self => ({
    get formatting() {
      return ({
        totalItems: {
          trim: true,
        },
        totalQuantity: {
          trim: true
        },
        subTotal: {
          currency: true,
          normalize: 2
        },
        additional: {
          currency: true,
          normalize: 2
        },
        total: {
          currency: true,
          normalize: 2
        }
      });
    },
    get placed() {
      return DateTime.fromMillis(self.created).toFormat('DDD');
    }
  }));
