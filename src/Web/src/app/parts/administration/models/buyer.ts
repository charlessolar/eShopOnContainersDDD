import Debug from 'debug';
import { DateTime } from 'luxon';
import { FormatDefinition } from '../../../components/models';
import { BuyerIndexModel as BuyerIndexModelBase, BuyerIndexType as BuyerIndexTypeBase } from '../../../models/ordering/buyer';

const debug = new Debug('buyers');

export interface BuyerIndexType extends BuyerIndexTypeBase {

    readonly lastOrderPlaced: string;
    readonly formatting: {[idx: string]: FormatDefinition};
}
export const BuyerIndexModel = BuyerIndexModelBase
  .views(self => ({
    get formatting() {
      return ({
        totalOrders: {
          trim: true
        },
        totalSpent: {
          currency: true,
          normalize: 2
        }
      });
    },
    get lastOrderPlaced() {
      return DateTime.fromMillis(self.lastOrder).toFormat('DDD');
    }
  }));
