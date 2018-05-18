import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { DateTime } from 'luxon';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { models } from '../../../utils';
import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { ItemIndexType } from '../../../models/basket/items';
import { BasketType as BasketTypeBase, BasketModel as BasketModelBase } from '../../../models/basket/baskets';

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
