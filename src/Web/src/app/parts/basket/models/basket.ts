import Debug from 'debug';
import { DateTime } from 'luxon';
import { FormatDefinition } from '../../../components/models';
import { BasketModel as BasketModelBase, BasketType as BasketTypeBase } from '../../../models/basket/baskets';
import { ItemIndexType } from '../../../models/basket/items';

const debug = new Debug('basket');

export interface BasketType extends BasketTypeBase {
  subtractItem: (item: ItemIndexType) => void;
  additionItem: (item: ItemIndexType) => void;

  readonly createdDate?: DateTime;
  readonly updatedDate?: DateTime;
  readonly formatting?: { [idx: string]: FormatDefinition };
}
export const BasketModel = BasketModelBase
  .actions(self => ({
    subtractItem(item: ItemIndexType) {
      self.totalItems--;
      self.totalQuantity -= item.quantity;
      self.subTotal -= item.subTotal;
    },
    additionItem(item: ItemIndexType) {
      self.totalItems++;
      self.totalQuantity += item.quantity;
      self.subTotal += item.subTotal;
    }
  }))
  .views(self => ({
    get createdDate() {
      return DateTime.fromMillis(self.created);
    },
    get updatedDate() {
      return DateTime.fromMillis(self.updated);
    },
    get formatting() {
      return ({
        subTotal: {
          currency: true,
          normalize: 2
        },
        totalFees: {
          currency: true,
          normalize: 2
        },
        totalTaxes: {
          currency: true,
          normalize: 2
        },
        total: {
          currency: true,
          normalize: 2
        }
      });
    },
  }));
