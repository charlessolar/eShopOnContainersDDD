import Debug from 'debug';
import { DateTime } from 'luxon';
import { FieldDefinition } from '../../../components/models';
import { OrderIndexModel as OrderIndexModelBase, OrderIndexType as OrderIndexTypeBase } from '../../../models/ordering/order';

const debug = new Debug('orders');

export interface OrderIndexType extends OrderIndexTypeBase {

  readonly placed: string;
  readonly formatting: {[idx: string]: FieldDefinition};
}
export const OrderIndexModel = OrderIndexModelBase
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
