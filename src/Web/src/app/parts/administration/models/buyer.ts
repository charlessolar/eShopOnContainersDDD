import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import { DateTime } from 'luxon';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BuyerIndexType as BuyerIndexTypeBase, BuyerIndexModel as BuyerIndexModelBase } from '../../../models/ordering/buyer';

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
